using System;
using System.Threading.Tasks;
using InvestmentApi.Common;
using InvestmentLogic;
using InvestmentShared.Interfaces;
using InvestmentShared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace InvestmentApi.Controllers
{
    [Route("api/stocks/{symbol}/alpha/{benchmark}")]
    public class AlphaController : ControllerBase
    {
        private readonly IStockReturns _stockReturn;
        private readonly ConfigurationVariables _configurationVariables;

        public AlphaController(IStockReturns stockReturn, IOptions<ConfigurationVariables> configurationVariables)
        {
            _stockReturn = stockReturn ?? new StockReturns(null);
            _configurationVariables = configurationVariables.Value;
        }

        [HttpGet]
        public async Task<ActionResult<StockReturn>> Get(string symbol, string benchmark, [FromQuery(Name = "startDate")] string startDate = "", [FromQuery(Name = "endDate")] string endDate = "")
        {
            try
            {
                var (startDateValue, endDateValue) = Validation.ValidateDates(startDate, endDate, _configurationVariables.MaxDaysInDateRange);
                var (alphaValue, message) = await _stockReturn.GetAlphaValueBySymbolOverDateRangeAsync(symbol, benchmark, startDateValue, endDateValue);

                if (string.IsNullOrEmpty(message)) return Ok(alphaValue);

                return message switch
                {
                    "Not Found" => NotFound(),
                    "Bad Request" => BadRequest(),
                    _ => Ok(alphaValue)
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
