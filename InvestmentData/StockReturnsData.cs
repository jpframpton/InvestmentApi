using System;
using System.Collections.Generic;
using InvestmentShared.Interfaces;
using InvestmentShared.Models;

namespace InvestmentData
{
    public class StockReturnsData : IStockReturnsData
    {
        public StockReturn GetStockReturnsBySymbolOverDateRange(string symbol, DateTime startDate, DateTime endDate)
        {
            if (symbol.ToUpperInvariant().Equals("AAPL"))
            {
                return new StockReturn()
                {
                    StockSymbol = symbol.ToUpperInvariant(),
                    Returns = new List<Return>
                    {
                        new()
                        {
                            Close = 134.35M, Open = 135.36M, High = 135.52M, Low = 133.68M, PreviousDayClose = 135.11M,
                            ReturnDate = Convert.ToDateTime(@"2/16/2021"), ReturnPercent = -.33M
                        },
                        new()
                        {
                            Close = 133.23M, Open = 135.50M, High = 136.01M, Low = 132.78M, PreviousDayClose = 134.35M,
                            ReturnDate = Convert.ToDateTime(@"2/17/2021"), ReturnPercent = -.33M
                        }
                    }
                };
            }

            return new StockReturn(){StockSymbol = symbol.ToUpperInvariant()};
        }
    }
}