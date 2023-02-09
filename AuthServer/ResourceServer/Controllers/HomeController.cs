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
            string username = "Ionut_test";

            // aici ar trebui o deserializare facuta la token ca sa fie afisat username-ul in view.

            return View("/Views/Home/Privacy.cshtml", username);             
        }

        [HttpGet]
        public IActionResult Logout()
        {
            if (HttpContext.Request.Cookies["Jwt"] != null)
            {
                //HttpContext.Response.Cookies["Jwt"].Expires = DateTime.Now.AddDays(-1);

                HttpContext.Response.Cookies.Append("Jwt", "", new CookieOptions()
                {
                    Expires = DateTime.Now.AddDays(-1)
                });
            }
            return View("/Views/Home/Index.cshtml");
        }
    }
}