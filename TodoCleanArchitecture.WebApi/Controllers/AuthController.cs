using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TodoCleanArchitecture.Application.Dtos;
using TodoCleanArchitecture.Application.UseCases;

namespace TodoCleanArchitecture.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly LoginUseCase _login;

        public AuthController(LoginUseCase login)
        {
            _login = login;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
        {
            var result = await _login.ExecuteAsync(request);
            if (result == null) return Unauthorized(new { message = "Invalid username or password" });

            return Ok(result);
        }
    }
}
