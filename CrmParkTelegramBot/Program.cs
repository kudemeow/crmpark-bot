using CrmParkTelegramBot.Commands;
using CrmParkTelegramBot.Services;
using System;
using System.Collections.Generic;
using Telegram.Bot.Polling;
using Telegram.Bot;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading;
using Telegram.Bot.Types.Enums;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Microsoft.Extensions.Logging;
using System.Text.Json;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var config = new ConfigurationBuilder()
                    .AddJsonFile("appSettings.json")
                    .AddUserSecrets<Program>()
                    .Build();

        var botClient = new TelegramBotClient(config["Telegram:BotToken"]);
        var cache = new BotResponseCache();
        var cancellationTokenSource = new CancellationTokenSource();
        var companyService = new DaDataCompanyService(config);

        var commands = new List<ICommand>
        {
            new StartCommand(),
            new HelpCommand(),
            new HelloCommand(config),
            new InnCommand(companyService, cache),
            new LastCommand(cache)
        };

        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = Array.Empty<UpdateType>() // только сообщения
        };

        async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken ct)
        {
            if (update.Message is not { } message || message.Text is null)
                return;

            var command = commands.FirstOrDefault(c =>
                message.Text.StartsWith(c.Name, StringComparison.OrdinalIgnoreCase));

            if (command != null)
            {
                await command.Execute(bot, message, ct);
            }
        }

        Task HandleErrorAsync(ITelegramBotClient bot, Exception exception, CancellationToken ct)
        {
            Console.WriteLine($"Ошибка: {exception.Message}");
            return Task.CompletedTask;
        }

        botClient.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync,
            receiverOptions,
            new CancellationTokenSource().Token
        );

        Console.WriteLine("Бот запущен. Нажмите Enter для остановки.");
        Console.ReadLine();
    }
}