using System;
using Newtonsoft.Json;

namespace InvestmentShared.Models
{
    public class ReturnInfo : DayAttributes
    {
        [JsonProperty("returnDate")]
        public DateTime ReturnDate { get; set; }

        [JsonProperty("returnPercent")]
        public decimal? ReturnPercent { get; set; }

        [JsonProperty("previousDayClose")]
        public decimal? PreviousDayClose { get; set; }

        public ReturnInfo() { }

        public ReturnInfo(DateTime returnDate, decimal open, decimal close, decimal high, decimal low, decimal? previousDayClose)
        {
            ReturnDate = returnDate;
            Open = open;
            Close = close;
            High = high;
            Low = low;
            PreviousDayClose = previousDayClose;
            if (previousDayClose.HasValue)
            {
                ReturnPercent = ((close - previousDayClose) / previousDayClose) * 100;
            }
            else
            {
                ReturnPercent = null;
            }
        }

        public decimal? GetReturnPercent()
        {
            return (Close - PreviousDayClose) / PreviousDayClose * 100;
        }
    }
}