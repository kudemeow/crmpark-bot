using System.Collections.Generic;

namespace CrmParkTelegramBot.Services
{
    internal class BotResponseCache
    {
        private readonly Dictionary<long, string> _cache = new();

        public void SetLastResponse(long chatId, string response)
            => _cache[chatId] = response;

        public string? GetLastResponse(long chatId)
            => _cache.TryGetValue(chatId, out var value) ? value : null;
    }
}
