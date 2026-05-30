using ApiInteligenteTareas.Data;
using ApiInteligenteTareas.DTOs;
using ApiInteligenteTareas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiInteligenteTareas.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TareasController : ControllerBase
{
    private readonly AppDbContext _context;

    public TareasController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TareaResponseDto>>> GetTareas([FromQuery] TareaFiltroDto filtro)
    {
        var errorFiltro = ValidarFiltros(filtro);
        if (errorFiltro is not null)
            return BadRequest(new { mensaje = errorFiltro });

        var query = _context.Tareas.AsQueryable();

        if (Enum.TryParse<EstadoTarea>(filtro.Estado, ignoreCase: true, out var estado))
            query = query.Where(t => t.Estado == estado);

        if (Enum.TryParse<PrioridadTarea>(filtro.Prioridad, ignoreCase: true, out var prioridad))
            query = query.Where(t => t.Prioridad == prioridad);

        if (filtro.FechaInicio.HasValue)
            query = query.Where(t => t.FechaVencimiento >= filtro.FechaInicio.Value.Date);

        if (filtro.FechaFin.HasValue)
            query = query.Where(t => t.FechaVencimiento <= filtro.FechaFin.Value.Date);

        var tareas = await query
            .OrderByDescending(t => t.FechaCreacion)
            .ToListAsync();

        return Ok(tareas.Select(MapToResponse));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TareaResponseDto>> GetTarea(int id)
    {
        var tarea = await _context.Tareas.FindAsync(id);
        if (tarea is null)
            return NotFound();

        return Ok(MapToResponse(tarea));
    }

    [HttpPost]
    public async Task<ActionResult<TareaResponseDto>> CreateTarea(TareaCreateDto dto)
    {
        var validationError = ValidateDto(dto.Titulo, dto.FechaVencimiento);
        if (validationError is not null)
            return BadRequest(new { mensaje = validationError });

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var tarea = new Tarea
        {
            Titulo = dto.Titulo.Trim(),
            Descripcion = dto.Descripcion?.Trim(),
            Estado = dto.Estado,
            Prioridad = dto.Prioridad,
            FechaCreacion = DateTime.Now,
            FechaVencimiento = dto.FechaVencimiento.Date
        };

        _context.Tareas.Add(tarea);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTarea), new { id = tarea.Id }, MapToResponse(tarea));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTarea(int id, TareaUpdateDto dto)
    {
        var validationError = ValidateDto(dto.Titulo, dto.FechaVencimiento);
        if (validationError is not null)
            return BadRequest(new { mensaje = validationError });

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var tarea = await _context.Tareas.FindAsync(id);
        if (tarea is null)
            return NotFound();

        tarea.Titulo = dto.Titulo.Trim();
        tarea.Descripcion = dto.Descripcion?.Trim();
        tarea.Estado = dto.Estado;
        tarea.Prioridad = dto.Prioridad;
        tarea.FechaVencimiento = dto.FechaVencimiento.Date;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTarea(int id)
    {
        var tarea = await _context.Tareas.FindAsync(id);
        if (tarea is null)
            return NotFound();

        _context.Tareas.Remove(tarea);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    private static string? ValidarFiltros(TareaFiltroDto filtro)
    {
        if (!string.IsNullOrWhiteSpace(filtro.Estado) &&
            !Enum.TryParse<EstadoTarea>(filtro.Estado, ignoreCase: true, out _))
            return "El estado no es válido. Valores permitidos: Pendiente, EnProceso, Completada.";

        if (!string.IsNullOrWhiteSpace(filtro.Prioridad) &&
            !Enum.TryParse<PrioridadTarea>(filtro.Prioridad, ignoreCase: true, out _))
            return "La prioridad no es válida. Valores permitidos: Baja, Media, Alta.";

        if (filtro.FechaInicio.HasValue && filtro.FechaFin.HasValue &&
            filtro.FechaInicio.Value.Date > filtro.FechaFin.Value.Date)
            return "La fecha de inicio no puede ser mayor que la fecha de fin.";

        return null;
    }

    private static string? ValidateDto(string titulo, DateTime fechaVencimiento)
    {
        if (string.IsNullOrWhiteSpace(titulo))
            return "El título es obligatorio.";

        if (fechaVencimiento.Date < DateTime.Today)
            return "La fecha de vencimiento no puede ser menor a la fecha actual.";

        return null;
    }

    private static TareaResponseDto MapToResponse(Tarea tarea) => new()
    {
        Id = tarea.Id,
        Titulo = tarea.Titulo,
        Descripcion = tarea.Descripcion,
        Estado = tarea.Estado,
        Prioridad = tarea.Prioridad,
        FechaCreacion = tarea.FechaCreacion,
        FechaVencimiento = tarea.FechaVencimiento
    };
}
