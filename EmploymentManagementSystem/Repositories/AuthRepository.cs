using EmploymentManagementSystem.Data;
using EmploymentManagementSystem.DTOS;
using EmploymentManagementSystem.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EmploymentManagementSystem.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly EmployeeDBContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthRepository(EmployeeDBContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> RegisterAsync(RegisterDTO registerDto)
        {
            if (await _dbContext.Users.AnyAsync(u => u.Email == registerDto.Email))
                return false;

            var user = new User
            {
                Username = registerDto.Username,
                Email = registerDto.Email,
                PasswordHash = registerDto.Password, // In production, hash the password using BCrypt or PBKDF2
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Role = User.UserRoles.User,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<string> LoginAsync(LoginDto loginDto)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
            if (user == null || user.PasswordHash != loginDto.Password || !user.IsActive)
                return null;

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
            };

            await _httpContextAccessor.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            // Update last login time
            user.LastLoginAt = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync();

            return user.Username;
        }

        public async Task<bool> ForgotPasswordAsync(string email)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                return false;

            // Implement password reset logic (e.g., send email with reset token)
            // For now, returning true to indicate user exists
            return true;
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == resetPasswordDto.Email);
            if (user == null)
                return false;

            // In production, verify reset token before allowing password change
            user.PasswordHash = resetPasswordDto.NewPassword; // Hash the password in production
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task LogoutAsync()
        {
            await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}