using Microsoft.Extensions.Logging;
using Moq;
using TodoCleanArchitecture.Application.Dtos;
using TodoCleanArchitecture.Application.Interfaces;
using TodoCleanArchitecture.Application.UseCases;
using Xunit;

namespace TodoCleanArchitecture.Application.Tests
{
    public class LoginUseCaseTests
    {
        [Fact]
        public async Task ExecuteAsync_WhenAuthFails_ShouldReturnNull()
        {
            // Arrange
            var auth = new Mock<IUserAuthService>();
            var refresh = new Mock<IRefreshTokenService>();
            var tokens = new Mock<ITokenService>();
            var logger = new Mock<ILogger<LoginUseCase>>();
            var audit = new Mock<IAuditLogWriter>();

            auth.Setup(a => a.ValidateAsync("admin", "bad"))
                .ReturnsAsync((false, new System.Collections.Generic.List<System.Security.Claims.Claim>()));

            var useCase = new LoginUseCase(auth.Object, refresh.Object, tokens.Object, logger.Object, audit.Object);

            // Act
            var result = await useCase.ExecuteAsync(new LoginRequest { Username = "admin", Password = "bad" }, "127.0.0.1");

            // Assert
            Assert.Null(result);
            refresh.Verify(r => r.IssueTokensAsync(It.IsAny<string>(), It.IsAny<string>(), default), Times.Never);
        }

        [Fact]
        public async Task ExecuteAsync_WhenAuthSuccess_ShouldIssueTokens()
        {
            // Arrange
            var auth = new Mock<IUserAuthService>();
            var refresh = new Mock<IRefreshTokenService>();
            var tokens = new Mock<ITokenService>();
            var logger = new Mock<ILogger<LoginUseCase>>();
            var audit = new Mock<IAuditLogWriter>();

            auth.Setup(a => a.ValidateAsync("admin", "1234"))
                .ReturnsAsync((true, new System.Collections.Generic.List<System.Security.Claims.Claim>()));

            refresh.Setup(r => r.IssueTokensAsync("admin", "127.0.0.1", default))
                .ReturnsAsync(new TokenPairResult
                {
                    Access = new LoginResponse { AccessToken = "AT", ExpiresIn = 900, TokenType = "Bearer" },
                    RefreshTokenPlain = "RT",
                    RefreshExpiresAtUtc = System.DateTime.UtcNow.AddDays(14)
                });

            var useCase = new LoginUseCase(auth.Object, refresh.Object, tokens.Object, logger.Object, audit.Object);

            // Act
            var result = await useCase.ExecuteAsync(new LoginRequest { Username = "admin", Password = "1234" }, "127.0.0.1");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("AT", result!.Access.AccessToken);

            refresh.Verify(r => r.IssueTokensAsync("admin", "127.0.0.1", default), Times.Once);
        }
    }
}