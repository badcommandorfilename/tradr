using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace mvcapp.Controllers
{
    [Route("")]
    [Route("/trade")]
    public class TradeController : Controller
    {
        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {
            return View(PortfolioAPIController.CurrentPortfolio());
        }
    }
}
