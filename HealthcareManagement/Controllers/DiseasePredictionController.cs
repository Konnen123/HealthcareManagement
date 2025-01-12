using Application.ML;
using Microsoft.AspNetCore.Mvc;

namespace HealthcareManagement.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class DiseasePredictionController : ControllerBase
{
    private readonly DiagnosisPredictionModel _diagnosisPredictionModel;

    private readonly string _csvPath;

    private readonly string _modelPath;
    public DiseasePredictionController(IConfiguration configuration)
    {
        _csvPath = configuration["ML:OutputFilePath"]!;
        _modelPath = configuration["ML:DiagnosisModelPath"]!;
        _diagnosisPredictionModel = new DiagnosisPredictionModel(_modelPath);
       
    }

    [HttpPost("train")]
    public ActionResult<string> Train()
    {
        try
        {
            _diagnosisPredictionModel.Train(_csvPath);
            _diagnosisPredictionModel.SaveModel(_modelPath);
            return Ok("Trained model and saved it to disk");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("predict")]
    public ActionResult<DiagnosisResponse> PredictDiagnosis([FromBody] List<string> symptoms)
    {
        try
        {
            var diagnosis = _diagnosisPredictionModel.Predict(symptoms, _csvPath);
            return Ok(diagnosis);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}