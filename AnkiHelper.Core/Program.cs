using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using AnkiHelper.Core.Abstractions;
using AnkiHelper.Core.Anki;
using AnkiHelper.Core.Generation;
using AnkiHelper.Core.Speech;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OllamaSharp;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddChatClient(
    new OllamaApiClient(
        new Uri("http://localhost:11434"),
        "gemma4"));

builder.Services.AddHttpClient<ISpeechSynthesizer, GoogleTextToSpeechSynthesizer>(client =>
{
    client.BaseAddress = new Uri("https://texttospeech.googleapis.com");
    client.DefaultRequestHeaders.Add("X-Goog-Api-Key", "");
});

builder.Services.AddHttpClient<IAnkiClient, AnkiConnectClient>(client =>
{
    client.BaseAddress = new Uri("http://localhost:8765");
});

builder.Services.AddScoped<IContentGenerator, ContentGenerator>();

using var app = builder.Build();

var chatClient = app.Services.GetRequiredService<IChatClient>();

var contentGenerator = app.Services.GetRequiredService<IContentGenerator>();
var textToSpeechSynthesizer = app.Services.GetRequiredService<ISpeechSynthesizer>();
var ankiClient = app.Services.GetRequiredService<IAnkiClient>();

var content = await contentGenerator.GenerateContent("hubris", "English", "Russian");

var tts = await textToSpeechSynthesizer.SynthesizeAsync(content.Items[0].Senses[0].ExampleTranslateLang, "ru-RU");


var result = await ankiClient.AddNoteAsync(new AnkiNote(
    "Test",
    "Basic (and reversed card)",
    new Dictionary<string, string>
    {
        ["Front"] = content.Items[0].Senses[0].ExampleTranslateLang,
        ["Back"] = content.Items[0].Senses[0].ExampleOriginalLang
    },
    new AnkiNoteOptions(false, "deck", new AnkiDuplicateScopeOptions("Test")),
    new AnkiAudio[] { new AnkiAudio("Test", tts, new[] { "Front" }) }
), CancellationToken.None);
