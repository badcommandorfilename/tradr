using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using mvcapp.Services;

namespace mvcapp.Controllers
{
    [Route("api/v0/prices")]
    public class APIController : Controller
    {
        QuoteService quotes = new QuoteService("ASX");
        [HttpGet]
        public IEnumerable<dynamic> Get()
        {
            var symbols = new[] { "PRU", "GFY" };
            foreach(var quote in symbols.Select(s => quotes.GetQuote(s)))
            {
                yield return quote;
            }
        }
    }
}
