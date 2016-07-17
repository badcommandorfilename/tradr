using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using mvcapp.Services;
using mvcapp.Models;
using System.IO;
using Newtonsoft.Json;
using System.Net.Http;

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
            var symbols = new[] { "ANZ", "BXB", "CBA", "BHP", "PRU", "GFY" };
            foreach(var quote in symbols.Select(s => quotes.GetQuote(s)))
            {
                yield return quote;
            }
        }

        [HttpPost]
        [Route("buy")]
        public dynamic Buy()
        {
            var req = Request.Body;
            string json = new StreamReader(req).ReadToEnd();

            dynamic input = JsonConvert.DeserializeObject(json);
            string symbol = input.symbol;
            var currentquote = quotes.GetQuote(symbol);
            uint quantity = (uint)input.quantity;
            return PortfolioAPIController.CurrentPortfolio().Buy(currentquote, quantity);

        }
    }
}
