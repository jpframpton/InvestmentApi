using System.Collections.Generic;
using InvestmentShared.Interfaces;
using InvestmentShared.Models;

namespace InvestmentData
{
    public class StockData : IStockData
    {
        public List<Stock> GetAllStocks()
        {
           return new List<Stock>()
            {
                new() {StockName = "Apple", StockSymbol = "AAPL"},
                new() {StockName = "Microsoft", StockSymbol = "MSFT"}
            };
        }
    }
}