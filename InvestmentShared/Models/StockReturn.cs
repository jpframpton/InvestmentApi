using Newtonsoft.Json;
using System.Collections.Generic;
using InvestmentShared.Models;

namespace InvestmentShared.Models
{
    public class StockReturn
    {
        [JsonProperty("stockSymbol")]
        public string StockSymbol { get; set; }

        [JsonProperty("returns")]
        public List<Return> Returns { get; set; }
    }
}