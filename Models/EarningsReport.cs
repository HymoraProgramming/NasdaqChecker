using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NasdaqChecker.Models
{
    public class EarningsReport
    {
        public string Symbol { get; set; }
        public string Company { get; set; }
        public string Time { get; set; }
        public double? EstimatedEPS { get; set; }
        public double? EPS { get; set; }
    }
}
