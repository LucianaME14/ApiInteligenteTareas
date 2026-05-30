using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using ApiInteligenteTareas.DTOs;

namespace ApiInteligenteTareas.Services;

public class ExternalApiService : IExternalApiService
{
    private readonly HttpClient _httpClient;

    public ExternalApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyList<TareaExternaDto>> ObtenerTareasAsync(CancellationToken cancellationToken = default)
    {
        HttpResponseMessage response;
        try
        {
            response = await _httpClient.GetAsync("todos", cancellationToken);
        }
        catch (HttpRequestException)
        {
            throw new ExternalApiException("No se pudo conectar con la API externa.");
        }
        catch (TaskCanceledException)
        {
            throw new ExternalApiException("La API externa no respondió a tiempo.");
        }

        await EnsureSuccessAsync(response);

        var todos = await response.Content.ReadFromJsonAsync<List<JsonPlaceholderTodo>>(cancellationToken)
            ?? [];

        return todos.Select(MapToDto).ToList();
    }

    public async Task<TareaExternaDto?> ObtenerTareaPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        HttpResponseMessage response;
        try
        {
            response = await _httpClient.GetAsync($"todos/{id}", cancellationToken);
        }
        catch (HttpRequestException)
        {
            throw new ExternalApiException("No se pudo conectar con la API externa.");
        }
        catch (TaskCanceledException)
        {
            throw new ExternalApiException("La API externa no respondió a tiempo.");
        }

        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;

        await EnsureSuccessAsync(response);

        var todo = await response.Content.ReadFromJsonAsync<JsonPlaceholderTodo>(cancellationToken);
        return todo is null ? null : MapToDto(todo);
    }

    private static async Task EnsureSuccessAsync(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
            return;

        throw new ExternalApiException("La API externa devolvió un error.");
    }

    private static TareaExternaDto MapToDto(JsonPlaceholderTodo todo) => new()
    {
        ExternalId = todo.Id,
        Titulo = todo.Title,
        Completado = todo.Completed
    };

    private sealed class JsonPlaceholderTodo
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("completed")]
        public bool Completed { get; set; }
    }
}

public class ExternalApiException : Exception
{
    public ExternalApiException(string message) : base(message)
    {
    }
}
