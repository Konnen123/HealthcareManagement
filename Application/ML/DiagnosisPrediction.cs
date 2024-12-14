using Microsoft.ML.Data;

namespace Application.ML;

public class DiagnosisPrediction
{
    [ColumnName("PredictedLabel")]
    public string Diseases { get; set; }
}