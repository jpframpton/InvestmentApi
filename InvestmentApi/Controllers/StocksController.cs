using InvestmentShared.Interfaces;
using InvestmentShared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using InvestmentApi.Common;

//using System.Threading.Tasks;

namespace InvestmentApi.Controllers
{
    [Route("api/[controller]")]
    public class StocksController : ControllerBase
    {
        private readonly IStockData _stockData;

        public StocksController(IStockData stockData)
        {
            _stockData = stockData;
        }

        [HttpGet]
        public ActionResult<List<Stock>> Get()
        {
            try
            {
                var results = _stockData.GetAllStocks();
                return Ok(results);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{symbol}")]
        //public async Task<ActionResult<List<Stock>>> Get()
        public ActionResult<List<Stock>> Get(string symbol)
        {
            try
            {
                var results = _stockData.GetAllStocks();
                if (!Validation.CheckSymbolFound(symbol, results)) return NotFound(symbol);

                var result = results.FirstOrDefault(s => s.StockSymbol.Equals(symbol.ToUpperInvariant()));

                return Ok(result);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
