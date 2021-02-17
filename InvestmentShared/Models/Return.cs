using System;
using Newtonsoft.Json;

namespace InvestmentShared.Models
{
    public class Return : DayAttributes
    {
        [JsonProperty("returnDate")]
        public DateTime ReturnDate { get; set; }

        [JsonProperty("returnPercent")]
        public decimal ReturnPercent { get; set; }

        [JsonProperty("previousDayClose")]
        public decimal PreviousDayClose { get; set; }
    }
}