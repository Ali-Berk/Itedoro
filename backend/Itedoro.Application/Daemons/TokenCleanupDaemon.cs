using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Itedoro.Application.Repositories;

namespace Itedoro.Application.Daemons;

public class TokenCleanupDaemon(
    IServiceProvider serviceProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);

            using (var scope = serviceProvider.CreateScope())
            {
                var repository = scope.ServiceProvider.GetRequiredService<IRefreshTokenRepository>();
                await repository.RemoveAllExpiredTokensAsync();
            }
        }
    }
}
