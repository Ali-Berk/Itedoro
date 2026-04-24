using Itedoro.Domain.Entities.UserStats;

namespace Itedoro.Application.Repositories;

public interface IUserWeekStatRepository : IRepository<UserWeekStat>
{
    Task IncrementWeekly(Guid userId, string weekId);

    Task<UserWeekStat?> GetByUserAndWeekAsync(Guid userId, string weekId);
}