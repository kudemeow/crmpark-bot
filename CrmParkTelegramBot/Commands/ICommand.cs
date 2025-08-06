using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using System.Threading;

namespace CrmParkTelegramBot.Commands
{
    public interface ICommand
    {
        string Name { get; }
        Task Execute(ITelegramBotClient bot, Message message, CancellationToken cancellationToken);
    }
}
