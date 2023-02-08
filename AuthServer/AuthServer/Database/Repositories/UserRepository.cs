using AuthServer.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthServer.Database.Repositories
{
    public class UserRepository
    {
        private AuthServerContext _context;
        public UserRepository(AuthServerContext context)
        {
            _context = context;
        }

        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            _context.SaveChanges();
        }

        public User GetUser(int id) {
            return _context.Users.FirstOrDefault(item => item.Id == id);
        }

        public async Task<bool> AnyAsync(string username) => await _context.Users.AnyAsync(user => user.Username == username);

        public List<User> GetAll() => _context.Users.Select(x => x).ToList();

        public async Task<User> GetUserAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(item => item.Username == username);
        }
    }
}
