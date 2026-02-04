using EmploymentManagementSystem.Models;

namespace EmploymentManagementSystem.Repositories
{
    public interface IUserRepository
    {
        
        Task<User?> GetByUserIdAsync(int userId);
        Task<IEnumerable<User>> GetAllUserAsync();
        Task<User> AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int userId);
    }
}
