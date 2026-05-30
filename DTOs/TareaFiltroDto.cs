namespace ApiInteligenteTareas.DTOs;

public class TareaFiltroDto
{
    public string? Estado { get; set; }
    public string? Prioridad { get; set; }
    public DateTime? FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
}
