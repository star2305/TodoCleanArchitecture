using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoCleanArchitecture.Application.Dtos
{
    public class TokenPairResult
    {
        public LoginResponse Access { get; set; } = new();
        public string RefreshTokenPlain { get; set; } = "";
        public DateTime RefreshExpiresAtUtc { get; set; }
    }
}
