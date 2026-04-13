using Itedoro.Data.Repositories.Repository.Interfaces;
namespace Itedoro.Data.Repositories.RefreshToken.Interfaces;

public interface IRefreshTokenRepository : IRepository<Itedoro.Data.Entities.Users.RefreshToken>
{
    Task<Itedoro.Data.Entities.Users.RefreshToken?> GetByTokenAsync(string token);
    void Update(Itedoro.Data.Entities.Users.RefreshToken refreshToken);
    Task RemoveExpiredTokensAsync(Guid userId);
    Task RemoveAllExpiredTokensAsync();
}