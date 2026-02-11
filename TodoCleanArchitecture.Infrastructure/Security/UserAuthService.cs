using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TodoCleanArchitecture.Application.Interfaces;
using TodoCleanArchitecture.Infrastructure.Persistence;

namespace TodoCleanArchitecture.Infrastructure.Security
{
    public class UserAuthService : IUserAuthService
    {
        private readonly TodoDbContext _db;
        private readonly ILogger<UserAuthService> _logger;

        public UserAuthService(TodoDbContext db, ILogger<UserAuthService> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<(bool Success, List<Claim> Claims)> ValidateAsync(string username, string password)
        {
            _logger.LogInformation("Login attempt for user: {Username}", username);

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return (false, new List<Claim>());

            var user = await _db.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Username == username);

            if (user == null)
            {
                _logger.LogWarning("Login failed: user not found ({Username})", username);
                return (false, new List<Claim>());
            }

            var ok = PasswordHasher.Verify(password, user.PasswordHash, user.PasswordSalt);

            if (!ok)
            {
                _logger.LogWarning("Login failed: invalid password for user ({Username})", username);
                return (false, new List<Claim>());
            }

            _logger.LogInformation("Login successful for user: {Username}", username);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            return (true, claims);
        }
    }
}
