using System;
using System.Collections.Generic;
using InvestmentShared.Models;

namespace InvestmentShared.Interfaces
{
    public interface IStockReturnsData
    {
        StockReturn GetStockReturnsBySymbolOverDateRange(string symbol, DateTime startDate, DateTime endDate);
    }
}