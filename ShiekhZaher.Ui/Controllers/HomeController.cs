using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Zaher.Ui.Controllers;
using Zaher.Ui.Models;

namespace Zaher.Ui.Controllers
{
  
  
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
     
        public ActionResult Index()
        {
            return RedirectToAction("Index", "FBooks");
        }

        public IActionResult Privacy()
        {
            return View();
        }

      
        public IActionResult MyCreed()
        {
            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
