namespace AnkiHelper.Core.Anki;

public sealed record AnkiNote(string DeckName, string ModelName, IReadOnlyDictionary<string, string> Fields, AnkiNoteOptions Options, IReadOnlyList<AnkiAudio> Audio);

public sealed record AnkiAudio(string Filename, byte[] Data, IReadOnlyList<string> Fields);

public sealed record AnkiNoteOptions(
    bool AllowDuplicate = false,
    string? DuplicateScope = null,
    AnkiDuplicateScopeOptions? DuplicateScopeOptions = null);

public sealed record AnkiDuplicateScopeOptions(string? DeckName = null, bool CheckChildren = false, bool CheckAllModels = false);