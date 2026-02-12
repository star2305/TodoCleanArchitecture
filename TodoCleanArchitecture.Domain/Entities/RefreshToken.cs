using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoCleanArchitecture.Domain.Entities
{
    public class RefreshToken
    {
        public long Id { get; private set; }

        public int UserId { get; private set; }
        public string TokenHash { get; private set; } = "";  // 원문 X (해시)
        public DateTime ExpiresAtUtc { get; private set; }

        public bool IsRevoked { get; private set; }
        public DateTime? RevokedAtUtc { get; private set; }

        // Rotation용(옵션): 이전 토큰이 어떤 토큰으로 교체되었는지
        public long? ReplacedByTokenId { get; private set; }

        // 보안/감사
        public string? CreatedByIp { get; private set; }
        public string? RevokedByIp { get; private set; }
        public DateTime CreatedAtUtc { get; private set; }

        protected RefreshToken() { }

        public RefreshToken(int userId, string tokenHash, DateTime expiresAtUtc, string? createdByIp)
        {
            UserId = userId;
            TokenHash = tokenHash;
            ExpiresAtUtc = expiresAtUtc;
            CreatedByIp = createdByIp;
            CreatedAtUtc = DateTime.UtcNow;
        }

        public void Revoke(string? revokedByIp, long? replacedByTokenId = null)
        {
            IsRevoked = true;
            RevokedAtUtc = DateTime.UtcNow;
            RevokedByIp = revokedByIp;
            ReplacedByTokenId = replacedByTokenId;
        }
    }
}
