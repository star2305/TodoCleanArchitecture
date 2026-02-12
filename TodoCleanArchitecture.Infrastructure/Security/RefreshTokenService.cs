using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TodoCleanArchitecture.Application.Dtos;
using TodoCleanArchitecture.Application.Interfaces;
using TodoCleanArchitecture.Domain.Entities;
using TodoCleanArchitecture.Infrastructure.Persistence;

namespace TodoCleanArchitecture.Infrastructure.Security
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly TodoDbContext _db;
        private readonly ITokenService _jwt;

        // 운영에서 보통 Access 10~15분, Refresh 7~30일
        private static readonly TimeSpan AccessLifetime = TimeSpan.FromMinutes(15);
        private static readonly TimeSpan RefreshLifetime = TimeSpan.FromDays(14);

        public RefreshTokenService(TodoDbContext db, ITokenService jwt)
        {
            _db = db;
            _jwt = jwt;
        }

        public async Task<TokenPairResult?> IssueTokensAsync(string username, string ip, CancellationToken ct = default)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Username == username, ct);
            if (user == null) return null;

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, (string)user.Username),
                new Claim(ClaimTypes.Role, (string)user.Role)
            };

            var accessExpiresAt = DateTime.UtcNow.Add(AccessLifetime);
            var accessToken = _jwt.CreateAccessToken(claims, accessExpiresAt);

            // refresh 생성 + DB 저장(해시)
            var refreshTokenPlain = RefreshTokenGenerator.GenerateToken();
            var refreshHash = RefreshTokenGenerator.HashToken(refreshTokenPlain);
            var refreshExpiresAt = DateTime.UtcNow.Add(RefreshLifetime);

            _db.RefreshTokens.Add(new RefreshToken(
                userId: user.Id,
                tokenHash: refreshHash,
                expiresAtUtc: refreshExpiresAt,
                createdByIp: ip));

            await _db.SaveChangesAsync(ct);

            return new TokenPairResult
            {
                Access = new LoginResponse
                {
                    AccessToken = accessToken,
                    ExpiresIn = (long)AccessLifetime.TotalSeconds,
                    TokenType = "Bearer"
                },
                RefreshTokenPlain = refreshTokenPlain,
                RefreshExpiresAtUtc = refreshExpiresAt
            };
        }

        public async Task<TokenPairResult?> RefreshAsync(string refreshToken, string ip, CancellationToken ct = default)
        {
            var hash = RefreshTokenGenerator.HashToken(refreshToken);

            var existing = await _db.RefreshTokens
                .FirstOrDefaultAsync(x => x.TokenHash == hash, ct);

            if (existing == null) return null;
            if (existing.IsRevoked) return null;
            if (existing.ExpiresAtUtc < DateTime.UtcNow) return null;

            // 사용자 조회
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == existing.UserId, ct);
            if (user == null) return null;

            // ✅ Rotation: 새 refresh 발급 + 기존 revoke
            var newPlain = RefreshTokenGenerator.GenerateToken();
            var newHash = RefreshTokenGenerator.HashToken(newPlain);
            var newExpiresAt = DateTime.UtcNow.Add(RefreshLifetime);

            var newEntity = new RefreshToken(
                userId: user.Id,
                tokenHash: newHash,
                expiresAtUtc: newExpiresAt,
                createdByIp: ip);

            _db.RefreshTokens.Add(newEntity);
            await _db.SaveChangesAsync(ct); // newEntity.Id 확보

            existing.Revoke(revokedByIp: ip, replacedByTokenId: newEntity.Id);
            await _db.SaveChangesAsync(ct);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, (string)user.Username),
                new Claim(ClaimTypes.Role, (string)user.Role)
            };

            var accessExpiresAt = DateTime.UtcNow.Add(AccessLifetime);
            var accessToken = _jwt.CreateAccessToken(claims, accessExpiresAt);

            return new TokenPairResult
            {
                Access = new LoginResponse
                {
                    AccessToken = accessToken,
                    ExpiresIn = (long)AccessLifetime.TotalSeconds,
                    TokenType = "Bearer"
                },
                RefreshTokenPlain = newPlain,
                RefreshExpiresAtUtc = newExpiresAt
            };
        }

        public async Task<bool> RevokeAsync(string refreshToken, string ip, CancellationToken ct = default)
        {
            var hash = RefreshTokenGenerator.HashToken(refreshToken);

            var existing = await _db.RefreshTokens
                .FirstOrDefaultAsync(x => x.TokenHash == hash, ct);

            if (existing == null) return false;
            if (existing.IsRevoked) return true;

            existing.Revoke(ip);
            await _db.SaveChangesAsync(ct);
            return true;
        }
    }
}
