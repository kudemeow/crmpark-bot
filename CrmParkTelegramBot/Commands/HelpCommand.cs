using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;

namespace CrmParkTelegramBot.Commands
{
    internal class HelpCommand : ICommand
    {
        public string Name => "/help";

        public async Task Execute(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {
            await botClient.SendMessage(
                chatId: message.Chat.Id,
                text: "<b>Справка по командам:</b>\n\n" +
                      "<code>/inn 1234567890</code> - Найти компанию по ИНН\n" +
                      "<code>/inn 1111111111 2222222222</code> - Поиск нескольких компаний\n" +
                      "<code>/last</code> - Повторить последний результат\n" +
                      "<code>/hello</code> - Контакты разработчика\n" +
                      "<code>/help</code> - Эта справка",
                parseMode: ParseMode.Html,
                cancellationToken: cancellationToken);
        }
    }
}
