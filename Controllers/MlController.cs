using ApiInteligenteTareas.DTOs;
using ApiInteligenteTareas.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiInteligenteTareas.Controllers;

[ApiController]
[Route("api/ml")]
public class MlController : ControllerBase
{
    private readonly ISentimentAnalysisService _sentimentService;

    public MlController(ISentimentAnalysisService sentimentService)
    {
        _sentimentService = sentimentService;
    }

    [HttpPost("sentimiento")]
    public ActionResult<SentimientoResponseDto> AnalizarSentimiento([FromBody] SentimientoRequestDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (string.IsNullOrWhiteSpace(request.Comentario))
            return BadRequest(new { mensaje = "El comentario es obligatorio." });

        var sentimiento = _sentimentService.AnalizarSentimiento(request.Comentario.Trim());

        return Ok(new SentimientoResponseDto
        {
            Comentario = request.Comentario.Trim(),
            Sentimiento = sentimiento
        });
    }
}
