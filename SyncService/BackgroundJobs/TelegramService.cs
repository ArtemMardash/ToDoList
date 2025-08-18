using System.Diagnostics;
using SyncService.Core.Entities;
using SyncService.Features.Shared.Interfaces;
using SyncService.Features.Shared.Repositories;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace SyncService.BackgroundJobs;

public class TelegramService : BackgroundService
{
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly IServiceProvider _serviceProvider;
    private ITgLinksRepository _tgLinksRepository;
    private IUnitOfWork _unitOfWork;
    private INotificationRepository _notificationRepository;

    public TelegramService(IServiceProvider serviceProvider, IConfiguration configuration)
    {
        _serviceProvider = serviceProvider;
        _telegramBotClient = new TelegramBotClient(configuration["Telegram:Token"]);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();
        _tgLinksRepository = scope.ServiceProvider.GetRequiredService<ITgLinksRepository>();
        _unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        _notificationRepository = scope.ServiceProvider.GetRequiredService<INotificationRepository>();
        _telegramBotClient.StartReceiving(HandleUpdateAsync, HandleErrorAsync, cancellationToken: stoppingToken);
        Console.WriteLine("Bot is started");
        await SendNotificationsAsync(_telegramBotClient, stoppingToken);
        await Task.Delay(-1, stoppingToken);
    }

    private async Task HandleErrorAsync(ITelegramBotClient bot, Exception ex, HandleErrorSource handleErrorSource,
        CancellationToken ct)
    {
        var message = ex switch
        {
            ApiRequestException apiException =>
                $"Telegram API error:\n [{apiException.ErrorCode}]\n{apiException.Message}",
            _ => ex.ToString()
        };
        await Console.Error.WriteLineAsync(message);
    }

    private async Task HandleCodeAsync(int uniqueCode, long chatId, CancellationToken ct)
    {
        try
        {
            var tgLink = await _tgLinksRepository.GetTgLinkByUniqueCodeAsync(uniqueCode, ct);
            tgLink.TgUserId = chatId;
            await _tgLinksRepository.UpdateAsync(tgLink, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }
        catch (InvalidOperationException exception)
        {
            Console.WriteLine("There is no tgLink with such unique code");
        }
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
                    var a when a.StartsWith("/code") => HandleCodeAsync(int.Parse(a.Substring(5).Trim()),
                        message.Chat.Id, ct),
                    _ => bot.SendMessage(message.Chat.Id, "I don't know command like this", cancellationToken: ct)
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
        return bot.SendMessage(message.Chat.Id,
            $"The message start was handled {message.Chat.Id} {message.Chat.Username}");
    }

    private async Task SendNotificationsAsync(ITelegramBotClient bot,CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            using var scope = _serviceProvider.CreateScope();
            var tgLinksRepository = scope.ServiceProvider.GetRequiredService<ITgLinksRepository>();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var notificationRepository = scope.ServiceProvider.GetRequiredService<INotificationRepository>();
            var undeliveredNotifications = await notificationRepository.GetNotDeliveredNotificationsAsync(cancellationToken);
            foreach (var n in undeliveredNotifications)
            {
                var tgLink = await tgLinksRepository.GetTgLinkByUserIdAsync(n.UserId, cancellationToken); //добавить tgUserId  to notifications
                await bot.SendMessage(tgLink.TgUserId,
                    $"New notification: Task with Id {n.TaskId} with type {n.NotificationType}", cancellationToken: cancellationToken);
                await notificationRepository.DeliverNotificationAsync(n, cancellationToken);
                await unitOfWork.SaveChangesAsync(cancellationToken);
            }
        }
    }
}