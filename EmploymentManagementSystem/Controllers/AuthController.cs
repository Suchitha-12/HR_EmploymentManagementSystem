using EmploymentManagementSystem.DTOS;
using EmploymentManagementSystem.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmploymentManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IAuthRepository _authRepository;

        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { message = "Invalid registration data", errors = ModelState });
                }

                var result = await _authRepository.RegisterAsync(registerDto);

                if (!result)
                {
                    return BadRequest(new { message = "User with this email already exists" });
                }

                return Ok(new { message = "User registered successfully", username = registerDto.Username });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error during registration", error = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { message = "Invalid login data", errors = ModelState });
                }

                var username = await _authRepository.LoginAsync(loginDto);

                if (username == null)
                {
                    return Unauthorized(new { message = "Invalid email or password" });
                }

                return Ok(new { message = "Login successful", username = username });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error during login", error = ex.Message });
            }
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { message = "Invalid request", errors = ModelState });
                }

                var result = await _authRepository.ForgotPasswordAsync(forgotPasswordDto.Email);

                if (!result)
                {
                    return NotFound(new { message = "User with this email does not exist" });
                }

                return Ok(new { message = "Password reset instructions sent to your email" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error processing forgot password request", error = ex.Message });
            }
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { message = "Invalid request", errors = ModelState });
                }

                var result = await _authRepository.ResetPasswordAsync(resetPasswordDto);

                if (!result)
                {
                    return BadRequest(new { message = "Password reset failed. Invalid email or token" });
                }

                return Ok(new { message = "Password reset successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error resetting password", error = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _authRepository.LogoutAsync();
                return Ok(new { message = "Logout successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error during logout", error = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("profile")]
        public IActionResult GetProfile()
        {
            try
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                var username = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;
                var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
                var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
                var firstName = User.FindFirst(System.Security.Claims.ClaimTypes.GivenName)?.Value;
                var lastName = User.FindFirst(System.Security.Claims.ClaimTypes.Surname)?.Value;

                return Ok(new
                {
                    userId = userId,
                    username = username,
                    email = email,
                    role = role,
                    firstName = firstName,
                    lastName = lastName
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving profile", error = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("check-auth")]
        public IActionResult CheckAuth()
        {
            return Ok(new { isAuthenticated = true, username = User.Identity?.Name });
        }
    }
}
