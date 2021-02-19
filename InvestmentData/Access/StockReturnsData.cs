using InvestmentShared.Constants;
using InvestmentShared.Entities.Iex;
using InvestmentShared.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace InvestmentData.Access
{
    public class StockReturnsData : IStockReturnsData
    {
        //TODO: Move constant to Constants file
        private const string IexTokenKey = "iexToken";

        public async Task<(List<HistoricalPriceReturn> historicalPriceReturns, string message)>
            GetStockReturnsBySymbolOverDateRangeAsync(string symbol,
                DateTime? startDate, DateTime? endDate)
        {
            using var client = new HttpClient { BaseAddress = new Uri(IexWebApi.BaseUrl) };
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (startDate == null || endDate == null)
            {
                return await GetYearToDateReturns(client, symbol);
            }
            return await GetDateRangeReturns(client, symbol, startDate, endDate);
        }

        private static async Task<(List<HistoricalPriceReturn> historicalPriceReturns, string message)> GetYearToDateReturns(HttpClient client, string symbol)
        {
            var historicalPriceReturns = new List<HistoricalPriceReturn>();
            var message = string.Empty;

            var response = await client.GetAsync($"stock/{symbol}/chart/ytd?token={GetIexToken()}");
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                historicalPriceReturns = JsonConvert.DeserializeObject<List<HistoricalPriceReturn>>(result);
            }
            else
            {
                message = response.ReasonPhrase;
            }
            return (historicalPriceReturns, message);
        }

        private static async Task<(List<HistoricalPriceReturn> historicalPriceReturns, string message)> GetDateRangeReturns(HttpClient client, string symbol, DateTime? startDate, DateTime? endDate)
        {
            var historicalPriceReturns = new List<HistoricalPriceReturn>();
            var message = string.Empty;
            var iexToken = GetIexToken();

            if (startDate != null && endDate != null)
            {
                foreach (var day in EachDay(startDate.Value, endDate.Value))
                {
                    var response = await client.GetAsync($"stock/{symbol}/chart/date/{day:yyyyMMdd}?chartByDay=true&token={iexToken}");
                    if (response.IsSuccessStatusCode)
                    {
                        var result = response.Content.ReadAsStringAsync().Result;
                        historicalPriceReturns.AddRange(JsonConvert.DeserializeObject<List<HistoricalPriceReturn>>(result));
                    }
                    else
                    {
                        message = response.ReasonPhrase;
                        break;
                    }
                }
            }
            
            return (historicalPriceReturns, message);
        }

        //TODO: Move to utility class
        private static IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }

        private static string GetIexToken()
        {
            //TODO: Debug why ConfigurationManager works in Test but not in actual runtime
            var retVal = ConfigurationManager.AppSettings[IexTokenKey];
            if (string.IsNullOrEmpty(retVal)) retVal = "Tsk_0934e80f0fbc46729bc1a3f958a12031";
            return retVal;
        }
    }
}