using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NasdaqChecker.Models
{
    public class EarningsApiResponse
    {
        public List<EarningsApiItem> PreMarket { get; set; }
        public List<EarningsApiItem> AfterMarket { get; set; }
        public List<EarningsApiItem> Undefined { get; set; }

        public List<EarningsApiItem> AllReports =>
            (PreMarket ?? new()).Concat(AfterMarket ?? new()).Concat(Undefined ?? new()).ToList();
    }
    public class EarningsApiItem
    {
        public string Ticker { get; set; }
        public string Company { get; set; }
        public string ReportTime { get; set; } // "Before Market Open", "After Market Close", etc.
        public double? EpsEstimate { get; set; }
        public double? EpsActual { get; set; }
    }
}
