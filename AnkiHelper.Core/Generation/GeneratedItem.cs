namespace AnkiHelper.Core.Generation;

public record GeneratedItem(
    string Original,
    string Translation,
    string ExampleOriginalLang,
    string ExampleTranslateLang
    );
    
public record GenerationResult(
    List<GeneratedItem> Items
);