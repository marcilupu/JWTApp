namespace JWTManager
{
    public interface IJwtManager
    {
        public string GenerateJwt(string username, DateTime expiresAt);
        public bool ValidateJwt(string token);
    }
}
