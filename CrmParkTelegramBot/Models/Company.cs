using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrmParkTelegramBot.Models
{
    public record Company(string Inn, string Name, string Address)
    {
        public bool IsValid => !string.IsNullOrWhiteSpace(Inn)
                           && !string.IsNullOrWhiteSpace(Name);
    }
}
