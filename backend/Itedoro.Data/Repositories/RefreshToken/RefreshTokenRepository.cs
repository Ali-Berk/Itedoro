using Itedoro.Data.Repositories.RefreshToken.Interfaces;
using Itedoro.Data.Repositories.Repository;
using Microsoft.EntityFrameworkCore;

namespace Itedoro.Data.Repositories.RefreshToken;

public class RefreshTokenRepository(ItedoroDbContext context) : Repository<Itedoro.Data.Entities.Users.RefreshToken>(context),IRefreshTokenRepository
{

    public async Task<Itedoro.Data.Entities.Users.RefreshToken?> GetByTokenAsync(string token)
    {
        return await Context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == token);
    }

    public void Update(Itedoro.Data.Entities.Users.RefreshToken refreshToken)
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