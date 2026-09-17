namespace AnkiHelper.Core.Generation;

public record GeneratedItem(
    string Original,
    List<Sense> Senses
    );

public record Sense(
    string Translation,
    string ExampleOriginalLang,
    string ExampleTranslateLang
    );

public record GenerationResult(
    List<GeneratedItem> Items
);