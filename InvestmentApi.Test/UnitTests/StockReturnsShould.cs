using InvestmentLogic;
using InvestmentShared.Entities.Iex;
using InvestmentShared.Interfaces;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Xunit;

namespace InvestmentApi.Test.UnitTests
{
    public class StockReturnsShould
    {
        [Fact]
        public void ForGetStockReturns_ReturnNotFoundMessage_ForInvalidStock()
        {
            // Arrange
            const string stockSymbol = "Z1X";
            var startDate = Convert.ToDateTime("12/9/2020");
            var endDate = Convert.ToDateTime("12/12/2020");
            const string notFoundMessage = "Not Found";
            var mock = new Mock<IStockReturnsData>();

            var historicPriceReturns = new List<HistoricalPriceReturn>();
            var historicPriceReturnsWithMessage = (historicPriceReturns, notFoundMessage);
            mock.Setup(d => d.GetStockReturnsBySymbolOverDateRangeAsync(stockSymbol, startDate, endDate))
                .ReturnsAsync(historicPriceReturnsWithMessage);
             var stockReturn = new StockReturns(mock.Object);

            // Act
            var result = stockReturn.GetStockReturnsBySymbolOverDateRangeAsync(stockSymbol, startDate, endDate).Result;

            // Assert
            Assert.Equal(notFoundMessage, result.message);
        }

        [Fact]
        public void ForGetStockReturns_ReturnStockReturns_ForValidStock()
        {
            // Arrange
            const string stockSymbol = "AAPL";
            var startDate = Convert.ToDateTime("12/9/2020");
            var endDate = Convert.ToDateTime("12/12/2020");
            var message = string.Empty;
            var mock = new Mock<IStockReturnsData>();
            
            var historicPriceReturns = JsonConvert.DeserializeObject<List<HistoricalPriceReturn>>(TestConstants.StockReturnsData.GoodData);
            var expectedStockReturnsCount = historicPriceReturns.Count;
            var historicPriceReturnsWithMessage = (historicPriceReturns, notFoundMessage: message);
            mock.Setup(d => d.GetStockReturnsBySymbolOverDateRangeAsync(stockSymbol, startDate, endDate))
                .ReturnsAsync(historicPriceReturnsWithMessage);
            var stockReturn = new StockReturns(mock.Object);

            // Act
            var result = stockReturn.GetStockReturnsBySymbolOverDateRangeAsync(stockSymbol, startDate, endDate).Result;

            // Assert
            Assert.Equal(message, result.message);
            Assert.Equal(expectedStockReturnsCount, result.stockReturn.Returns.Count);
            Assert.Equal(stockSymbol, result.stockReturn.Stock.StockSymbol);
        }

        [Fact]
        public void ForGetAlphaValue_ReturnNotFoundMessage_ForInvalidStock()
        {
            // Arrange
            const string stockSymbol = "Z1X";
            const string benchmarkSymbol = "MSFT";
            var startDate = Convert.ToDateTime("12/9/2020");
            var endDate = Convert.ToDateTime("12/12/2020");
            const string notFoundMessage = "Not Found";
            var mock = new Mock<IStockReturnsData>();

            var historicPriceReturns = new List<HistoricalPriceReturn>();
            var historicPriceReturnsWithMessage = (historicPriceReturns, notFoundMessage);
            mock.Setup(d => d.GetStockReturnsBySymbolOverDateRangeAsync(stockSymbol, startDate, endDate))
                .ReturnsAsync(historicPriceReturnsWithMessage);
            var stockReturn = new StockReturns(mock.Object);

            // Act
            var (_, message) = stockReturn.GetAlphaValueBySymbolOverDateRangeAsync(stockSymbol, benchmarkSymbol, startDate, endDate).Result;

            // Assert
            Assert.Equal(notFoundMessage, message);
        }

        [Fact]
        public void ForGetAlphaValue_ReturnStockReturns_ForValidStock()
        {
            // Arrange
            const string stockSymbol = "AAPL";
            const string benchmarkSymbol = "MSFT";
            var startDate = Convert.ToDateTime("12/9/2020");
            var endDate = Convert.ToDateTime("12/12/2020");
            var message = string.Empty;
            var mock = new Mock<IStockReturnsData>();

            var historicPriceReturns = JsonConvert.DeserializeObject<List<HistoricalPriceReturn>>(TestConstants.StockReturnsData.GoodData);
            var historicPriceReturnsWithMessage = (historicPriceReturns, notFoundMessage: message);
            mock.Setup(d => d.GetStockReturnsBySymbolOverDateRangeAsync(stockSymbol, startDate, endDate))
                .ReturnsAsync(historicPriceReturnsWithMessage);
            mock.Setup(d => d.GetStockReturnsBySymbolOverDateRangeAsync(benchmarkSymbol, startDate, endDate))
                .ReturnsAsync(historicPriceReturnsWithMessage);
            var stockReturn = new StockReturns(mock.Object);

            // Act
            var (alphaValue, s) = stockReturn.GetAlphaValueBySymbolOverDateRangeAsync(stockSymbol, benchmarkSymbol, startDate, endDate).Result;

            // Assert
            Assert.Equal(message, s);
            Assert.Equal(0, alphaValue); // same data should result in 0 difference
        }

    }
}