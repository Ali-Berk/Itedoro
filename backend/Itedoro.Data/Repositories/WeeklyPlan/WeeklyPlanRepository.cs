using Itedoro.Data.Entities.WeeklyPlan;
using Itedoro.Data.Repositories.WeeklyPlan.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Itedoro.Data.Repositories.WeeklyPlan;

public class WeeklyPlanRepository(ItedoroDbContext context) : IWeeklyPlanRepository
{
    //TODO: SAVE için Parent Classtan inherit edilecek ve fazlalık fonksiyonlar kaldırılacak. (Add Update Delete)
    public async Task<PlanItem?> GetByIdAsync(Guid id, Guid userId)
    {
        return await context.PlanItems
            .Where(p => p.UserId == userId && p.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task AddAsync(PlanItem planItem)
    {
        await context.PlanItems.AddAsync(planItem);
    }

    public void Update(PlanItem planItem)
    {
        context.PlanItems.Update(planItem);
    }

        public void Delete(PlanItem planItem)
        {
            context.PlanItems
                .Remove(planItem);
        }

    public async Task<List<PlanItem>> GetPlansBetweenDatesAsync(Guid userId, DateTime startDate, DateTime endDate)
    {
        return await context.PlanItems
            .AsNoTracking()
            .Where(p => p.UserId == userId && p.EndDate >= startDate && p.StartDate <= endDate)
            .OrderBy(p => p.StartDate)
            .ToListAsync();
    }

    public async Task AddRangeAsync(IEnumerable<PlanItem> planItems)
    {
        await context.PlanItems.AddRangeAsync(planItems);
    }

    public async Task<List<PlanItem>> GetOverduePlansAsync(Guid userId, DateTime referenceDate)
    {
        return await context.PlanItems
            .AsNoTracking()
            .Where(p => p.UserId == userId && p.EndDate <= referenceDate && !p.IsCompleted)
            .OrderBy(p => p.EndDate)
            .ToListAsync();
    }

    public async Task<List<PlanItem>> GetUpcomingPlansAsync(Guid userId, DateTime startDate, DateTime endDate)
    {
        return await context.PlanItems
            .AsNoTracking()
            .Where(p => p.UserId == userId && p.StartDate >= startDate && p.EndDate <= endDate)
            .OrderBy(p => p.StartDate)
            .ToListAsync();
    }
}