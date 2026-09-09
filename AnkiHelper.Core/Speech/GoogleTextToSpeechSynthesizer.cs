using System.Net.Http.Json;
using AnkiHelper.Core.Abstractions;

namespace AnkiHelper.Core.Speech;

public sealed class GoogleTextToSpeechSynthesizer(HttpClient httpClient) : ISpeechSynthesizer
{
    public async Task<byte[]> SynthesizeAsync(string text, string languageCode, CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PostAsJsonAsync("v1/text:synthesize", new
        {
            input = new { text },
            voice = new { languageCode },
            audioConfig = new { audioEncoding = "MP3" }
        }, cancellationToken);

        var body = await response.Content.ReadFromJsonAsync<SynthesizeResponse>(cancellationToken);

        return Convert.FromBase64String(body.AudioContent); 
    }
    
    private sealed record SynthesizeResponse(string AudioContent);
}