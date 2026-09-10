using System.Net.Http.Json;
using System.Text.Json;
using AnkiHelper.Core.Abstractions;

namespace AnkiHelper.Core.Anki;

public sealed class AnkiConnectClient(HttpClient httpClient) : IAnkiClient
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public async Task<long> AddNoteAsync(AnkiNote note, CancellationToken cancellationToken)
    {
        var result = await httpClient.PostAsJsonAsync("", new
        {
            action = "addNote",
            version = 6,
            @params = new
            {
                note
            }
        }, cancellationToken);

        return 1;
    }
}