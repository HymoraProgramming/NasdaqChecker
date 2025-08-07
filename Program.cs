using NasdaqChecker;

class Program
{
    static async Task Main()
    {
        var api = new NasdaqApi();

        Console.WriteLine("Downloading data from the Internet...");
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