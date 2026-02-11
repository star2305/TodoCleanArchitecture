using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace TodoCleanArchitecture.Application.Interfaces
{
    public interface ITokenService
    {
        string CreateAccessToken(IEnumerable<Claim> claims, DateTime expiresAtUtc);
    }
}
