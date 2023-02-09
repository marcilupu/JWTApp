using JWTManager;
using Microsoft.AspNetCore.Mvc;
using ResourceServer.Filters;

namespace ResourceServer.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (HttpContext.Request.Cookies["Jwt"] != null ? true : false)
            {
                return new RedirectToActionResult("Logout", null, null);
            }
            return View();
        }

        public IActionResult NoPrivacy([FromServices] IJwtManager jwtManager)
        {
            return View();
        }

        [JwtAuthorization]
        public IActionResult Privacy([FromServices] IJwtManager jwtManager)
        {
            // the token cannot be null if the JwtAuthorization has been performed
            string token = (string)HttpContext.Items["jwt"]!;

            var username = jwtManager.GetPayload(token).Username;
            
            return View("/Views/Home/Privacy.cshtml", username);             
        }

        [HttpGet]
        public IActionResult Logout()
        {
            if (HttpContext.Request.Cookies["Jwt"] != null)
            {
                HttpContext.Response.Cookies.Append("Jwt", "", new CookieOptions()
                {
                    Expires = DateTime.Now.AddDays(-1)
                });
            }
            return View("/Views/Home/Index.cshtml");
        }
    }
}