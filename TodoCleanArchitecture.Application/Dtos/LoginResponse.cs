using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoCleanArchitecture.Application.Dtos
{
    public class LoginResponse
    {
        public string AccessToken { get; set; } = "";
        public string TokenType { get; set; } = "Bearer";
        public long ExpiresIn { get; set; } // seconds

        public string RefreshToken { get; set; } = ""; // ✅ 추가
        public long RefreshExpiresIn { get; set; }     // seconds (선택)
    }
}
