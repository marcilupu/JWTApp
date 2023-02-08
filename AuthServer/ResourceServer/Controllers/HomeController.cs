using Microsoft.AspNetCore.Mvc;
using ResourceServer.Models;
using System.Diagnostics;

namespace ResourceServer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Privacy()
        {

            string authHeader = HttpContext.Request.Query["authorizationCode"];



            HttpClient client = new HttpClient();
            var response = await client.GetAsync("serverautorizare/gettoken");


            string token = response.Headers.GetValues("Authorization").ToString();

            if(authHeader != null)
            {
                return View();
            }
            
            //if(jwt) and jwt is valid
            //return Privacy
            //if is not valid return BadRequest
            //else return Redirect()
            return Redirect("https://localhost:7186/User/login?redirectUrl=https://localhost:7109/Home/Privacy");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}