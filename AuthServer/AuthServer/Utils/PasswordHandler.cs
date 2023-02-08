using System.Security.Cryptography;
using System.Text;

namespace AuthServer.Utils
{
    public static class PasswordHandler
    {
        public static string GenerateSalt()
        {
            using var randomNumber = RandomNumberGenerator.Create();
            var byteSalt = new byte[16];
            randomNumber.GetBytes(byteSalt);
            string salt = Convert.ToBase64String(byteSalt);
            return salt;
        }

        public static string ComputePassword(string password, string salt)
        {
            string passwordSalt = $"{password}{salt}";
            byte[] passwordBytes = Encoding.UTF8.GetBytes(passwordSalt);
            byte[] passwordHashBytes = SHA256.Create().ComputeHash(passwordBytes);
            string passwordHash = Convert.ToBase64String(passwordHashBytes);

            return passwordHash;
        }

        public static bool ValidatePassword(string password, string dbPassword, string salt)
        {
            string dbPasswordHash = ComputePassword(password, salt);

            if (dbPassword == dbPasswordHash) { return true; }

            return false;
        }
    }
}
