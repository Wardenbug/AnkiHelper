using AnkiHelper.Core;
using AnkiHelper.Core.Abstractions;
using AnkiHelper.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddAnkiHelperCore(builder.Configuration);

using var app = builder.Build();

var ankiHelper = app.Services.GetService<IAnkiHelper>();

var token = "";

var chatConfigs = new Dictionary<long, AnkiHelperConfiguration>();

using var cts = new CancellationTokenSource();
var bot = new TelegramBotClient(token, cancellationToken: cts.Token);
var me = await bot.GetMe();
bot.OnError += OnError;
bot.OnMessage += OnMessage;
bot.OnUpdate += OnUpdate;

Console.WriteLine($"@{me.Username} is running... Press Enter to terminate");
Console.ReadLine();
cts.Cancel();


async Task OnError(Exception exception, HandleErrorSource source)
{
    Console.WriteLine(exception);
}

async Task OnMessage(Message msg, UpdateType type)
{
    var text = msg.Text;
    if (text is null) return;
    
    if (text == "/start")
    {
        await bot.SendMessage(msg.Chat, "Welcome! Pick your language",
            replyMarkup: new InlineKeyboardMarkup( new []
            {
               InlineKeyboardButton.WithCallbackData("English", "src:en-US"), 
               InlineKeyboardButton.WithCallbackData("русский", "src:ru-RU"), 
               InlineKeyboardButton.WithCallbackData("українська", "src:uk-UA"), 
            }) 
            );
        return;
    }
    
    if (text.StartsWith("/deck "))
    {
        var deckName = text["/deck ".Length..].Trim();
        var config = chatConfigs.GetValueOrDefault(msg.Chat.Id, new AnkiHelperConfiguration("", "", ""));
        chatConfigs[msg.Chat.Id] = config with { DeckName = deckName };
        await bot.SendMessage(msg.Chat, $"Deck set to '{deckName}'. Send a word or phrase to translate.");
        return;
    }
    
    if (!chatConfigs.TryGetValue(msg.Chat.Id, out var cfg)
        || string.IsNullOrEmpty(cfg.SourceLanguage)
        || string.IsNullOrEmpty(cfg.TargetLanguage)
        || string.IsNullOrEmpty(cfg.DeckName))
    {
        await bot.SendMessage(msg.Chat, "Finish setup first: /start, pick your languages, then /deck <name>.");
        return;
    }
    
    await ankiHelper!.HandleMessageAsync(text, cfg);
    await bot.SendMessage(msg.Chat, $"Added '{text}' to deck '{cfg.DeckName}'.");
}

async Task OnUpdate(Update update)
{
    if (update is not { CallbackQuery: { Data: { } data } query }) return;

    var parts = data.Split(':', 2);
    var step = parts[0];
    var message = parts.Length > 1 ? parts[1] : "";
    
    var chatId = query.Message!.Chat.Id;
    var config = chatConfigs.GetValueOrDefault(chatId, new AnkiHelperConfiguration("", "", ""));
    
    if (step == "src")
    {
        chatConfigs[chatId] = config with { SourceLanguage = message };
        await bot.SendMessage(query.Message.Chat, "Pick your target language",
            replyMarkup: new InlineKeyboardMarkup(new[]
            {
                InlineKeyboardButton.WithCallbackData("English", "trl:en-US"),
                InlineKeyboardButton.WithCallbackData("Slovensky", "trl:sk-SK"),
                InlineKeyboardButton.WithCallbackData("German", "trl:de-DE"),
            }));
    }

    if (step == "trl")
    {
        chatConfigs[chatId] = config with { TargetLanguage = message };
        await bot.SendMessage(query.Message.Chat, "Now type /deck <name> to choose your Anki deck.");
    }

    await bot.AnswerCallbackQuery(query.Id, $"You picked {message}");
}
