using Microsoft.AspNetCore.Mvc;
using ResourceServer.Utils;

namespace ResourceServer.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Privacy()
        {
            string authQueryCode = HttpContext.Request.Query["authorizationCode"];

            HttpClient client = new HttpClient();
            var response = await client.GetAsync("https://localhost:7186/user/gettoken?authorizationCode=" + authQueryCode);

            string token = response.Headers.GetValues("Authorization").ToString();

            if(token != null)
            {
                return View();
            }
            
            //if(jwt and jwt is valid)
            //return Privacy View
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