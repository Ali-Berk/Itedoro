using Itedoro.Domain.Entities.UserStats;

namespace Itedoro.Application.Repositories;

public interface IUserTotalStatRepository : IRepository<UserTotalStat>
{
    Task IncrementTotalAsync(Guid userId, int minutes);
    
    Task<UserTotalStat?> GetByUserIdAsync(Guid userId);
}