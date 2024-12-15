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
    
    public DiagnosisResponse Predict(List<string> symptoms, string csvPath)
    {
        var (featureVector, warnings) = CreateFeatureVector(symptoms, csvPath);
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
        
        return new DiagnosisResponse
        {
            Disease = prediction.Diseases,
            Warnings = warnings
        };
    }
    
    public static (float[] FeatureVector, string Warnings) CreateFeatureVector(List<string> symptoms, string csvPath)
    {
        try
        {
            // Read and normalize the column names from the CSV file
            var columnNames = File.ReadLines(csvPath)
                .First()
                .Split(',')
                .Skip(1)
                .Select(c => c.Trim().ToLower())
                .ToArray();

            // Normalize the symptoms for comparison
            var normalizedSymptoms = symptoms
                .Select(s => s.Trim().ToLower())
                .ToList();

            // Initialize the feature vector
            var featureVector = new float[columnNames.Length];

            // Track unmatched symptoms
            var unmatchedSymptoms = new List<string>();

            // Map symptoms to the feature vector
            foreach (var symptom in normalizedSymptoms)
            {
                // Check if the symptom matches any column name
                bool isMatched = false;
                for (var i = 0; i < columnNames.Length; i++)
                {
                    if (columnNames[i].Contains(symptom))
                    {
                        featureVector[i] = 1;
                        isMatched = true;
                    }
                }

                // If no match found, add to unmatched symptoms
                if (!isMatched)
                {
                    unmatchedSymptoms.Add(symptom);
                }
            }

            // Construct the warning message
            var warningMessage = unmatchedSymptoms.Any()
                ? string.Join(" ", unmatchedSymptoms.Select(s => $"Given symptom '{s}' doesn't exist in the features table."))
                : string.Empty;

            // Return the feature vector and warning message
            return (featureVector, warningMessage);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: {e.Message}");
            throw;
        }
    }


}