using NasdaqChecker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Globalization;

namespace NasdaqChecker.Services.Api
{
    public class EarningsReportService
    {
        private readonly HttpClient _client = new HttpClient();

        private readonly string _apiKey;
        public EarningsReportService(string apiKey)
        {
            _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey), "Missing EarningsAPI key");
        }
        public async Task<List<EarningsReport>> GetTodayReportsForAsync(List<NasdaqCompany> companies)
        {
            var reports = new List<EarningsReport>();

            string url = $"https://api.earningsapi.com/v1/calendar/{DateTime.Now:yyyy-MM-dd}?apikey={_apiKey}";

            var json = await _client.GetStringAsync(url);

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var response = JsonSerializer.Deserialize<EarningsCalendarResponse>(json, options);

            if (response == null)
                return reports;

            var allReports = response.Pre.Concat(response.After).Concat(response.NotSupplied);

            // Only NASDAQ-100 companies
            var nasdaqSymbols = new HashSet<string>(companies.Select(c => c.Symbol));

            foreach (var r in allReports)
            {
                double? estimatedEPS = null;
                double? actualEPS = null;

                if (!string.IsNullOrWhiteSpace(r.EpsForecast))
                {
                    var cleaned = r.EpsForecast.Replace("$", "").Trim();
                    if (cleaned.StartsWith("(") && cleaned.EndsWith(")"))
                        cleaned = "-" + cleaned.Trim('(', ')');
                    if (double.TryParse(cleaned, NumberStyles.Any, CultureInfo.InvariantCulture, out var parsed))
                    {
                        estimatedEPS = parsed;
                    }
                }

                if (!string.IsNullOrWhiteSpace(r.LastYearEPS))
                {
                    var cleaned = r.LastYearEPS.Replace("$", "").Trim();
                    if (cleaned.StartsWith("(") && cleaned.EndsWith(")"))
                        cleaned = "-" + cleaned.Trim('(', ')');
                    if (double.TryParse(cleaned, NumberStyles.Any, CultureInfo.InvariantCulture, out var parsed))
                    {
                        actualEPS = parsed;
                    }
                }

                if (nasdaqSymbols.Contains(r.Symbol))
                {
                    reports.Add(new EarningsReport
                    {
                        Symbol = r.Symbol,
                        Company = r.Name,
                        Time = r.Time,
                        EstimatedEPS = estimatedEPS,
                        EPS = actualEPS
                    });
                }
            }

            return reports;
        }




    }
}
