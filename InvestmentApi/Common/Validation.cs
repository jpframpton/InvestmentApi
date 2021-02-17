using System;
using System.Collections.Generic;
using System.Linq;
using InvestmentShared.Interfaces;
using InvestmentShared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace InvestmentApi.Common
{
    public static class Validation
    {
        public static bool CheckSymbolFound(string symbol, IStockData stockData)
        {
            var allStocks = stockData.GetAllStocks();
            return CheckSymbolFound(symbol, allStocks);
        }

        public static bool CheckSymbolFound(string symbol, List<Stock> allStocks)
        {
            var found = allStocks.FirstOrDefault(s => s.StockSymbol.Equals(symbol.ToUpper()))?.StockSymbol;
            return !string.IsNullOrEmpty(found);
        }

        public static DateTime GetStartDateValue(string startDate)
        {
            if (string.IsNullOrEmpty(startDate)) return new DateTime(DateTime.Now.Year, 1, 1);

            if (DateTime.TryParse(startDate, out var result)) return result;

            throw new BadHttpRequestException("Start date is invalid");
        }

        public static DateTime GetEndDateValue(string endDate)
        {
            if (string.IsNullOrEmpty(endDate)) return new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            if (DateTime.TryParse(endDate, out var result)) return result;

            throw new BadHttpRequestException("End date is invalid");
        }

        public static void CheckDateRange(DateTime startDate, DateTime endDate, int maxDaysInRange)
        {
            if ((endDate - startDate).TotalDays > maxDaysInRange)
                throw new BadHttpRequestException(
                    $"Date range between start and end date is greater than {maxDaysInRange} days.");
        }
    }
}