using AnkiHelper.Core.Abstractions;

namespace AnkiHelper.Core.Anki;

public sealed class AnkiConnectClient(HttpClient httpClient) : IAnkiClient
{
    public Task<long> AddNoteAsync(AnkiNote note, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}