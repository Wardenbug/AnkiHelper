namespace AnkiHelper.Core.Abstractions;

public interface IContentGenerator
{
    public Task GenerateContent(string text, CancellationToken cancellationToken = default);
}