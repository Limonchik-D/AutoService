using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoServiceAdmin_.Services
{
    internal class RepairCostCalculator
    {
        private static readonly Dictionary<string, decimal> WorkTypeCosts = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase)
        {
            { "двигатель", 5000 },
            { "кузов", 3000 },
            { "электрик", 2000 }
        };

        public static decimal GetCost(string problemDescription)
        {
            if (string.IsNullOrWhiteSpace(problemDescription))
                return 1000; // Базовая стоимость

            foreach (var pair in WorkTypeCosts)
            {
                if (problemDescription.IndexOf(pair.Key, StringComparison.OrdinalIgnoreCase) >= 0)
                    return pair.Value;
            }

            return 1000; // Базовая стоимость, если не найдено совпадений
        }
    }
}
