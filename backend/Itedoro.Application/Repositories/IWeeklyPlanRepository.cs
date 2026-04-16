using Itedoro.Domain.Entities.WeeklyPlans;

namespace Itedoro.Application.Repositories;

public interface IWeeklyPlanRepository : IRepository<PlanItem>
{
    Task<PlanItem?> GetByIdAsync(Guid id, Guid userId);
    Task<(List<PlanItem> Plans, bool HasMoreData)> GetPlansBetweenDatesAsync(Guid userId, DateTime startDate, DateTime endDate, int page, int pageSize);
    Task AddRangeAsync(IEnumerable<PlanItem> planItems);
    Task<List<PlanItem>> GetOverduePlansAsync(Guid userId, DateTime referenceDate, CancellationToken cancellationToken);
    Task<List<PlanItem>> GetUpcomingPlansAsync(Guid userId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken);
    Task<bool> IsAuthorized(Guid userId, Guid planId);
}