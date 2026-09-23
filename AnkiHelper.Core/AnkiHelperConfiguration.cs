namespace AnkiHelper.Core;

public sealed record AnkiHelperConfiguration(
    string DeckName,
    string SourceLanguage,
    string TargetLanguage
    );