using CrmParkTelegramBot.Services;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;

namespace CrmParkTelegramBot.Commands
{
    internal class LastCommand : ICommand
    {
        public string Name => "/last";

        private readonly BotResponseCache _cache;

        public LastCommand(BotResponseCache cache)
        {
            _cache = cache;
        }

        public async Task Execute(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {
            var lastResponse = _cache.GetLastResponse(message.Chat.Id);

            await botClient.SendMessage(
                chatId: message.Chat.Id,
                text: lastResponse ?? "Нет данных для повтора",
                parseMode: ParseMode.Html,
                cancellationToken: cancellationToken);
        }
    }
}
