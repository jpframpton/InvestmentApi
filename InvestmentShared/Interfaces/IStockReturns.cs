using InvestmentShared.Models;
using System;
using System.Threading.Tasks;

namespace InvestmentShared.Interfaces
{
    public interface IStockReturns
    {
        Task<(StockReturn stockReturn, string message)> GetStockReturnsBySymbolOverDateRangeAsync(string symbol, DateTime? startDate, DateTime? endDate);

        Task<(decimal? alphaValue, string message)> GetAlphaValueBySymbolOverDateRangeAsync(string symbol,
            string benchmark, DateTime? startDate, DateTime? endDate);
    }
}