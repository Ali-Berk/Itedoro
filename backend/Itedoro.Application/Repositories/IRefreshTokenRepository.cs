using Itedoro.Application.Repositories;
namespace Itedoro.Application.Repositories;

public interface IRefreshTokenRepository : IRepository<Itedoro.Domain.Entities.RefreshTokens.RefreshToken>
{
    Task<Itedoro.Domain.Entities.RefreshTokens.RefreshToken?> GetByTokenAsync(string token);

    void Update(Itedoro.Domain.Entities.RefreshTokens.RefreshToken refreshToken);

    Task RemoveExpiredTokensAsync(Guid userId);

    Task RemoveAllExpiredTokensAsync();
}