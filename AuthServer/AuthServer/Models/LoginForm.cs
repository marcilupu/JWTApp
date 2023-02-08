namespace AuthServer.Models
{
    public class LoginForm
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string RedirectUrl { get; set; } = null!;
    }
}