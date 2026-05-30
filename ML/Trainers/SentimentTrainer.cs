using ApiInteligenteTareas.ML.Models;
using Microsoft.ML;

namespace ApiInteligenteTareas.ML.Trainers;

public static class SentimentTrainer
{
    public static void EntrenarYGuardar(string dataPath, string modelPath)
    {
        var mlContext = new MLContext(seed: 0);

        var data = mlContext.Data.LoadFromTextFile<SentimentData>(
            dataPath,
            hasHeader: true,
            separatorChar: ',');

        var pipeline = mlContext.Transforms.Conversion
            .MapValueToKey(outputColumnName: "Label", inputColumnName: nameof(SentimentData.Sentimiento))
            .Append(mlContext.Transforms.Text.FeaturizeText(
                outputColumnName: "Features",
                inputColumnName: nameof(SentimentData.Texto)))
            .Append(mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy(
                labelColumnName: "Label",
                featureColumnName: "Features"))
            .Append(mlContext.Transforms.Conversion.MapKeyToValue(
                outputColumnName: "PredictedLabel",
                inputColumnName: "PredictedLabel"));

        var model = pipeline.Fit(data);

        var modelDir = Path.GetDirectoryName(modelPath);
        if (!string.IsNullOrEmpty(modelDir))
            Directory.CreateDirectory(modelDir);

        mlContext.Model.Save(model, data.Schema, modelPath);
    }
}
