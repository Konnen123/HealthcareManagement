using Microsoft.ML;

namespace Application.ML;

public class DiagnosisPredictionModel
{
    private readonly MLContext _mlContext;
    private ITransformer _model;
    private readonly string _modelPath;
    public DiagnosisPredictionModel(string modelPath)
    {
        _mlContext = new MLContext();
        _modelPath = modelPath;
    }

    public void Train(string csvFilePath)
    {
        var data = _mlContext.Data.LoadFromTextFile<DiagnosisData>(csvFilePath, hasHeader: true, separatorChar: ',');

        var splitData = _mlContext.Data.TrainTestSplit(data, testFraction: 0.2);

        var pipeline = _mlContext.Transforms.Conversion.MapValueToKey("Label", nameof(DiagnosisData.Diseases)) // Map target to key
            .Append(_mlContext.Transforms.Concatenate("Features", nameof(DiagnosisData.Features))) // Combine features
            .Append(_mlContext.MulticlassClassification.Trainers.LbfgsMaximumEntropy( // Train with SDCA
                labelColumnName: "Label",
                featureColumnName: "Features"))
            .Append(_mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel")); // Decode the predicted label

        _model = pipeline.Fit(splitData.TrainSet);

        var predictions = _model.Transform(splitData.TestSet);
        var metrics = _mlContext.MulticlassClassification.Evaluate(predictions, labelColumnName: "Label", scoreColumnName: "Score");

        Console.WriteLine($"MacroAccuracy: {metrics.MacroAccuracy:F2}");
        Console.WriteLine($"MicroAccuracy: {metrics.MicroAccuracy:F2}");
        Console.WriteLine($"LogLoss: {metrics.LogLoss:F2}");
        Console.WriteLine($"LogLossReduction: {metrics.LogLossReduction:F2}");
    }
    
    public void SaveModel(string modelPath)
    {
        _mlContext.Model.Save(_model, null, modelPath);
        Console.WriteLine($"Model saved to {modelPath}");
    }
    
    public ITransformer LoadModel(string modelPath)
    {
        _model = _mlContext.Model.Load(modelPath, out _);
        Console.WriteLine($"Model loaded from {modelPath}");
        return _model;
    }
    
    public string Predict(List<string> symptoms, string csvPath)
    {
        var featureVector = CreateFeatureVector(symptoms, csvPath);
        if (featureVector.Length != 382)
        {
            foreach (var elem in featureVector)
            {
                Console.Write(elem + " ");
            }
            throw new InvalidOperationException($"Feature vector size mismatch. Expected 377, but got {featureVector.Length}.");
        }

        var diagnosisInput = new DiagnosisData
        {
            Features = featureVector
        };
        var predictionEngine = _mlContext.Model.CreatePredictionEngine<DiagnosisData, DiagnosisPrediction>(LoadModel(_modelPath));

        var prediction = predictionEngine.Predict(diagnosisInput);
        Console.WriteLine($"Predicted disease: {prediction.Diseases}");
        return prediction.Diseases;
    }
    public static float[] CreateFeatureVector(List<string> symptoms, string csvPath)
    {
        try
        {
            var columnNames = File.ReadLines(csvPath)
                .First()
                .Split(',')
                .Skip(1)
                .ToArray();

            var featureVector = new float[columnNames.Length];

            var normalizedSymptoms = symptoms
                .Select(s => s.Trim().ToLower())
                .ToList();

            for (var i = 0; i < columnNames.Length; i++)
            {
                var normalizedColumnName = columnNames[i].Trim().ToLower();

                if (normalizedSymptoms.Any(s => normalizedColumnName.Contains(s)))
                {
                    featureVector[i] = 1;
                }
            }

            return featureVector;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}