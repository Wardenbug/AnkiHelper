using System.Text.Encodings.Web;
using System.Text.Json;
using AnkiHelper.Core.Abstractions;
using Microsoft.Extensions.AI;
using OllamaSharp;
using OllamaSharp.Models;

namespace AnkiHelper.Core.Generation;

public class ContentGenerator(IChatClient chatClient) : IContentGenerator
{
    private static readonly ChatOptions Temperature = new ChatOptions { Temperature = 0.2f }
        .AddOllamaOption(OllamaOption.Think, true);
    private static readonly JsonSerializerOptions CamelCase = new(JsonSerializerDefaults.Web)
    {
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    public async Task<GenerationResult> GenerateContent(string text, string sourceLang, string targetLang, CancellationToken cancellationToken = default)
    {
        var promptValidationPath = Path.Combine(AppContext.BaseDirectory, "Generation", "Prompts", "Validation.md");
        var validationPrompt = new PromptTemplate(promptValidationPath)
            .Build(new() { ["sourceLang"] = sourceLang, ["targetLang"] = targetLang, ["inputText"] = text });

        var result = await chatClient.GetResponseAsync<ValidationResult>(validationPrompt, cancellationToken: cancellationToken);

        var promptGenerationPath = Path.Combine(AppContext.BaseDirectory, "Generation", "Prompts", "Generation.md");

        var generationPrompt = new PromptTemplate(promptGenerationPath)
            .Build(new() { ["sourceLang"] = sourceLang, ["targetLang"] = targetLang, ["inputItemsJson"] = JsonSerializer.Serialize(result.Result.Items, CamelCase) });

        var genResult = await chatClient.GetResponseAsync<GenerationResult>(generationPrompt, Temperature, cancellationToken: cancellationToken);

        Console.WriteLine(genResult.Text);

        return genResult.Result;
    }
}