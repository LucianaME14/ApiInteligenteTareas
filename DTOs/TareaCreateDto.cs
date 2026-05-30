using System.ComponentModel.DataAnnotations;
using ApiInteligenteTareas.Models;

namespace ApiInteligenteTareas.DTOs;

public class TareaCreateDto
{
    [Required(ErrorMessage = "El título es obligatorio.")]
    public string Titulo { get; set; } = string.Empty;

    public string? Descripcion { get; set; }

    [Required(ErrorMessage = "El estado es obligatorio.")]
    public EstadoTarea Estado { get; set; }

    [Required(ErrorMessage = "La prioridad es obligatoria.")]
    public PrioridadTarea Prioridad { get; set; }

    [Required(ErrorMessage = "La fecha de vencimiento es obligatoria.")]
    public DateTime FechaVencimiento { get; set; }
}
