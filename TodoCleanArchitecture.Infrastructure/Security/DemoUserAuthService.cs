using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TodoCleanArchitecture.Application.Interfaces;

namespace TodoCleanArchitecture.Infrastructure.Security
{
    public class DemoUserAuthService : IUserAuthService
    {
        public Task<(bool Success, List<Claim> Claims)> ValidateAsync(string username, string password)
        {
            if (username == "admin" && password == "1234")
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, "User")
                };
                return Task.FromResult((true, claims));
            }

            return Task.FromResult((false, new List<Claim>()));
        }
    }
}
