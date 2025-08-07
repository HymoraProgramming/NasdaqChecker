using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NasdaqChecker.Models
{
    public class EarningsCalendarResponse
    {
        public string Date { get; set; }
        public List<EarningsEntry> Pre { get; set; } = new();
        public List<EarningsEntry> After { get; set; } = new();
        public List<EarningsEntry> NotSupplied { get; set; } = new();
    }
}
