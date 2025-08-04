using System.Diagnostics;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace SyncService.BackgroundJobs;

public class TelegramService: BackgroundService
{
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly IServiceProvider _serviceProvider;

    public TelegramService(IServiceProvider serviceProvider, IConfiguration configuration)
    {
        _serviceProvider = serviceProvider;
        _telegramBotClient = new TelegramBotClient(configuration["Telegram:Token"]);
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
         _telegramBotClient.StartReceiving(HandleUpdateAsync, HandleErrorAsync, cancellationToken: stoppingToken);
         Console.WriteLine("Bot is started");
         await Task.Delay(-1, stoppingToken);
    }

    private async Task HandleErrorAsync(ITelegramBotClient bot, Exception ex, HandleErrorSource handleErrorSource, CancellationToken ct)
    {
        var message = ex switch
        {
            ApiRequestException apiException =>
                $"Telegram API error:\n [{apiException.ErrorCode}]\n{apiException.Message}",
            _ => ex.ToString()
        };
        await Console.Error.WriteLineAsync(message);
    }

    private async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken ct)
    {
        try
        {
            if (update.Message is { } message)
            {
                var handler = message.Text switch
                {
                    "/start" => OnStartCommandAsync(bot, message, ct),
                    //var a when a.StartsWith("/code")=>HandleCodeAsync(a, ct),
                    _=>bot.SendMessage(message.Chat.Id, "I don't know command like this", cancellationToken: ct)
                };
                await handler;
            }
        }
        catch (Exception ex)
        {
            await Console.Error.WriteLineAsync($"The error during handling update\n {ex}");
        }
    }

    private Task OnStartCommandAsync(ITelegramBotClient bot, Message message, CancellationToken cancellationToken)
    {
        return bot.SendMessage(message.Chat.Id, $"The message start was handled {message.Chat.Id} {message.Chat.Username}");
    }
}