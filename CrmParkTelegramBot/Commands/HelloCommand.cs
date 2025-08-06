using Microsoft.Extensions.Configuration;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;
using CrmParkTelegramBot.Models;
using CrmParkTelegramBot.Services;

namespace CrmParkTelegramBot.Commands
{
    internal class HelloCommand : ICommand
    {
        public string Name => "/hello";

        private readonly DevProfile _profile;
        public HelloCommand(IConfiguration config)
        {
            _profile = new DevProfile(

                config["DevProfile:FullName"],
                config["DevProfile:Email"],
                config["DevProfile:GitHub"],
                config["DevProfile:ResumeUrl"]
            );
        }

        public async Task Execute(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {
            await botClient.SendMessage(
                chatId: message.Chat.Id,
                text: $"Информация о разработчике:\n\n" +
                      $"Имя: {_profile.FullName}\n" +
                      $"Email: {_profile.Email}\n" +
                      $"GitHub: {_profile.GitHub}\n" +
                      $"Резюме: {_profile.ResumeUrl}",
                parseMode: ParseMode.Html,
                cancellationToken: cancellationToken);
        }
    }
}
