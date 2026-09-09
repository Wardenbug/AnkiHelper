namespace AnkiHelper.Core.Abstractions;

public interface ISpeechSynthesizer
{
    public Task<byte[]> SynthesizeAsync(string text, string languageCode, CancellationToken cancellationToken = default);
}