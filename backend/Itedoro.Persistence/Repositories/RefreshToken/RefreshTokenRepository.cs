using Itedoro.Application.Repositories;
using Itedoro.Persistence.Repositories.Repository;
using Microsoft.EntityFrameworkCore;

namespace Itedoro.Persistence.Repositories.RefreshToken;

public class RefreshTokenRepository(ItedoroDbContext context) : Repository<Itedoro.Domain.Entities.RefreshTokens.RefreshToken>(context),IRefreshTokenRepository
{

    public async Task<Itedoro.Domain.Entities.RefreshTokens.RefreshToken?> GetByTokenAsync(string token)
    {
        return await Context.RefreshTokens
            .Include(rt => rt.User)
            .Include(rt => rt.User.Role)
            .FirstOrDefaultAsync(rt => rt.Token == token);
    }

    public void Update(Itedoro.Domain.Entities.RefreshTokens.RefreshToken refreshToken)
    {
        Context.RefreshTokens.Update(refreshToken);
    }

    public async Task RemoveExpiredTokensAsync(Guid userId)
    {
        var expiredTokens = await Context.RefreshTokens
            .Where(rt => rt.UserId == userId && rt.ExpiryTime < DateTime.UtcNow)
            .ToListAsync();

        if (expiredTokens.Any())
        {
            Context.RefreshTokens.RemoveRange(expiredTokens);
        }
    }
    
    async Task IRefreshTokenRepository.RemoveAllExpiredTokensAsync()
    {
        var expiredTokens = await Context.RefreshTokens
            .Where(rt => rt.ExpiryTime < DateTime.UtcNow)
            .ToListAsync();

        if (expiredTokens.Any())
        {
            Context.RefreshTokens.RemoveRange(expiredTokens);
        }
    }
}
