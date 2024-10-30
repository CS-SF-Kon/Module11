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
using Microsoft.Extensions.DependencyInjection;
using Module11.TestTGBot.Controllers;
using Module11.TestTGBot.Services;
using Module11.TestTGBot.Configuration;

namespace Module11.TestTGBot
{
    internal class Program
    {
        public static async Task Main()
        {
            Console.OutputEncoding = Encoding.UTF8;

            var host = new HostBuilder()
                .ConfigureServices((hostContext, services) => ConfigureServices(services))
                .UseConsoleLifetime()
                .Build();
            
            Console.WriteLine("Service launched");
            await host.RunAsync();
            Console.WriteLine("Service stopped");
        }

        static void ConfigureServices(IServiceCollection services)
        {
            AppSettings appSettings = BuildAppSettings();

            services.AddTransient<TextMessageController>();
            services.AddTransient<DefaultMessageController>();
            services.AddTransient<InlineKeyboardController>();

            services.AddSingleton(appSettings);
            services.AddSingleton<ITelegramBotClient>(provider => new TelegramBotClient(appSettings.Token));
            services.AddSingleton<IStorage, MemoryStorage>();

            services.AddHostedService<Bot>();
        }
        
        static AppSettings BuildAppSettings()
        {
            return new AppSettings() { Token = "" };
        }
    }

}
