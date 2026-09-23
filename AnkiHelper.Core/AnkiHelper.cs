using AnkiHelper.Core.Abstractions;
using AnkiHelper.Core.Anki;

namespace AnkiHelper.Core;

public sealed class AnkiHelper(
    IContentGenerator contentGenerator, 
    ISpeechSynthesizer speechSynthesizer, 
    IAnkiClient ankiClient) : IAnkiHelper
{
    public async Task<bool> HandleMessageAsync(
        string message, 
        AnkiHelperConfiguration config,
        CancellationToken cancellationToken = default)
    {
        var content = await contentGenerator.GenerateContent(message,
            config.SourceLanguage,
            config.TargetLanguage,
            cancellationToken);

        foreach (var item in content.Items)
        {
            foreach (var sense in item.Senses)
            {
                var tts = await speechSynthesizer.SynthesizeAsync(sense.ExampleTranslateLang, config.TargetLanguage, cancellationToken);
                
                var result = await ankiClient.AddNoteAsync(new AnkiNote(
                    config.DeckName,
                    AnkiModelNames.BasicAndReversed,
                    new Dictionary<string, string>
                    {
                        ["Front"] = sense.ExampleTranslateLang,
                        ["Back"] =  sense.ExampleOriginalLang,
                    },
                    new AnkiNoteOptions(false, "deck", new AnkiDuplicateScopeOptions(config.DeckName)),
                    new AnkiAudio[] { new AnkiAudio($"{Guid.CreateVersion7()}.mp3", tts, new[] { "Front" }) }
                ), cancellationToken);
            }
        }

        await ankiClient.SyncAsync(cancellationToken);
        return true;
    }
}