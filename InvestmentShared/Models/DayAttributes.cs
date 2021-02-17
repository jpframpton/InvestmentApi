using Newtonsoft.Json;

namespace InvestmentShared.Models
{
    public class DayAttributes
    {
        [JsonProperty("open")]
        public decimal Open { get; set; }

        [JsonProperty("close")]
        public decimal Close { get; set; }

        [JsonProperty("high")]
        public decimal High { get; set; }

        [JsonProperty("low")]
        public decimal Low { get; set; }
    }
}