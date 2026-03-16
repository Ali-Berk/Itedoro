using Itedoro.Data.Entities.Users;

namespace Itedoro.Business.Services.TokenService;
public interface ITokenService
{
    string GenereteAccessToken(User user);

    string GenereteRefreshToken();

    Task<string> AsyncGenerateAndSaveRefreshToken(User user);
}