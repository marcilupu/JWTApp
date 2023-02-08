using AuthServer.Database.Repositories;
using AuthServer.Models;
using AuthServer.Utils;
using JWTManager;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using User = AuthServer.Database.Models.User;

namespace AuthServer.Controllers
{
    [Route("[controller]")]
    public class UserController : Controller
    {
        [HttpPost]
        public void AddUser([FromServices] UserRepository userRepository, string username, string password)
        {
            string salt = PasswordHandler.GenerateSalt();
            string passwordhash = PasswordHandler.ComputePassword(password, salt);

            User user = new User() { Username = username, Password = passwordhash, Salt = salt };

            userRepository.AddUser(user);
        }

        [HttpGet]
        public IActionResult GenerateToken([FromQuery] AuthServer.Models.User user, [FromServices] IJwtManager jwtManager, [FromServices] UserRepository userRepository)
        {
            //check the username and password
            User dbUser = userRepository.GetUser(user.Id);

            if (user.Username == dbUser.Username && PasswordHandler.ValidatePassword(user.Password, dbUser.Password, dbUser.Salt))
            {
                //generate token
                string token = jwtManager.GenerateJwt(user.Username, DateTime.Now.AddMinutes(20));

                return new JsonResult(token);
            }

            else return new BadRequestResult();
        }

        [HttpGet("Login")]
        public IActionResult Login([FromQuery] string? redirectUrl, [FromServices] IJwtManager jwtManager) => View(null, redirectUrl);

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginForm loginForm, [FromServices] IJwtManager jwtManager)
        {
            //validate user, password cu db
            //if valid generate jwt cu username ul respectiv, post, redirect
            string token = jwtManager.GenerateJwt(loginForm.Username, DateTime.Now.AddMinutes(20));

            HttpContext.Response.Cookies.Append("naughty-shawty-access-token", token, new CookieOptions { IsEssential = true, HttpOnly = true, SameSite = SameSiteMode.Strict});

            var html = $@"
             <html><head>
                <meta http-equiv='refresh' content='0;url={loginForm.RedirectUrl}' />
             </head></html>";

            return Content(html, "text/html");
            //if not valid warning in view credentiale incorecte
        }
    }
}