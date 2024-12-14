using Application.ML;
using Microsoft.AspNetCore.Mvc;

namespace HealthcareManagement.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class DiseasePredictionController : ControllerBase
{
    private readonly DiagnosisPredictionModel _diagnosisPredictionModel;

    readonly IConfiguration _configuration;

    private string CsvPath;

    private string ModelPath;
    public DiseasePredictionController(IConfiguration configuration)
    {
        _configuration = configuration;
        CsvPath = _configuration["ML:OutputFilePath"]!;
        ModelPath = _configuration["ML:DiagnosisModelPath"]!;
        _diagnosisPredictionModel = new DiagnosisPredictionModel(ModelPath);
       
    }

    [HttpPost("train")]
    public ActionResult<string> Train()
    {
        try
        {
            _diagnosisPredictionModel.Train(CsvPath);
            _diagnosisPredictionModel.SaveModel(ModelPath);
            return Ok("Trained model and saved it to disk");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("predict")]
    public ActionResult<string> PredictDiagnosis([FromBody] List<string> symptoms)
    {
        try
        {
            var model = _diagnosisPredictionModel.Predict(symptoms, CsvPath);
            return Ok(model);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("create-feature-vector")]
    public ActionResult<float[]> CreateFeatures([FromBody] List<string> symptoms)
    {
        try
        {
            var vector = _diagnosisPredictionModel.CreateFeatureVector(symptoms, CsvPath);
            return Ok(vector);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}
