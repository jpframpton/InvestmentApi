using System;
using InvestmentData.Access;
using Xunit;

namespace InvestmentApi.Test.IntegrationTests
{
    public class StockReturnsDataShould
    {
        [Fact]
        public void ForGetStockReturns_YTD_ReturnNotFoundMessage_ForInvalidStock()
        {
            // Arrange
            const string stockSymbol = "Z1X";
            const string expectedMessage = "Not Found";
            var stockReturnsData = new StockReturnsData(); 

            // Act
            var (_, message) = stockReturnsData.GetStockReturnsBySymbolOverDateRangeAsync(stockSymbol, null, null).Result;

            // Assert
            Assert.Equal(expectedMessage, message);
        }

        [Fact]
        public void ForGetStockReturns_YTD_ReturnHistoricalPriceReturnRecords_ForValidStock()
        {
            // Arrange
            const string stockSymbol = "MSFT";
            var stockReturnsData = new StockReturnsData();

            // Act
            var (historicalPriceReturns, message) = stockReturnsData.GetStockReturnsBySymbolOverDateRangeAsync(stockSymbol, null, null).Result;

            // Assert
            //TODO: Modify test to account for only business days as this will fail on years that begin on holiday or weekend
            Assert.True(historicalPriceReturns.Count > 0);
            Assert.True(string.IsNullOrEmpty(message));
        }

        [Fact]
        public void ForGetStockReturns_OverDateRange_ReturnNotFoundMessage_ForInvalidStock()
        {
            // Arrange
            const string stockSymbol = "Z1X";
            const string expectedMessage = "Not Found";
            var startDate = Convert.ToDateTime("12/1/2020");
            var endDate = Convert.ToDateTime("12/31/2020");
            var stockReturnsData = new StockReturnsData();

            // Act
            var (_, message) = stockReturnsData.GetStockReturnsBySymbolOverDateRangeAsync(stockSymbol, startDate, endDate).Result;

            // Assert
            Assert.Equal(expectedMessage, message);
        }

        [Fact]
        public void ForGetStockReturns_OverDateRange_ReturnHistoricalPriceReturnRecords_ForValidStock()
        {
            // Arrange
            const string stockSymbol = "AAPL";
            var startDate = Convert.ToDateTime("12/9/2020");
            var endDate = Convert.ToDateTime("12/12/2020");
            var stockReturnsData = new StockReturnsData();

            // Act
            var (historicalPriceReturns, message) = stockReturnsData.GetStockReturnsBySymbolOverDateRangeAsync(stockSymbol, startDate, endDate).Result;

            // Assert
            Assert.True(historicalPriceReturns.Count > 0);
            Assert.True(string.IsNullOrEmpty(message));
        }

        [Fact]
        public void ForGetStockReturns_OverDateRangeOfOneDay_ReturnHistoricalPriceReturnRecordsOfOneRecord_ForValidStock()
        {
            // Arrange
            const string stockSymbol = "AAPL";
            var startDate = Convert.ToDateTime("12/10/2020");
            var endDate = Convert.ToDateTime("12/10/2020");
            var stockReturnsData = new StockReturnsData();

            // Act
            var (historicalPriceReturns, message) = stockReturnsData.GetStockReturnsBySymbolOverDateRangeAsync(stockSymbol, startDate, endDate).Result;

            // Assert
            Assert.True(historicalPriceReturns.Count == 1);
            Assert.True(string.IsNullOrEmpty(message));
        }
    }
}