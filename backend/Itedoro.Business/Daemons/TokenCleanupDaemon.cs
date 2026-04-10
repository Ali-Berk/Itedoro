using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Itedoro.Data;

namespace Itedoro.Business.Daemons;

public class TokenCleanupDaemon : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public TokenCleanupDaemon(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);

            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ItedoroDbContext>();

                var expiredTokens = dbContext.RefreshTokens
                    .Where(t => t.ExpiryTime <= DateTime.UtcNow);

                if (expiredTokens.Any())
                {
                    dbContext.RefreshTokens.RemoveRange(expiredTokens);
                    await dbContext.SaveChangesAsync(stoppingToken);
                }
            }
        }
    }
}