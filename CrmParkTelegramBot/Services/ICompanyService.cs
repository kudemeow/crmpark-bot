using CrmParkTelegramBot.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CrmParkTelegramBot.Services
{
    public interface ICompanyService
    {
        Task<List<Company>> GetCompaniesByInnAsync(List<string> inns);
    }
}
