using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoCleanArchitecture.Application.Dtos;
using TodoCleanArchitecture.Application.Interfaces;

namespace TodoCleanArchitecture.Application.UseCases
{
    public class LoginUseCase
    {
        private readonly IUserAuthService _auth;
        private readonly ITokenService _tokens;

        public LoginUseCase(IUserAuthService auth, ITokenService tokens)
        {
            _auth = auth;
            _tokens = tokens;
        }

        public async Task<LoginResponse?> ExecuteAsync(LoginRequest request)
        {
            var (success, claims) = await _auth.ValidateAsync(request.Username, request.Password);
            if (!success) return null;

            var expiresAt = DateTime.UtcNow.AddHours(2);
            var token = _tokens.CreateAccessToken(claims, expiresAt);

            return new LoginResponse
            {
                AccessToken = token,
                TokenType = "Bearer",
                ExpiresIn = (long)(expiresAt - DateTime.UtcNow).TotalSeconds
            };
        }
    }
}
