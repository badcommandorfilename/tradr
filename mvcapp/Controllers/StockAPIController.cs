using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using mvcapp.Services;
using mvcapp.Models;

namespace mvcapp.Controllers
{
    [Route("api/v0/stocks")]
    public class StockAPIController : Controller
    {
        QuoteService quotes = new QuoteService("ASX");
        [HttpGet]
        [Route("prices")]
        public IEnumerable<dynamic> Get()
        {
            var symbols = new[] { "PRU", "GFY" };
            foreach(var quote in symbols.Select(s => quotes.GetQuote(s)))
            {
                yield return quote;
            }
        }

        [HttpPost]
        [Route("buy")]
        public dynamic Buy(string symbol, uint quantity)
        {
            var currentquote = quotes.GetQuote(symbol);

            return CurrentPortfolio.Shared.Buy(currentquote, quantity);
        }
    }
}
