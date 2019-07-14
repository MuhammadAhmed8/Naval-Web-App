using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication6.Controllers
{
    public class HomeController : Controller
    {
        [Route("/contact")]
        public IActionResult Contact()
        {
            return View();
        }
        [Route("/about")]
        public IActionResult About()
        {
            return View();
        }

    }
}