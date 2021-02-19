using InvestmentShared.Entities.Iex;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InvestmentShared.Interfaces
{
    public interface IStockReturnsData
    {
        Task<(List<HistoricalPriceReturn> historicalPriceReturns, string message)> GetStockReturnsBySymbolOverDateRangeAsync(string symbol, DateTime? startDate, DateTime? endDate);
    }
}