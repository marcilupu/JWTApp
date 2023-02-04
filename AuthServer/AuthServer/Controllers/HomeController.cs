using AuthServer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AuthServer.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

    }
}