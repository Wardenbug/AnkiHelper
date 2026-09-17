using AnkiHelper.Core.Generation;

namespace AnkiHelper.Core.Abstractions;

public interface IContentGenerator
{
    public Task<GenerationResult> GenerateContent(string text, string sourceLang, string targetLang, CancellationToken cancellationToken = default);
}