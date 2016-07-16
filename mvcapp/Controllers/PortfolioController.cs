using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using mvcapp.Models;
using Microsoft.AspNet;
using Microsoft.AspNetCore.Http;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace mvcapp.Controllers
{
    [Route("portfolio")]
    public class PortfolioController : Controller
    {
        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {
            if(CurrentPortfolio.Shared == null)
            {
                CurrentPortfolio.Shared = new Portfolio("Shared", 1000);
            }
            return View(CurrentPortfolio.Shared);
        }

        [HttpPost]
        [Route("new")]
        public IActionResult Create(FormCollection input)
        {
            try
            {
                string name = input["name"];
                decimal balance = Decimal.Parse(input["balance"]);
                var portfolio = new Portfolio(name, balance);
                CurrentPortfolio.Shared = portfolio;
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
            
        }
    }
}
