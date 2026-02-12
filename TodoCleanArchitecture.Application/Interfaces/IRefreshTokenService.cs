using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoCleanArchitecture.Application.Dtos;

namespace TodoCleanArchitecture.Application.Interfaces
{
    public interface IRefreshTokenService
    {
        Task<TokenPairResult?> IssueTokensAsync(string username, string ip, CancellationToken ct = default);
        Task<TokenPairResult?> RefreshAsync(string refreshToken, string ip, CancellationToken ct = default);
        Task<bool> RevokeAsync(string refreshToken, string ip, CancellationToken ct = default);
    }
}
