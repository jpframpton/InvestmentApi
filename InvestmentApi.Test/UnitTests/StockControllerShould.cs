using InvestmentApi.Controllers;
using InvestmentShared.Interfaces;
using InvestmentShared.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace InvestmentApi.Test.UnitTests
{
    public class StockControllerShould
    {
        [Fact]
        public void ForGetStocks_ReturnHttpNotFound_ForInvalidStock()
        {
            // Arrange
            const string stockSymbol = "Z1X";
            var stockList = new List<Stock>()
            {
                new() {StockName = "Apple", StockSymbol = "AAPL"},
                new() {StockName = "Microsoft", StockSymbol = "MSFT"}
            };
            var mock = new Mock<IStockData>();
            mock.Setup(d => d.GetAllStocks()).Returns(stockList);
            var controller = new StocksController(mock.Object);

            // Act
            var result = controller.Get(stockSymbol).Result;

            // Assert
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(stockSymbol, notFoundObjectResult.Value);
        }
    }
}