using AuthServer.Database.Models;
using AuthServer.Database.Repositories;
using AuthServer.Utils;
using JWTManager;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;

namespace AuthServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        [HttpPost]
        public void AddUser([FromServices] UserRepository userRepository, string username, string password)
        {
            string salt = PasswordHandler.GenerateSalt();
            string passwordhash = PasswordHandler.ComputePassword(password, salt);

            User user = new User() { Username = username, Password = passwordhash, Salt = salt};

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
    }
}