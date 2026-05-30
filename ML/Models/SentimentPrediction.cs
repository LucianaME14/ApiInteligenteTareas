using Microsoft.ML.Data;

namespace ApiInteligenteTareas.ML.Models;

public class SentimentPrediction
{
    [ColumnName("PredictedLabel")]
    public string Sentimiento { get; set; } = string.Empty;
}
