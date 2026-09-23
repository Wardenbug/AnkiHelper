using AnkiHelper.Core.Anki;

namespace AnkiHelper.Core.Abstractions;

public interface IAnkiClient
{
    Task<long?> AddNoteAsync(AnkiNote note, CancellationToken ct);
    
    Task<long?> SyncAsync(CancellationToken ct = default);
}