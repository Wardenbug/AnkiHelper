namespace AnkiHelper.Core.Generation;

public record ValidatedItem(
    string Original,
    string Type,
    string DetectedLanguage,
    string Status,
    string? Issue);