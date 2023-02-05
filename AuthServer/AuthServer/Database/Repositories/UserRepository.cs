using AuthServer.Database.Models;

namespace AuthServer.Database.Repositories
{
    public class UserRepository
    {
        private AuthServerContext _context;
        public UserRepository(AuthServerContext context)
        {
            _context = context;
        }

        public void AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public User GetUser(int id) {
            return _context.Users.FirstOrDefault(item => item.Id == id);
        }
    }
}
