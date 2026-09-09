namespace AnkiHelper.Core.Anki;

public record AnkiNote(string DeckName, string ModelName, IReadOnlyDictionary<string, string> Fields, IReadOnlyList<AnkiAudio> Audios);

public record AnkiAudio(string Filename, byte[] Data, IReadOnlyList<string> Fields);