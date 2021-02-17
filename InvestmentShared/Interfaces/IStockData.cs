using System.Collections.Generic;
using InvestmentShared.Models;

namespace InvestmentShared.Interfaces
{
    public interface IStockData
    {
        List<Stock> GetAllStocks();
    }
}