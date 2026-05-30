using ApiInteligenteTareas.DTOs;

namespace ApiInteligenteTareas.Services;

public interface IExternalApiService
{
    Task<IReadOnlyList<TareaExternaDto>> ObtenerTareasAsync(CancellationToken cancellationToken = default);
    Task<TareaExternaDto?> ObtenerTareaPorIdAsync(int id, CancellationToken cancellationToken = default);
}
