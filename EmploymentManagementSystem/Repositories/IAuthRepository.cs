using EmploymentManagementSystem.DTOS;

namespace EmploymentManagementSystem.Repositories
{
    public interface IAuthRepository
    {
        Task<bool> RegisterAsync(RegisterDTO registerDto);
        Task<string> LoginAsync(LoginDto loginDto);
        Task<bool> ForgotPasswordAsync(string email);
        Task<bool> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
        Task LogoutAsync();
    }
}
