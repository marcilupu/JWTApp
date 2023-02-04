namespace AuthServer.Database.Repositories
{
    public class UserRepository
    {
        private AuthServerContext _context;
        public UserRepository(AuthServerContext context)
        {
            _context = context;
        }

    }
}
