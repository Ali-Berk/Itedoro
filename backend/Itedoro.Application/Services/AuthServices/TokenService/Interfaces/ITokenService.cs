using Itedoro.Application.Common.Shared.Results;
using Itedoro.Domain.Entities.RefreshTokens;
using Itedoro.Domain.Entities.Users;

namespace Itedoro.Application.Services.AuthServices.TokenService.Interfaces;

public interface ITokenService
{
    string GenerateAccessToken(User user);

    (RefreshToken Entity, string rawToken) CreateRefreshToken(Guid userId);

    Task<Result<string>> RefreshAsync(string refreshToken);
}
