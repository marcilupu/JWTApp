using AuthServer.Database.Repositories;
using AuthServer.Models;
using AuthServer.Utils;
using JWTManager;
using Microsoft.AspNetCore.Mvc;
using User = AuthServer.Database.Models.User;

namespace AuthServer.Controllers
{
    [Route("[controller]")]
    public class UserController : Controller
    {
        [HttpPost]
        public async Task AddUser([FromServices] UserRepository userRepository, string username, string password)
        {
            string salt = PasswordHandler.GenerateSalt();
            string passwordhash = PasswordHandler.ComputePassword(password, salt);
            
            if (await userRepository.AnyAsync(username))
            {
                throw new Exception("A username with this name already exists");
            }

            User user = new User() { Username = username, Password = passwordhash, Salt = salt };

            await userRepository.AddUserAsync(user);
        }

        [HttpGet("Login")]
        public IActionResult Login([FromQuery] string? redirectUrl, [FromServices] IJwtManager jwtManager) => View(null, redirectUrl);

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginForm loginForm, [FromServices] IJwtManager jwtManager, [FromServices] UserRepository userRepository)
        {
            var dbUser = await userRepository.GetUserAsync(loginForm.Username);

            if (dbUser != null && PasswordHandler.ValidatePassword(loginForm.Password, dbUser.Password, dbUser.Salt))
            {
                string token = jwtManager.GenerateJwt(loginForm.Username, DateTime.Now.AddMinutes(20));

                HttpContext.Response.Cookies.Append("naughty-shawty-access-token", token, new CookieOptions { IsEssential = true, HttpOnly = true, SameSite = SameSiteMode.Strict });

                var html = $@"
                        <html><head>
                            <meta http-equiv='refresh' content='0;url={loginForm.RedirectUrl}'/>
                        </head></html>";

                return Content(html, "text/html");
            }
            else
            {
                return View(); // eventual cu avertismente
            }
        }
    }
}