using AuthServer.Database.Repositories;
using AuthServer.Models;
using AuthServer.Utils;
using JWTManager;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
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

        [HttpGet("GetToken")]
        public async Task<IActionResult> GetToken([FromQuery] string authorizationCode, [FromServices] IJwtManager jwtManager, [FromServices] UserRepository userRepository)
        {
            //check the username and password
            User? dbUser = await userRepository.GetUserByCode(authorizationCode);

            if(dbUser == null)
            {
                return BadRequest();  
            }

            //generate token
            string token = jwtManager.GenerateJwt(dbUser.Username, DateTime.Now.AddDays(1));

            // set authorization header
            HttpContext.Response.Headers.Authorization = new StringValues(new[] { "Bearer", token });

            return Ok();
        }

        [HttpGet("Login")]
        public IActionResult Login([FromQuery] string redirectUrl, [FromServices] IJwtManager jwtManager) => View(null, redirectUrl);

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginForm loginForm, [FromServices] IJwtManager jwtManager, [FromServices] UserRepository userRepository)
        {
          
            //Get the user with the related username from db.
            User? dbUser = await userRepository.GetUserAsync(loginForm.Username);

            if (dbUser == null)
            {
                return BadRequest();
            }
            
            // Validate username and password
            if (loginForm.Username == dbUser.Username && PasswordHandler.ValidatePassword(loginForm.Password, dbUser.Password, dbUser.Salt))
            { 
                //Generate a random code
                string code = Guid.NewGuid().ToString();

                // update the user's code in db.
                dbUser.Code = code;

                if (await userRepository.UpdateAsync(dbUser))
                {
                    // redirect to the redirectUrl
                    return Redirect(loginForm.RedirectUrl + "?authorizationCode=" + code);
                }
            }

            return BadRequest();
        }
    }
}