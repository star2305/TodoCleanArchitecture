using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TodoCleanArchitecture.Application.Dtos;
using TodoCleanArchitecture.Application.Interfaces;
using TodoCleanArchitecture.Application.UseCases;

namespace TodoCleanArchitecture.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly LoginUseCase _login;
        private readonly IRefreshTokenService _refresh;

        public AuthController(LoginUseCase login, IRefreshTokenService refresh)
        {
            _login = login;
            _refresh = refresh;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var result = await _login.ExecuteAsync(request, ip);
            if (result == null) return Unauthorized(new { message = "Invalid username or password" });

            return Ok(result);
        }

        public class RefreshRequest
        {
            public string RefreshToken { get; set; } = "";
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<LoginResponse>> Refresh([FromBody] RefreshRequest request)
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var result = await _refresh.RefreshAsync(request.RefreshToken, ip);
            if (result == null) return Unauthorized(new { message = "Invalid refresh token" });

            return Ok(result);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] RefreshRequest request)
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var ok = await _refresh.RevokeAsync(request.RefreshToken, ip);
            if (!ok) return NotFound(new { message = "Refresh token not found" });

            return NoContent();
        }
    }
}
