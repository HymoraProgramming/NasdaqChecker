using NasdaqChecker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NasdaqChecker.Presentation
{
    public static class TablePrinter
    {
        public static void PrintCompanyTable(List<NasdaqCompany> companies)
        {
            int nameMaxLength = 30;
            int lpWidth = 4;
            int symbolWidth = 6;
            int nameWidth = nameMaxLength;
            int capWidth = 20;
            int weightWidth = 11;

            string horizontal = "+" +
                new string('-', lpWidth + 2) + "+" +
                new string('-', symbolWidth + 2) + "+" +
                new string('-', nameWidth + 2) + "+" +
                new string('-', capWidth + 2) + "+" +
                new string('-', weightWidth + 2) + "+";

            Console.WriteLine(horizontal);
            Console.WriteLine($"| {"No..".PadRight(lpWidth)} | {"Ticker".PadRight(symbolWidth)} | {"Name".PadRight(nameWidth)} | {"Market Cap ($)".PadRight(capWidth)} | {"Weight (%)".PadRight(weightWidth)} |");
            Console.WriteLine(horizontal);

            int i = 1;
            foreach (var company in companies.OrderByDescending(c => c.Weight))
            {
                string name = company.Name.Length > nameMaxLength ? company.Name.Substring(0, nameMaxLength - 3) + "..." : company.Name;
                string marketCapFormatted = company.MarketCap.ToString("N0").PadLeft(capWidth);
                string weightFormatted = company.Weight.ToString("F2").PadLeft(weightWidth);

                Console.WriteLine($"| {(i++ + ".").PadRight(lpWidth)} | {company.Symbol.PadRight(symbolWidth)} | {name.PadRight(nameWidth)} | {marketCapFormatted} | {weightFormatted} |");
            }

            Console.WriteLine(horizontal);
        }
    }
}
