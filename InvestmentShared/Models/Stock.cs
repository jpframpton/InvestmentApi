using Newtonsoft.Json;

namespace InvestmentShared.Models
{
    public class Stock
    {
        [JsonProperty("stockSymbol")]
        public string StockSymbol { get; set; }

        [JsonProperty("stockName")]
        public string StockName { get; set; }
    }
}