using NasdaqChecker.Models;
using NasdaqChecker.Services.Api;
using NasdaqChecker.Presentation;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Configuration;

class Program
{
    static async Task Main()
    {
        // Build config
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()  // The environmental variable will overwrite the value from the json file
            .Build();

        var fmpApiKey = config["ApiKeys:FMP_API_KEY"];
        var earningsApiKey = config["ApiKeys:EARNINGS_API_KEY"];

       //Console.WriteLine("FMP API Key from config: " + config["ApiKeys:FMP_API_KEY"]);
       //Console.WriteLine("Current environment: " + Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));


        var api = new NasdaqApi(fmpApiKey);

        Console.WriteLine("Downloading NASDAQ-100 composition...");
        var companies = await api.GetNasdaq100CompaniesAsync();

        Console.WriteLine("Downloading company capitalization...");
        await api.FetchMarketCapsAsync(companies);

        Console.WriteLine("Calculating weights...");
        api.CalculateWeights(companies);

        Console.WriteLine("\nData source: Financial Modeling Prep API (https://financialmodelingprep.com/)");
        Console.WriteLine($"NASDAQ-100 index data from: {api.NasdaqDataTimestamp}");
        Console.WriteLine($"Market capitalization data from: {api.MarketCapDataTimestamp}");

        Console.WriteLine("Done!\n");

        Console.WriteLine("Enter the ticker or company name to check if it belongs to the NASDAQ-100.");
        Console.WriteLine("Type ‘list’ to display a list of all companies.");
        Console.WriteLine("Type ‘earnings’ to see which NASDAQ-100 companies report earnings today.");
        Console.WriteLine("Type ‘exit’ to quit.");



        while (true)
        {
            Console.Write("\nCompany: ");
            var input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("No value entered.");
                continue;
            }

            if (input.ToLower() == "exit")
            {
                break;
            }

            if (input.ToLower() == "list")
            {
                Console.WriteLine("\n List of companies in the NASDAQ 100:\n");

                TablePrinter.PrintCompanyTable(companies);

                continue;
            }

            if (input.ToLower() == "earnings")
            {
                var earningsService = new EarningsReportService(earningsApiKey);

                var earningsToday = await earningsService.GetTodayReportsForAsync(companies);

                if (earningsToday.Count == 0)
                {
                    Console.WriteLine("Today, no NASDAQ-100 companies are reporting earnings.");
                }
                else
                {
                    Console.WriteLine("Today's earnings reports (NASDAQ-100):");
                    foreach (var report in earningsToday)
                    {
                        //Console.WriteLine($"- {report.Symbol} ({report.Company}) at {report.Time} | Est. EPS: {report.EstimatedEPS} | Actual EPS: {report.EPS}");
                        Console.WriteLine($"- {report.Symbol} ({report.Company}) at {report.Time} | Est. EPS: {(report.EstimatedEPS?.ToString("F2") ?? "N/A")} | Actual EPS: {(report.EPS?.ToString("F2") ?? "N/A")}");

                    }
                }

                continue;
            }

            string inputUpper = input.ToUpper();

            var match = companies.FirstOrDefault(c => c.Symbol == inputUpper || c.Name.ToUpper().Contains(inputUpper));

            if (match != null)
            {
                Console.WriteLine($"{match.Name} ({match.Symbol}) is part of the NASDAQ-100 (${match.MarketCap.ToString("N0")} Market Cap - {match.Weight.ToString("F2")}% of index value).");
            }
            else
            {
                Console.WriteLine("The company is not listed on the NASDAQ-100.");
            }
        }

        Console.WriteLine("Bye!");
    }
}