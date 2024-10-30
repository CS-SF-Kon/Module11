using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Module11.TestTGBot.Controllers;

public class DefaultMessageController
{
    private readonly ITelegramBotClient _telegramClient;

    public DefaultMessageController(ITelegramBotClient telegramClient)
    {
        _telegramClient = telegramClient;
    }

    public async Task Handle(Message message, CancellationToken ct)
    {
        Console.WriteLine($"Controller {GetType().FullName} recieved an unsupported message from {message.Chat.Id} {message.Chat.FirstName} {message.Chat.LastName}");
        await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"Получено неподдерживаемое сообщение", cancellationToken: ct);
    }
}
