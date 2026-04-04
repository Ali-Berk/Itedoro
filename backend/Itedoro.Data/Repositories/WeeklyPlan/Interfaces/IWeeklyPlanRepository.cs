using Itedoro.Data.Entities.WeeklyPlan;

namespace Itedoro.Data.Repositories.WeeklyPlan.Interfaces;

public interface IWeeklyPlanRepository
{
    Task<PlanItem?> GetByIdAsync(Guid id, Guid userId);
    Task AddAsync(PlanItem planItem);
    void Update(PlanItem planItem);
    void Delete(PlanItem planItem);

    Task<List<PlanItem>> GetPlansBetweenDatesAsync(Guid userId, DateTime startDate, DateTime endDate);

    Task AddRangeAsync(IEnumerable<PlanItem> planItems);

    Task<List<PlanItem>> GetOverduePlansAsync(Guid userId, DateTime referenceDate);
    Task<List<PlanItem>> GetUpcomingPlansAsync(Guid userId, DateTime startDate, DateTime endDate);
}