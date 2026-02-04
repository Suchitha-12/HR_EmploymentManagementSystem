using EmploymentManagementSystem.Data;
using EmploymentManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace EmploymentManagementSystem.Repositories
{
    public class UserRepository : IUserRepository
    {

        private readonly EmployeeDBContext _context;

        public UserRepository(EmployeeDBContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByUserIdAsync(int userId)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task<IEnumerable<User>> GetAllUserAsync()
        {
            return await _context.Users
                .OrderBy(u => u.Username)
                .ToListAsync();
        }

        public async Task<User> AddUserAsync(User user)
        {
            user.CreatedAt = DateTime.UtcNow;
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

    }
}
