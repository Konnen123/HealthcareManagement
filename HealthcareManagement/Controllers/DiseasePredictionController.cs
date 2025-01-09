using Application.ML;
using Application.Use_Cases.Queries.TranslateTextQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HealthcareManagement.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class DiseasePredictionController : ControllerBase
{
    private readonly DiagnosisPredictionModel _diagnosisPredictionModel;

    private readonly string _csvPath;

    private readonly string _modelPath;

    private readonly IMediator mediator;
    public DiseasePredictionController(IConfiguration configuration, IMediator mediator)
    {
        _csvPath = configuration["ML:OutputFilePath"]!;
        _modelPath = configuration["ML:DiagnosisModelPath"]!;
        _diagnosisPredictionModel = new DiagnosisPredictionModel(_modelPath);

        this.mediator = mediator;
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
    public async Task<ActionResult<DiagnosisResponse>> PredictDiagnosis([FromBody] TranslateTextQuery translateTextQuery)
    {
        try
        {
            var diagnosis = _diagnosisPredictionModel.Predict(translateTextQuery.Symptoms, _csvPath);

            translateTextQuery.Text = diagnosis.Disease;
            var translatedDiagnosis = await mediator.Send(translateTextQuery);

            if(!translatedDiagnosis.IsSuccess)
            {
                return BadRequest(translatedDiagnosis.Error);
            }

            diagnosis.Disease = translatedDiagnosis.Value!;
            return Ok(diagnosis);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}
