using InvestmentApi.Common;
using InvestmentShared.Interfaces;
using InvestmentShared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using InvestmentLogic;
using Microsoft.Extensions.Options;

namespace InvestmentApi.Controllers
{
    [Route("api/stocks/{symbol}/returns")]
    public class ReturnsController : ControllerBase
    {
        private readonly IStockReturns _stockReturn;
        private readonly ConfigurationVariables _configurationVariables;

        public ReturnsController(IStockReturns stockReturn, IOptions<ConfigurationVariables> configurationVariables)
        {
            _stockReturn = stockReturn ?? new StockReturns(null);
            _configurationVariables = configurationVariables.Value;
        }

        [HttpGet]
        public async Task<ActionResult<StockReturn>> Get(string symbol, [FromQuery(Name = "startDate")] string startDate = "", [FromQuery(Name = "endDate")] string endDate = "")
        {
            try
            {
                var (startDateValue, endDateValue) = Validation.ValidateDates(startDate, endDate, _configurationVariables.MaxDaysInDateRange);
                var (stockReturn, message) = await _stockReturn.GetStockReturnsBySymbolOverDateRangeAsync(symbol, startDateValue, endDateValue);

                if (string.IsNullOrEmpty(message)) return Ok(stockReturn);

                return message switch
                {
                    "Not Found" => NotFound(),
                    "Bad Request" => BadRequest(),
                    _ => Ok(stockReturn)
                };
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
    }
}
