using CrmParkTelegramBot.Models;
using Dadata;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace CrmParkTelegramBot.Services
{
    internal class DaDataCompanyService : ICompanyService
    {
        private readonly SuggestClientAsync _apiClient;

        public DaDataCompanyService(IConfiguration config)
        {
            var apiKey = config["DaData:ApiKey"];
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new ArgumentException("API-ключ DaData не указан в конфигурации");
            }

            _apiClient = new SuggestClientAsync(apiKey);
        }

        public async Task<List<Company>> GetCompaniesByInnAsync(List<string> inns)
        {
            var results = new List<Company>();

            foreach (var inn in inns.Distinct())
            {
                try
                {
                    var response = await _apiClient.FindParty(inn);

                    if (response.suggestions != null && response.suggestions.Count > 0)
                    {
                        var suggestion = response.suggestions[0];
                        var data = suggestion.data;

                        results.Add(new Company(
                            data.inn,
                            data.name.full_with_opf,
                            data.address.value
                        ));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при обработке ИНН {inn}: {ex.Message}");
                }
            }

            return results.OrderBy(c => c.Name).ToList();
        }
    }
}
