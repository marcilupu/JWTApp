using Microsoft.AspNetCore.Mvc;
using ResourceServer.Models;
using ResourceServer.Utils;
using System.Diagnostics;

namespace ResourceServer.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }
    }
}