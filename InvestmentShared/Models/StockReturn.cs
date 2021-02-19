using Newtonsoft.Json;
using System.Collections.Generic;

namespace InvestmentShared.Models
{
    public class StockReturn
    {
        [JsonProperty("stock")]
        public Stock Stock { get; set; }

        [JsonProperty("returns")]
        public List<ReturnInfo> Returns { get; set; }
    }
}