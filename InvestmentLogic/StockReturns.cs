using InvestmentData.Access;
using InvestmentShared.Interfaces;
using InvestmentShared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvestmentLogic
{
    public class StockReturns : IStockReturns
    {
        private readonly IStockReturnsData _iStockReturnsData;

        public StockReturns(IStockReturnsData iStockReturnsData)
        {
            _iStockReturnsData = iStockReturnsData ?? new StockReturnsData();
        }

        public async Task<(StockReturn stockReturn, string message)> GetStockReturnsBySymbolOverDateRangeAsync(string symbol, DateTime? startDate,
            DateTime? endDate)
        {
            var stockReturnRetVal = new StockReturn();
            var returns = new List<ReturnInfo>();
            var (historicalPriceReturns, message)
                = await _iStockReturnsData.GetStockReturnsBySymbolOverDateRangeAsync(symbol, startDate, endDate);

            if (!string.IsNullOrEmpty(message))
            {
                return (new StockReturn(), message);
            }

            stockReturnRetVal.Stock = new Stock()
            {
                StockSymbol = symbol
            };

            //TODO: Confirm that having a null return for the first day is acceptable.
            //TODO: If not, then the day previous to the beginning of the date range will need to be returned as well
            decimal? previousDayClose = null;
            foreach (var historicalPriceReturn in historicalPriceReturns)
            {
                returns.Add(new ReturnInfo(historicalPriceReturn.Date, historicalPriceReturn.Open,
                    historicalPriceReturn.Close, historicalPriceReturn.High, historicalPriceReturn.Low,
                    previousDayClose));
                previousDayClose = historicalPriceReturn.Close;
            }

            stockReturnRetVal.Returns = returns;
            return (stockReturnRetVal, string.Empty);
        }

        public async Task<(decimal? alphaValue, string message)> GetAlphaValueBySymbolOverDateRangeAsync(string symbol, string benchmark, DateTime? startDate, DateTime? endDate)
        {
            (startDate, endDate) = SetDates(startDate, endDate);

            var (primaryReturn, primaryMessage) = await GetReturnBySymbolOverDateRangeAsync(symbol, startDate, endDate);
            if(!string.IsNullOrEmpty(primaryMessage)) return (null, primaryMessage);
            var (benchmarkReturn, benchmarkMessage) = await GetReturnBySymbolOverDateRangeAsync(benchmark, startDate, endDate);
            if(!string.IsNullOrEmpty(benchmarkMessage)) return (null, benchmarkMessage);

            decimal? alphaValue = null;
            // Rate of return for stock - rate of return for benchmark
            if (primaryReturn.ReturnPercent.HasValue && benchmarkReturn.ReturnPercent.HasValue)
            {
                alphaValue = primaryReturn.ReturnPercent.Value - benchmarkReturn.ReturnPercent.Value;
            }

            return (alphaValue, string.Empty);
        }

        private static (DateTime? StartDate, DateTime? EndDate) SetDates(DateTime? startDate, DateTime? endDate)
        {
            // Use year to date if either startDate or endDate is null
            if (!startDate.HasValue || !endDate.HasValue)
            {
                startDate = new DateTime(DateTime.Now.Year, 1, 1);
                endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            }

            return (startDate, endDate);
        }

        private async Task<(ReturnInfo retVal, string message)> GetReturnBySymbolOverDateRangeAsync(string symbol, DateTime? startDate, DateTime? endDate)
        {
            var returnVal = new ReturnInfo();
            var (returns, message)
                = await _iStockReturnsData.GetStockReturnsBySymbolOverDateRangeAsync(symbol, startDate, endDate);
            if (!string.IsNullOrEmpty(message)) return (null, message);
            var firstDayReturn = returns.FirstOrDefault();
            var lastDayReturn = returns.LastOrDefault();

            // create a return using these two
            if (firstDayReturn != null && lastDayReturn != null)
            {
                returnVal = new ReturnInfo(firstDayReturn.Date, firstDayReturn.Open,
                    firstDayReturn.Close, firstDayReturn.High, firstDayReturn.Low,
                    lastDayReturn.Close);
            }

            return (returnVal, string.Empty);
        }
    }
}