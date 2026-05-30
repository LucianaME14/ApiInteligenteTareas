using ApiInteligenteTareas.ML.Models;
using ApiInteligenteTareas.ML.Trainers;
using Microsoft.ML;

namespace ApiInteligenteTareas.Services;

public class SentimentAnalysisService : ISentimentAnalysisService
{
    private readonly PredictionEngine<SentimentData, SentimentPrediction> _predictionEngine;

    public SentimentAnalysisService(IWebHostEnvironment environment)
    {
        var dataPath = Path.Combine(environment.ContentRootPath, "ML", "Data", "sentimiento-data.csv");
        var modelPath = Path.Combine(environment.ContentRootPath, "ML", "ModelsGenerated", "sentiment-model.zip");

        if (!File.Exists(dataPath))
            throw new FileNotFoundException($"No se encontró el dataset: {dataPath}");

        if (!File.Exists(modelPath))
            SentimentTrainer.EntrenarYGuardar(dataPath, modelPath);

        var mlContext = new MLContext();
        using var stream = File.OpenRead(modelPath);
        var model = mlContext.Model.Load(stream, out _);
        _predictionEngine = mlContext.Model.CreatePredictionEngine<SentimentData, SentimentPrediction>(model);
    }

    public string AnalizarSentimiento(string comentario)
    {
        var prediction = _predictionEngine.Predict(new SentimentData { Texto = comentario });
        return prediction.Sentimiento;
    }
}
