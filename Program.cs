using NasdaqChecker;

class Program
{
    static async Task Main()
    {
        var api = new NasdaqApi();
        Console.WriteLine("Pobieranie danych z internetu...");
        var companies = await api.GetNasdaq100CompaniesAsync();

        Console.WriteLine("Pobieranie kapitalizacji spółek...");
        await api.FetchMarketCapsAsync(companies);

        Console.WriteLine("Obliczanie wag...");
        api.CalculateWeights(companies);

        Console.WriteLine("\nŹródło danych: Financial Modeling Prep API (https://financialmodelingprep.com/)");
        Console.WriteLine($"Dane o składzie NASDAQ-100 z: {api.NasdaqDataTimestamp}");
        Console.WriteLine($"Dane o kapitalizacji rynkowej z: {api.MarketCapDataTimestamp}");

        Console.WriteLine("Gotowe!\n");

        Console.WriteLine("Wpisz ticker lub nazwę spółki, aby sprawdzić, czy należy do NASDAQ-100.");
        Console.WriteLine("Wpisz 'exit', aby zakończyć.");
        Console.WriteLine("Wpisz 'list', aby wyświetlić listę wszystkich spółek.");



        while (true)
        {
            // Mogłoby być Console.WriteLine(); zamiast \n
            Console.Write("\nSpółka: ");
            var input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Nie wpisano żadnej wartości.");
                continue;
            }

            if (input.ToLower() == "exit")
            {
                break;
            }

            if (input.ToLower() == "list")
            {
                Console.WriteLine("\n Lista spółek w NASDAQ 100:\n");

                TablePrinter.PrintCompanyTable(companies);

                continue;
            }

            string inputUpper = input.ToUpper();

            var match = companies.FirstOrDefault(c => c.Symbol == inputUpper || c.Name.ToUpper().Contains(inputUpper));

            if (match != null)
            {
                Console.WriteLine($"{match.Name} ({match.Symbol}) należy do NASDAQ-100 (${match.MarketCap.ToString("N0")} Market Cap - {match.Weight.ToString("F2")}% wartości indeksu).");
            }
            else
            {
                Console.WriteLine("Podana spółka nie znajduje się w NASDAQ-100.");
            }
        }

        Console.WriteLine("Do widzenia!");
    }
}