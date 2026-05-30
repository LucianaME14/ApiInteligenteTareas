using ApiInteligenteTareas.DTOs;
using ApiInteligenteTareas.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiInteligenteTareas.Controllers;

[ApiController]
[Route("api/tareas-externas")]
public class TareasExternasController : ControllerBase
{
    private readonly IExternalApiService _externalApiService;

    public TareasExternasController(IExternalApiService externalApiService)
    {
        _externalApiService = externalApiService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TareaExternaDto>>> GetTareasExternas(
        CancellationToken cancellationToken)
    {
        try
        {
            var tareas = await _externalApiService.ObtenerTareasAsync(cancellationToken);
            return Ok(tareas);
        }
        catch (ExternalApiException ex)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, new { mensaje = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TareaExternaDto>> GetTareaExterna(int id, CancellationToken cancellationToken)
    {
        try
        {
            var tarea = await _externalApiService.ObtenerTareaPorIdAsync(id, cancellationToken);
            if (tarea is null)
                return NotFound();

            return Ok(tarea);
        }
        catch (ExternalApiException ex)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, new { mensaje = ex.Message });
        }
    }
}
