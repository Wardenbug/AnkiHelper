using System.Text.Json;
using AnkiHelper.Core.Abstractions;
using Microsoft.Extensions.AI;

namespace AnkiHelper.Core.Generation;

public class ContentGenerator(IChatClient chatClient) : IContentGenerator
{
    // private async Task<string> ExtractInformationFromText(string text)
    // {
    //     
    // }

    public async Task GenerateContent(string text, CancellationToken cancellationToken = default)
    {
        var promptValidationPath = Path.Combine(AppContext.BaseDirectory, "Generation", "Prompts", "Validation.md");
        var validationPrompt = new PromptTemplate(promptValidationPath)
            .Build(new() { ["sourceLang"] = "English", ["targetLang"] = "German", ["inputText"] = text});

        var result = await chatClient.GetResponseAsync<ValidationResult>(validationPrompt, cancellationToken: cancellationToken);
        
        var promptGenerationPath = Path.Combine(AppContext.BaseDirectory, "Generation", "Prompts", "Generation.md");
        
        var generationPrompt = new PromptTemplate(promptGenerationPath)
            .Build(new() { ["sourceLang"] = "English", ["targetLang"] = "German", ["inputItemsJson"] = JsonSerializer.Serialize(result.Result.Items)});
        
        var genResult = await chatClient.GetResponseAsync<GenerationResult>(generationPrompt, cancellationToken: cancellationToken);

        Console.WriteLine(JsonSerializer.Serialize(genResult.Result));
    }
}