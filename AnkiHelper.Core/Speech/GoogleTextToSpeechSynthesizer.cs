using System.Net.Http.Json;
using System.Text.RegularExpressions;
using System.Linq;
using AnkiHelper.Core.Abstractions;

namespace AnkiHelper.Core.Speech;

public sealed class GoogleTextToSpeechSynthesizer(HttpClient httpClient) : ISpeechSynthesizer
{
    public async Task<byte[]> SynthesizeAsync(string text, string languageCode, CancellationToken cancellationToken = default)
    {
        var ssml = $"<speak>{ToEmphasisSsml(text)}</speak>";

        var response = await httpClient.PostAsJsonAsync("v1/text:synthesize", new
        {
            input = new { ssml },
            voice = new { languageCode },
            audioConfig = new { audioEncoding = "MP3" }
        }, cancellationToken);

        var body = await response.Content.ReadFromJsonAsync<SynthesizeResponse>(cancellationToken);

        return Convert.FromBase64String(body.AudioContent);
    }

    private static string ToEmphasisSsml(string text) =>
        string.Concat(Regex.Split(text, "(</?b>)").Select(part => part switch
        {
            "<b>" => "<emphasis level=\"strong\">",
            "</b>" => "</emphasis>",
            _ => System.Security.SecurityElement.Escape(part)
        }));

    private sealed record SynthesizeResponse(string AudioContent);
}