using InvestmentApi.Common;
using InvestmentApi.Controllers;
using InvestmentShared.Interfaces;
using InvestmentShared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using System;
using Xunit;

namespace InvestmentApi.Test.UnitTests
{
    public class ReturnsControllerShould
    {
        [Fact]
        public void ForGetStocks_ReturnHttpNotFound_ForInvalidStock()
        {
            // Arrange
            const string stockSymbol = "Z1X";
            const string notFoundMessage = "Not Found";
            var mock = new Mock<IStockReturns>();
            var settings = new ConfigurationVariables()
            {
                MaxDaysInDateRange = 365
            };
            var configurationVariables = Options.Create(settings);

            var stockReturn = new StockReturn();
            var stockReturnWithMessage = (stockReturn, notFoundMessage);
            mock.Setup(d => d.GetStockReturnsBySymbolOverDateRangeAsync(stockSymbol, It.IsAny<DateTime?>(), It.IsAny<DateTime?>()))
                .ReturnsAsync(stockReturnWithMessage);

            var controller = new ReturnsController(mock.Object, configurationVariables);

            // Act
            var result = controller.Get(stockSymbol).Result.Result;

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void ForGetStockReturns_ReturnStockReturns_ForValidStock()
        {
            // Arrange
            const string stockSymbol = "AAPL";
            var message = string.Empty;
            var mock = new Mock<IStockReturns>();
            var settings = new ConfigurationVariables()
            {
                MaxDaysInDateRange = 365
            };
            var configurationVariables = Options.Create(settings);

            var stockReturns = JsonConvert.DeserializeObject<StockReturn>(TestConstants.StockReturns.GoodData);
            var stockReturnsWithMessage = (stockReturns, message);
            mock.Setup(d => d.GetStockReturnsBySymbolOverDateRangeAsync(stockSymbol, null, null)).ReturnsAsync(stockReturnsWithMessage) ;//.ReturnsAsync(stockReturnsWithMessage);
            var controller = new ReturnsController(mock.Object, configurationVariables);

            // Act
            var result = (StockReturn) ((ObjectResult) controller.Get(stockSymbol).Result.Result).Value;

            // Assert
            Assert.Equal(stockSymbol, result.Stock.StockSymbol);
            Assert.True(result.Returns.Count > 0);
        }
    }
}