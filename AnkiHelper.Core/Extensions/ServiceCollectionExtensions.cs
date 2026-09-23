using AnkiHelper.Core.Abstractions;
using AnkiHelper.Core.Anki;
using AnkiHelper.Core.Generation;
using AnkiHelper.Core.Speech;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OllamaSharp;

namespace AnkiHelper.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAnkiHelperCore(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddChatClient(
            new OllamaApiClient(
                new Uri("http://localhost:11434"),
                "gemma4"));

        services.AddHttpClient<ISpeechSynthesizer, GoogleTextToSpeechSynthesizer>(client =>
        {
            client.BaseAddress = new Uri("https://texttospeech.googleapis.com");
            client.DefaultRequestHeaders.Add("X-Goog-Api-Key", "");
        });
        
        services.AddHttpClient<IAnkiClient, AnkiConnectClient>(client =>
        {
            client.BaseAddress = new Uri("http://localhost:8765");
        });
        
        services.AddScoped<IContentGenerator, ContentGenerator>();
        services.AddScoped<IAnkiHelper, AnkiHelper>();
        
        return services;
    }
}