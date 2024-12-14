using Microsoft.ML.Data;

namespace Application.ML;

public class DiagnosisData
{
    [LoadColumn(0)]
    public string Diseases { get; set; }

    [LoadColumn(1, 377)]
    [VectorType(377)]
    public float[] Features { get; set; }
}