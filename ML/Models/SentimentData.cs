using Microsoft.ML.Data;

namespace ApiInteligenteTareas.ML.Models;

public class SentimentData
{
    [LoadColumn(0)]
    public string Texto { get; set; } = string.Empty;

    [LoadColumn(1)]
    public string Sentimiento { get; set; } = string.Empty;
}
