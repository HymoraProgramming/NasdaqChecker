using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NasdaqChecker.Models
{
    public class NasdaqCompany
    {
        public string Symbol { get; set; }
        public string Name { get; set; }
        public double MarketCap { get; set; }
        public double Weight {  get; set; }



        public NasdaqCompany(string symbol, string name)
        {
            Symbol = (symbol ?? "").ToUpper(); // if null, set empty string
            Name = name ?? "";
        }
    }
}
