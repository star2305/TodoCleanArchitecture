using Microsoft.Extensions.Logging;
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
        private readonly IRefreshTokenService _refresh;
        private readonly ITokenService _tokens;
        private readonly ILogger<LoginUseCase> _logger;
        private readonly IAuditLogWriter _audit;

        public LoginUseCase(IUserAuthService auth, IRefreshTokenService refresh, ITokenService tokens, ILogger<LoginUseCase> logger, IAuditLogWriter audit)
        {
            _auth = auth;
            _refresh = refresh;
            _tokens = tokens;
            _logger = logger;
            _audit = audit;
        }

        public async Task<TokenPairResult?> ExecuteAsync(LoginRequest request, string ip)
        {
            var traceId = System.Diagnostics.Activity.Current?.Id;
            _logger.LogInformation("Executing LoginUseCase for {Username}", request.Username);
            await _audit.WriteAsync("Info", "Auth", "LoginAttempt", "Login attempt", request.Username, traceId);

            var (success, _) = await _auth.ValidateAsync(request.Username, request.Password);
            if (!success)
            {
                _logger.LogWarning("LoginUseCase failed for {Username}", request.Username);
                await _audit.WriteAsync("Warning", "Auth", "LoginFailed", "Invalid credentials", request.Username, traceId);
                return null;
            }

            //var expiresAt = DateTime.UtcNow.AddHours(2);
            //var token = _tokens.CreateAccessToken(claims, expiresAt);

            _logger.LogInformation("LoginUseCase succeeded for {Username}", request.Username);
            await _audit.WriteAsync("Info", "Auth", "LoginSuccess", "Login succeeded", request.Username, traceId);

            //return new LoginResponse
            //{
            //    AccessToken = token,
            //    TokenType = "Bearer",
            //    ExpiresIn = (long)(expiresAt - DateTime.UtcNow).TotalSeconds
            //};

            return await _refresh.IssueTokensAsync(request.Username, ip);
        }
    }
}
