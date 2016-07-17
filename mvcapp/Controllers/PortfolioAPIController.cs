
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using mvcapp.Services;
using mvcapp.Models;
using System.Collections.Generic;

namespace mvcapp.Controllers
{
    [Route("api/v0/portfolio")]
    public class PortfolioAPIController : Controller
    {
        [HttpGet]
        [Route("current")]
        public Portfolio Get()
        {
            return CurrentPortfolio();
        }

        [HttpGet]
        [Route("stocks")]
        public IEnumerable<Stock> Stocks()
        {
            return CurrentPortfolio().OwnedStocks;
        }

        [HttpGet]
        [Route("balance")]
        public decimal Balance()
        {
            return CurrentPortfolio().Balance;
        }

        [HttpGet]
        [Route("reset")]
        public Portfolio Reset()
        {
            Models.CurrentPortfolio.Shared = new Portfolio("Test", 1000);
            return Models.CurrentPortfolio.Shared;
        }



        public static Portfolio CurrentPortfolio()
        {
            if (Models.CurrentPortfolio.Shared == null)
            {
                Models.CurrentPortfolio.Shared = new Portfolio("Test", 1000);
            }
            return Models.CurrentPortfolio.Shared;
        }
    }
}
