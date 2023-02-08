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
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetUserByCode(string code)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Code == code);
        }

        public User? GetUser(int id) {
            return _context.Users.FirstOrDefault(item => item.Id == id);

        }
        public bool Any(string username) => _context.Users.Any(x => x.Username == username);

        public async Task<bool> AnyAsync(string username) => await _context.Users.AnyAsync(user => user.Username == username);

        public List<User> GetAll() => _context.Users.Select(x => x).ToList();

        public async Task<User?> GetUserAsync(string username)
        {
            return await _context.Users.AsTracking(QueryTrackingBehavior.TrackAll).FirstOrDefaultAsync(item => item.Username == username);
        }
    }
}
