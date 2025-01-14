using Logic.IService;
using Microsoft.AspNetCore.Mvc;
using ShopAPI.Requests;

namespace ShopAPI.Controllers
{
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (loginRequest == null ||
            string.IsNullOrEmpty(loginRequest.Username) || loginRequest.Username.Length < 3 ||
            string.IsNullOrEmpty(loginRequest.Password) || loginRequest.Password.Length < 6)
            {
                return BadRequest(new { Message = "Username must be at least 3 characters and password must be at least 6 characters." });
            }

            try
            {
                var token = await _authService.GenerateJwtTokenAsync(loginRequest.Username, loginRequest.Password);
                return Ok(new { Token = token });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An unexpected error occurred while processing your request.", Details = ex.Message });
            }
        }
    }
}
