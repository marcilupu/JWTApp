using JWTManager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResourceServer.Filters;
using ResourceServer.Utils;
using System.Net.Http;

namespace ResourceServer.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [JwtAuthorization]
        public async Task<IActionResult> Privacy()
        {
            var token = HttpContext.Items["jwt"];

            // aici ar trebui o deserializare facuta la token ca sa fie afisat username-ul in view.

            return View();             
        }

    }
}