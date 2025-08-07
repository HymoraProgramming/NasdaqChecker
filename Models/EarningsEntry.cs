using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NasdaqChecker.Models
{
    public class EarningsEntry
    {
        public string LastYearRptDt { get; set; }
        public string LastYearEPS { get; set; }
        public string Time { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public long MarketCap { get; set; }
        public string FiscalQuarterEnding { get; set; }
        public string EpsForecast { get; set; }
        public string NoOfEsts { get; set; }
    }
}
