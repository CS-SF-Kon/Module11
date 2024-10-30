using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.Enums;
using Module11.TestTGBot.Services;
using System.Collections.Specialized;

namespace Module11.TestTGBot.Controllers;

public class TextMessageController
{
    private readonly ITelegramBotClient _telegramClient;
    private readonly IStorage _storage;


    public TextMessageController(ITelegramBotClient telegramClient, IStorage storage)
    {
        _telegramClient = telegramClient;
        _storage = storage;
    }

    public async Task Handle(Message message, CancellationToken ct)
    {
        switch (message.Text)
        {
            case "/start":
                var buttons = new List<InlineKeyboardButton[]>();
                buttons.Add(new[]
                {
                    InlineKeyboardButton.WithCallbackData("Подсчёт количества символов", "count"),
                    InlineKeyboardButton.WithCallbackData("Сумма введённых чисел", "summ")
                });

                Console.WriteLine($"Controller {GetType().FullName} recieved a \"start\" command from {message.Chat.Id} {message.Chat.FirstName} {message.Chat.LastName}");

                await _telegramClient.SendTextMessageAsync(message.Chat.Id,
                    $"Этот бот может либо выводить число символов в сообщении, либо суммировать все введённые числа{Environment.NewLine}" + $"{Environment.NewLine}<i>Выберите опцию</i>",
                    cancellationToken: ct,
                    parseMode: ParseMode.Html,
                    replyMarkup: new InlineKeyboardMarkup(buttons));
                break;
            
            default:
                string mode = _storage.GetSession(message.Chat.Id).selectedItem;
                switch (mode)
                {
                    case "count":
                        Console.WriteLine($"User {message.Chat.Id} {message.Chat.FirstName} {message.Chat.LastName} sends a text message \"{message.Text}\" with an symbols count task");
                        
                        await _telegramClient.SendTextMessageAsync(message.Chat.Id,
                            $"Число символов в Вашем сообщении \"{message.Text}\" составляет {message.Text.Length}",
                            cancellationToken: ct,
                            parseMode: ParseMode.Html);
                        break;
                    
                    case "summ":
                        Console.WriteLine($"User {message.Chat.Id} {message.Chat.FirstName} {message.Chat.LastName} sends a text message \"{message.Text}\" with an numbers sum task");

                        string[] numbers = message.Text.Split(' ');
                        double sum = 0;
                        string unrecog = "";
                        string addMSG = "";
                        bool flag = false;
                        foreach (var num in numbers)
                        {
                            if (double.TryParse(num, out double number))
                            {
                                sum += number;
                            }
                            else
                            {
                                flag = true;
                                unrecog += num + ", ";
                            }
                        }

                        if (flag)
                        {
                            addMSG = "Были встречены нераспознанные символы: \"" + unrecog + "\"";
                        }
                        else
                        {
                            addMSG = "Нераспознанных символов нет";
                        }

                        await _telegramClient.SendTextMessageAsync(message.Chat.Id,
                            $"Сумма введённых Вами чисел \"{message.Text}\" составляет {sum}" + $"{Environment.NewLine}{addMSG}",
                            cancellationToken: ct,
                            parseMode: ParseMode.Html);
                        break;
                    
                    default:

                        break;
                }
                break;
        }
        
        //Console.WriteLine($"Controller {GetType().FullName} recieved a message \"{message.Text}\" from {message.Chat.Id} {message.Chat.FirstName} {message.Chat.LastName}");
        //await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"Получено текстовое сообщение {message.Text}", cancellationToken: ct);
    }
}
