using Itedoro.Data.Entities.Users;
using Itedoro.Business.Shared.Result;

namespace Itedoro.Business.Services.AuthServices.TokenService;
public interface ITokenService
{
    string GenerateAccessToken(User user);


    (RefreshToken Entity, string rawToken) CreateRefreshToken(Guid userId);

    Task<Result<string>> RefreshAsync(string refreshToken);
}