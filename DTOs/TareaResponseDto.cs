using ApiInteligenteTareas.Models;

namespace ApiInteligenteTareas.DTOs;

public class TareaResponseDto
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public EstadoTarea Estado { get; set; }
    public PrioridadTarea Prioridad { get; set; }
    public DateTime FechaCreacion { get; set; }
    public DateTime FechaVencimiento { get; set; }
}
