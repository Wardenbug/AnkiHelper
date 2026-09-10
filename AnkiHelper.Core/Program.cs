using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using AnkiHelper.Core.Abstractions;
using AnkiHelper.Core.Generation;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OllamaSharp;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddChatClient(
    new OllamaApiClient(
        new Uri("http://localhost:11434"),
        "gemma4"));

builder.Services.AddScoped<IContentGenerator, ContentGenerator>();

using var app = builder.Build();

var chatClient = app.Services.GetRequiredService<IChatClient>();

var contentGenerator = app.Services.GetRequiredService<IContentGenerator>();

await contentGenerator.GenerateContent("Hello");

// await foreach (var stream in chatClient.GetStreamingResponseAsync("Что такое C#?"))
//     Console.Write(stream.Text);

// const string token = "";
// var httpClient = new HttpClient();
//
// httpClient.BaseAddress = new Uri("https://texttospeech.googleapis.com");
// httpClient.DefaultRequestHeaders.Add("X-Goog-Api-Key", token);
//
// using StringContent jsonContent1 = new(
//     JsonSerializer.Serialize(new
//     {
//         input = new
//         {
//             text = "Hello world"
//         },
//         voice = new
//         {
//             languageCode = "en-US"
//         },
//         audioConfig = new
//         {
//             audioEncoding = "MP3"
//         }
//     }),
//     Encoding.UTF8,
//     "application/json");
//
// using var result1 = await httpClient.PostAsync("v1/text:synthesize", jsonContent1);
//
// result1.EnsureSuccessStatusCode();
//
// var json = await result1.Content.ReadAsStringAsync();
// using var doc = JsonDocument.Parse(json);
// var base64Audio = doc.RootElement.GetProperty("audioContent").GetString();
//
// // var audioBytes = Convert.FromBase64String(base64Audio);
// // await File.WriteAllBytesAsync("output.mp3", audioBytes);
//
// var httpClient1 = new HttpClient();
//
// httpClient1.BaseAddress = new Uri("http://localhost:8765");
//
// using StringContent jsonContent = new(
//     JsonSerializer.Serialize(new
//     {
//         action = "addNote",
//         version = 6,
//         @params = new
//         {
//             note = new
//             {
//                 deckName = "Test",
//                 modelName = "Basic (and reversed card)",
//                 fields = new
//                 {
//                     Front = "Fronte Content",
//                     Back = "Back Content",
//                 },
//                 options = new
//                 {
//                     allowDublicate = false,
//                     duplicateScope = "deck",
//                     duplicateScopeOptions = new
//                     {
//                         deckName = "Test",
//                         checkChildren = false,
//                         checkAllModels = false
//                     }
//                 },
//                 audio = new[]
//                 {
//                     new
//                     {
//                         filename = "test.mp3",
//                         data = base64Audio,
//                         fields = new []
//                         {
//                             "Front"
//                         }
//                     }
//                 }
//             }
//         }
//     }),
//     Encoding.UTF8,
//     "application/json");
//
// // using StringContent jsonContent = new(
// //     JsonSerializer.Serialize(new
// //     {
// //         action = "modelNames",
// //         version = 6,
// //     }),
// //     Encoding.UTF8,
// //     "application/json");
//
// using var result = await httpClient1.PostAsync("", jsonContent);
//
// var json1 = await result.Content.ReadAsStringAsync();
//
// Console.WriteLine(json1);

