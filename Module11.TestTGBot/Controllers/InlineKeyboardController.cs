using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Module11.TestTGBot.Configuration;
using Module11.TestTGBot.Services;

namespace Module11.TestTGBot.Controllers;

public class InlineKeyboardController
{
    private readonly ITelegramBotClient _telegramClient;
    private readonly IStorage _storage;

    public InlineKeyboardController(ITelegramBotClient telegramClient, IStorage storage)
    {
        _telegramClient = telegramClient;
        _storage = storage;
    }

    public async Task Handle(CallbackQuery? callbackQuery, CancellationToken ct)
    {
        if (callbackQuery?.Data == null) return;

        _storage.GetSession(callbackQuery.From.Id).selectedItem = callbackQuery.Data;

        string selectedItem = callbackQuery.Data switch
        {
            "count" => $" подсчёта символов в сообщении.{Environment.NewLine}<b>Отправьте любое текстовое сообщение</b>",
            "summ" => $" суммирования всех чисел в сообщении.{Environment.NewLine}<b>Введите числа через пробел (целую и дробную часть разделять точкой)</b>",
            _ => String.Empty
        };

        Console.WriteLine($"Controller {GetType().FullName} recieved a {callbackQuery.Data} button pressed by {callbackQuery.From.Id} {callbackQuery.From.FirstName} {callbackQuery.From.LastName}");

        await _telegramClient.SendTextMessageAsync(callbackQuery.From.Id,
            $"Выбран режим {selectedItem}" + $"{Environment.NewLine}<i>(cменить режим можно с промощью кнопок, прилагаемых к приветственному сообщению)</i>",
            cancellationToken: ct,
            parseMode: ParseMode.Html);

        //Console.WriteLine($"Controller {GetType().FullName} recieved an button {callbackQuery.Message} press from {callbackQuery.From.Id} {callbackQuery.From.FirstName} {callbackQuery.From.LastName}");
        //await _telegramClient.SendTextMessageAsync(callbackQuery.From.Id, $"Обнаружено нажатие на кнопку", cancellationToken: ct);
    }
}
