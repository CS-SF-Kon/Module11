using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Microsoft.Extensions.Hosting;
using Module11.TestTGBot.Controllers;

namespace Module11.TestTGBot;

internal class Bot : BackgroundService
{
    private ITelegramBotClient _telegramClient;

    private InlineKeyboardController _inlineKeyboardController;
    private TextMessageController _textMessageController;
    private DefaultMessageController _defaultMessageController;

    public Bot(
        ITelegramBotClient telegramClient,
        InlineKeyboardController inlineKeyboardController,
        TextMessageController textMessageController,
        DefaultMessageController defaultMessageController)
    {
        _telegramClient = telegramClient;
        _inlineKeyboardController = inlineKeyboardController;
        _textMessageController = textMessageController;
        _defaultMessageController = defaultMessageController;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _telegramClient.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync,
            new ReceiverOptions() { AllowedUpdates = { } },
            cancellationToken: stoppingToken);
        Console.WriteLine("Bot launched");
    }

    async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Type == UpdateType.CallbackQuery)
        {
            await _inlineKeyboardController.Handle(update.CallbackQuery, cancellationToken);
            return;
        }

        if (update.Type == UpdateType.Message)
        {
            switch (update.Message!.Type)
            {
                case MessageType.Text:
                    await _textMessageController.Handle(update.Message, cancellationToken);
                    return;
                default:
                    await _defaultMessageController.Handle(update.Message, cancellationToken);
                    return;
            }
        }
    }

    Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException => $"TG API error: {apiRequestException.ErrorCode} - {apiRequestException.Message}",
            _ => exception.ToString()
        };
        Console.WriteLine($"Error: {errorMessage}");
        Console.WriteLine("10 seconds delay before reconnecting");
        Thread.Sleep(10000);
        return Task.CompletedTask;
    }
}
