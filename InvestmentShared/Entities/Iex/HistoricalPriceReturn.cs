using System;
using Newtonsoft.Json;

namespace InvestmentShared.Entities.Iex
{
    public class HistoricalPriceReturn
    {
        [JsonProperty("close")]
        public decimal Close { get; set; }

        [JsonProperty("high")]
        public decimal High { get; set; }

        [JsonProperty("low")]
        public decimal Low { get; set; }

        [JsonProperty("open")]
        public decimal Open { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("volume")]
        public long Volume { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("subkey")]
        public string Subkey { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("updated")]
        public long Updated { get; set; }

        [JsonProperty("changeOverTime")]
        public long ChangeOverTime { get; set; }

        [JsonProperty("marketChangeOverTime")]
        public long MarketChangeOverTime { get; set; }

        [JsonProperty("uOpen")]
        public decimal UOpen { get; set; }

        [JsonProperty("uClose")]
        public decimal UClose { get; set; }

        [JsonProperty("uHigh")]
        public decimal UHigh { get; set; }

        [JsonProperty("uLow")]
        public decimal ULow { get; set; }

        [JsonProperty("uVolume")]
        public long UVolume { get; set; }

        [JsonProperty("fOpen")]
        public decimal FOpen { get; set; }

        [JsonProperty("fClose")]
        public decimal FClose { get; set; }

        [JsonProperty("fHigh")]
        public decimal FHigh { get; set; }

        [JsonProperty("fLow")]
        public decimal FLow { get; set; }

        [JsonProperty("fVolume")]
        public long FVolume { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("change")]
        public long Change { get; set; }

        [JsonProperty("changePercent")]
        public long ChangePercent { get; set; }
    }
}