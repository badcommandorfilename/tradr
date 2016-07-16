
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using mvcapp.Services;
using mvcapp.Models;

namespace mvcapp.Controllers
{
    [Route("api/v0/portfolio")]
    public class PortfolioAPIController : Controller
    {
        [HttpGet]
        [Route("current")]
        public Portfolio Get()
        {
            if(CurrentPortfolio.Shared == null)
            {
                CurrentPortfolio.Shared = new Portfolio("Test", 1000);
            }
            return CurrentPortfolio.Shared;
        }
    }
}
