using Itedoro.Domain.Entities.WeeklyPlans;
using Itedoro.Persistence.Repositories.Repository;
using Itedoro.Application.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Itedoro.Persistence.Repositories.WeeklyPlan;

public class WeeklyPlanRepository(ItedoroDbContext context) : Repository<PlanItem>(context), IWeeklyPlanRepository
{
    public async Task<PlanItem?> GetByIdAsync(Guid id, Guid userId)
    {
        return await Context.PlanItems
            .Where(p => p.UserId == userId && p.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<(List<PlanItem> Plans, bool HasMoreData)> GetPlansBetweenDatesAsync(Guid userId, DateTime startDate, DateTime endDate, int page, int pageSize)
    {
        var query = Context.PlanItems
            .AsNoTracking()
            .Where(p => p.UserId == userId && p.EndDate >= startDate && p.StartDate <= endDate);
        var totalCount = await query.CountAsync();
        var hasMoreData = totalCount > (page * pageSize);
        
        var items = await Context.PlanItems
            .AsNoTracking()
            .Where(p => p.UserId == userId && p.EndDate >= startDate && p.StartDate <= endDate)
            .OrderBy(p => p.StartDate)
            .Skip((page - 1)*pageSize)
            .Take(pageSize)
            .ToListAsync();
        
        return (items, hasMoreData);
    }

    public async Task AddRangeAsync(IEnumerable<PlanItem> planItems)
    {
        await Context.PlanItems.AddRangeAsync(planItems);
    }

    public async Task<List<PlanItem>> GetOverduePlansAsync(Guid userId, DateTime referenceDate, CancellationToken cancellationToken)
    {
        return await Context.PlanItems
            .AsNoTracking()
            .Where(p => p.UserId == userId && p.EndDate <= referenceDate && !p.IsCompleted)
            .OrderBy(p => p.EndDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<PlanItem>> GetUpcomingPlansAsync(Guid userId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken)
    {
        return await Context.PlanItems
            .AsNoTracking()
            .Where(p => p.UserId == userId && p.StartDate >= startDate && p.EndDate <= endDate && !p.IsCompleted)
            .OrderBy(p => p.StartDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> IsAuthorized(Guid userId, Guid planId)
    {
        return await Context.PlanItems
            .Where(p => p.Id == planId && p.UserId == userId).AnyAsync();
    }
    
}
