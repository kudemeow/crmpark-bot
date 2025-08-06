using CrmParkTelegramBot.Services;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using CrmParkTelegramBot.Models;
using System.Collections.Generic;
using System.Threading;
using System;
using Telegram.Bot.Types.Enums;

namespace CrmParkTelegramBot.Commands
{
    internal class InnCommand : ICommand
    {
        public string Name => "/inn";

        private readonly ICompanyService _companyService;
        private readonly BotResponseCache _cache;

        public InnCommand(ICompanyService companyService, BotResponseCache cache)
        {
            _companyService = companyService;
            _cache = cache;
        }

        public async Task Execute(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {
            try
            {
                var inns = ParseInns(message.Text);

                if (!inns.Any())
                {
                    await botClient.SendMessage(
                        chatId: message.Chat.Id,
                        text: "Используйте: <code>/inn 1234567890 9876543210</code>",
                        parseMode: ParseMode.Html,
                        cancellationToken: cancellationToken);
                    return;
                }

                var companies = await _companyService.GetCompaniesByInnAsync(inns);
                var response = BuildResponse(companies);

                _cache.SetLastResponse(message.Chat.Id, response);
                await botClient.SendMessage(
                    chatId: message.Chat.Id,
                    text: response,
                    parseMode: ParseMode.Html,
                    cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                await SendError(botClient, message.Chat.Id, ex, cancellationToken);
            }
        }

        private List<string> ParseInns(string input)
        {
            return input?
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Skip(1)
                .Where(inn => inn.Length is 10 or 12 && inn.All(char.IsDigit))
                .ToList() ?? new List<string>();
        }

        private string BuildResponse(List<Company> companies)
        {
            if (!companies.Any()) return "Компании не найдены";

            return string.Join("\n\n", companies.Select(c =>
                $"<b>{Escape(c.Name)}</b>\n" +
                $"Адрес: {Escape(c.Address)}\n" +
                $"ИНН: {c.Inn}"));
        }

        private async Task SendError(ITelegramBotClient bot, long chatId, Exception ex, CancellationToken ct)
        {
            await bot.SendMessage(
                chatId: chatId,
                text: $"Ошибка: {ex.Message}",
                cancellationToken: ct);
        }

        private string Escape(string text) =>
            System.Net.WebUtility.HtmlEncode(text);
    }
}
