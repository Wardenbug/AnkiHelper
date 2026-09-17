using System.Text;
using System.Text.Json;
using AnkiHelper.Core.Abstractions;

namespace AnkiHelper.Core.Anki;

public sealed class AnkiConnectClient(HttpClient httpClient) : IAnkiClient
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public async Task<long> AddNoteAsync(AnkiNote note, CancellationToken cancellationToken)
    {
        var json = JsonSerializer.Serialize(new
        {
            action = "addNote",
            version = 6,
            @params = new
            {
                note
            }
        }, JsonOptions);
        
        using var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await httpClient.PostAsync("", content, cancellationToken);

        var raw = await response.Content.ReadAsStringAsync(cancellationToken);
        var result = JsonSerializer.Deserialize<AnkiConnectResponse>(raw, JsonOptions);

        if (result.Error is not null)
        {
            throw new InvalidOperationException(result.Error);
        }

        return result.Result;
    }

    private sealed record AnkiConnectResponse(long Result, string? Error);
}