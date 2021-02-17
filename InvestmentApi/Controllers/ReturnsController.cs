using InvestmentApi.Common;
using InvestmentShared.Interfaces;
using InvestmentShared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.Extensions.Options;

namespace InvestmentApi.Controllers
{
    [Route("api/stocks/{symbol}/returns")]
    public class ReturnsController : ControllerBase
    {
        private readonly IStockData _stockData;
        private readonly IStockReturnsData _stockReturnsData;
        private readonly ConfigurationVariables _configurationVariables;

        public ReturnsController(IStockData stockData, IStockReturnsData stockReturnsData, IOptions<ConfigurationVariables> configurationVariables)
        {
            _stockData = stockData;
            _stockReturnsData = stockReturnsData;
            _configurationVariables = configurationVariables.Value;
        }

        [HttpGet]
        //public async Task<ActionResult<StockReturn>> Get(string symbol, [FromQuery(Name = "startDate")] string startDate = "")
        public ActionResult<StockReturn> Get(string symbol, [FromQuery(Name = "startDate")] string startDate = "", [FromQuery(Name = "endDate")] string endDate = "")
        {
            try
            {
                if (!Validation.CheckSymbolFound(symbol, _stockData)) return NotFound(symbol);
                var (startDateValue, endDateValue) = ValidateDates(startDate, endDate);
                var results = _stockReturnsData.GetStockReturnsBySymbolOverDateRange(symbol, startDateValue, endDateValue);
                
                return Ok(results);
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        private (DateTime StartDateValue, DateTime EndDateValue) ValidateDates(string startDate, string endDate)
        {
            var startDateValue = Validation.GetStartDateValue(startDate);
            var endDateValue = Validation.GetEndDateValue(endDate);
            Validation.CheckDateRange(startDateValue, endDateValue, _configurationVariables.MaxDaysInDateRange);
            return (startDateValue, endDateValue);
        }
    }
}
