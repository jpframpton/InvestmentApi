using System;
using InvestmentApi.Common;
using InvestmentApi.Controllers;
using InvestmentShared.Interfaces;
using InvestmentShared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace InvestmentApi.Test.UnitTests
{
    public class AlphaControllerShould
    {
        [Fact]
        public void ForGetAlphaValue_ReturnHttpNotFound_ForInvalidStock()
        {
            // Arrange
            const string stockSymbol = "Z1X";
            const string benchmarkSymbol = "MSFT";
            const string notFoundMessage = "Not Found";
            var mock = new Mock<IStockReturns>();
            var settings = new ConfigurationVariables()
            {
                MaxDaysInDateRange = 365
            };
            var configurationVariables = Options.Create(settings);

            decimal?  alphaValue = null;
            var alphaValueWithMessage = (alphaValue, notFoundMessage);
            mock.Setup(d => d.GetAlphaValueBySymbolOverDateRangeAsync(stockSymbol, benchmarkSymbol, It.IsAny<DateTime?>(), It.IsAny<DateTime?>())).ReturnsAsync(alphaValueWithMessage);

            var controller = new AlphaController(mock.Object, configurationVariables);

            // Act
            var result = controller.Get(stockSymbol, benchmarkSymbol).Result.Result;

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void ForGetAlphaValue_ReturnStockReturns_ForValidStock()
        {
            // Arrange
            const string stockSymbol = "AAPL";
            const string benchmarkSymbol = "MSFT";
            var message = string.Empty;
            var mock = new Mock<IStockReturns>();
            var settings = new ConfigurationVariables()
            {
                MaxDaysInDateRange = 365
            };
            var configurationVariables = Options.Create(settings);

            var stockReturns = JsonConvert.DeserializeObject<StockReturn>(TestConstants.StockReturns.GoodData);
            var stockReturnsWithMessage = (stockReturns, message);
            mock.Setup(d => d.GetStockReturnsBySymbolOverDateRangeAsync(stockSymbol, null, null)).ReturnsAsync(stockReturnsWithMessage);//.ReturnsAsync(stockReturnsWithMessage);
            mock.Setup(d => d.GetStockReturnsBySymbolOverDateRangeAsync(benchmarkSymbol, null, null)).ReturnsAsync(stockReturnsWithMessage);//.ReturnsAsync(stockReturnsWithMessage);
            var controller = new AlphaController(mock.Object, configurationVariables);

            // Act
            var result = Convert.ToDecimal(((ObjectResult)controller.Get(stockSymbol, benchmarkSymbol).Result.Result).Value);

            // Assert
            Assert.Equal(0, result);
        }
    }
}