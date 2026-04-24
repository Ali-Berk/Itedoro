using Itedoro.Domain.Entities.UserStats;
using Microsoft.EntityFrameworkCore;
using Itedoro.Persistence.Repositories.Repository;
using Itedoro.Application.Repositories;

namespace Itedoro.Persistence.Repositories.UserStatRepositories;

public class UserTotalStatRepository(ItedoroDbContext context) : Repository<UserTotalStat>(context), IUserTotalStatRepository
{

    public Task IncrementTotalAsync(Guid userId, int minutes)
    {
        return Context.Database.ExecuteSqlInterpolatedAsync($@"
            INSERT INTO user_total_stats 
                (user_id, total_completed_pomodoros, total_study_time_in_minutes, updated_at)
            VALUES 
                ({userId}, 1, {minutes}, NOW())
            ON CONFLICT (user_id)
            DO UPDATE SET
                total_completed_pomodoros = user_total_stats.total_completed_pomodoros + 1,
                total_study_time_in_minutes = user_total_stats.total_study_time_in_minutes + {minutes},
                updated_at = NOW();
        ");
    }

    public async Task<UserTotalStat?> GetByUserIdAsync(Guid userId)
    {
        return await Context.TotalStats
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.UserId == userId);
    }
}