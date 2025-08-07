using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;

namespace NasdaqChecker
{
    public class NasdaqApi
    {
        public DateTime NasdaqDataTimestamp { get; private set; }
        public DateTime MarketCapDataTimestamp { get; private set; }

        private readonly HttpClient _client = new HttpClient();

        private readonly string _apiKey = Environment.GetEnvironmentVariable("FMP_API_KEY") ?? throw new InvalidOperationException("Brak klucza API");

        public async Task<List<NasdaqCompany>> GetNasdaq100CompaniesAsync()
        {
            const string cachePath = "nasdaq100_cache.json";

            var cached = await CacheHelper.LoadFromCacheAsync<List<NasdaqCompany>>(cachePath, TimeSpan.FromDays(1), ts => NasdaqDataTimestamp = ts);
            if (cached != null && cached.Any())
            {
                NasdaqDataTimestamp = File.GetLastWriteTime(cachePath);
                return cached;
            }

            string url = $"https://financialmodelingprep.com/api/v3/nasdaq_constituent?apikey={_apiKey}";
            var response = await _client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            //using var stream = await response.Content.ReadAsStreamAsync();

            var json = await response.Content.ReadAsStringAsync();
            var companies = JsonSerializer.Deserialize<List<NasdaqCompany>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<NasdaqCompany>();

            await CacheHelper.SaveToCacheAsync(cachePath, companies);
            NasdaqDataTimestamp = DateTime.Now;
            return companies;
        }

        public async Task FetchMarketCapsAsync(List<NasdaqCompany> companies)
        {
            const string cachePath = "market_caps_cache.json";
            var cachedCaps = await CacheHelper.LoadFromCacheAsync<List<NasdaqCompany>>(cachePath, TimeSpan.FromDays(1), ts => MarketCapDataTimestamp = ts);
            if (cachedCaps != null && cachedCaps.Count == companies.Count)
            {
                for (int i = 0; i < companies.Count; i++)
                    companies[i].MarketCap = cachedCaps[i].MarketCap;

                MarketCapDataTimestamp = File.GetLastWriteTime(cachePath);
                return;
            }

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            string joinedSymbols = string.Join(",", companies.Select(c => c.Symbol));

            string url = $"https://financialmodelingprep.com/api/v3/profile/{joinedSymbols}?apikey={_apiKey}";

            var json = await _client.GetStringAsync(url);

            var profiles = JsonSerializer.Deserialize<List<CompanyProfile>>(json, options);

            var profileDict = profiles?.ToDictionary(p => p.symbol, StringComparer.OrdinalIgnoreCase) ?? new();

            foreach (var company in companies)
            {
                if (profileDict.TryGetValue(company.Symbol, out var profile))
                    company.MarketCap = profile.mktCap;
                else
                    company.MarketCap = 0;
            }

            await CacheHelper.SaveToCacheAsync(cachePath, companies);
            MarketCapDataTimestamp = DateTime.Now;

            //var tasks = companies.Select(async company =>
            //{
            //    try
            //    {
            //        string url = $"https://financialmodelingprep.com/api/v3/profile/{company.Symbol}?apikey={_apiKey}";
            //        var json = await _client.GetStringAsync(url);
            //        var profiles = JsonSerializer.Deserialize<List<CompanyProfile>>(json, options);
            //        company.MarketCap = profiles?.FirstOrDefault()?.mktCap ?? 0;

            //    }
            //    catch
            //    {
            //        company.MarketCap = 0;
            //    }
            //});

            //await Task.WhenAll(tasks);
            //await CacheHelper.SaveToCacheAsync(cachePath, companies);
            //MarketCapDataTimestamp = DateTime.Now;
        }

        public void CalculateWeights(List<NasdaqCompany> companies)
        {
            double totalMarketCap = companies.Sum(c => c.MarketCap);
            foreach (var company in companies)
            {
                company.Weight = totalMarketCap > 0 ? (company.MarketCap / totalMarketCap) * 100 : 0;
            }
        }

    }

    public class CompanyProfile
    {

        public string symbol { get; set; }
        public double mktCap { get; set; }
        public string companyName { get; set; }
    }
}
