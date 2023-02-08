using AuthServer.Database.Models;
using AuthServer.Database.Repositories;
using AuthServer.Models;
using AuthServer.Utils;
using Azure.Core;
using JWTManager;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.Net.Http.Headers;
using System.Net.Http.Headers;
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

        [HttpGet]
        public async Task<IActionResult> GetToken([FromQuery] string? authorizationCode, [FromServices] IJwtManager jwtManager, [FromServices] UserRepository userRepository)
        {
            //check the username and password
            User? dbUser = await userRepository.GetUserByCode(authorizationCode);

            if(dbUser == null)
            {
                throw new NullReferenceException("The user does not exists!");
            }

            //generate token
            string token = jwtManager.GenerateJwt(dbUser.Username, DateTime.Now.AddMinutes(20));

            // set authorization header
            HttpContext.Response.Headers.Authorization = new StringValues(new[] { "Bearer", token });

            return new OkResult();
        }

        [HttpGet("Login")]
        public IActionResult Login([FromQuery] string? redirectUrl, [FromServices] IJwtManager jwtManager) => View(null, redirectUrl);

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginForm loginForm, [FromServices] IJwtManager jwtManager, [FromServices] UserRepository userRepository)
        {
            //Generate a random code
            string code = Guid.NewGuid().ToString();

            //Get the user with the related username from db.
            User? dbUser = await userRepository.GetUserAsync(loginForm.Username);

            if (dbUser == null)
            {
                throw new NullReferenceException("The user does not exists");
            }

            if (loginForm.Username == dbUser.Username && PasswordHandler.ValidatePassword(loginForm.PasswordHash, dbUser.Password, dbUser.Salt))
            {
                dbUser.Code = code;

                await userRepository.UpdateAsync(dbUser);

                return Redirect(loginForm.RedirectUrl + "?authorizationCode=" + code);
            }

            return new BadRequestResult();
        }
    }
}