using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;
using CrmParkTelegramBot.Services;

namespace CrmParkTelegramBot.Commands
{
    internal class StartCommand : ICommand
    {
        public string Name => "/start";

        public async Task Execute(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {
            await botClient.SendMessage(
                chatId: message.Chat.Id,
                text: "Добро пожаловать в CRMpark Bot!\n" +
                      "Я помогу найти информацию о компаниях по ИНН.\n\n" +
                      "Доступные команды:\n" +
                      "/start - Начать работу\n" +
                      "/help - Справка по командам\n" +
                      "/hello - Информация о разработчике\n" +
                      "/inn [ИНН] - Поиск компаний\n" +
                      "/last - Повторить последний результат",
                parseMode: ParseMode.Html,
                cancellationToken: cancellationToken);
        }
    }
}
