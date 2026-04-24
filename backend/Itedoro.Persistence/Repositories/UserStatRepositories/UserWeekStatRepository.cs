using Itedoro.Domain.Entities.UserStats;
using Microsoft.EntityFrameworkCore;
using Itedoro.Persistence.Repositories.Repository;
using Itedoro.Application.Repositories;


namespace Itedoro.Persistence.Repositories.UserStatRepositories;

public class UserWeekStatRepository(ItedoroDbContext context) : Repository<UserWeekStat>(context), IUserWeekStatRepository
{
    public Task IncrementWeekly(Guid userId, string weekId)
    {
        return Context.Database.ExecuteSqlInterpolatedAsync($@"
            INSERT INTO user_week_stats (user_id, week_id, completed_pomodoros, updated_at)
            VALUES ({userId}, {weekId}, 1, NOW())
            ON CONFLICT (user_id, week_id)
            DO UPDATE SET
                completed_pomodoros = user_week_stats.completed_pomodoros + 1,
                updated_at = NOW();
        ");
    }

    public async Task<UserWeekStat?> GetByUserAndWeekAsync(Guid userId, string weekId)
    {
        return await Context.WeekStats
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.UserId == userId && x.WeekId == weekId);
    }
}