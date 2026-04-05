using Itedoro.Business.Services.WeeklyPlanService.Dtos.Responses;
using Itedoro.Data.Entities.WeeklyPlan;
using Itedoro.Data.Shared;

namespace Itedoro.Business.Services.WeeklyPlanService.Mappers;

public static class WeeklyPlanMappingExtensions
{
    public static GetAllPlansPagedBetweenDatesResponse GetAllPlansPagedBetweenDatesResponseMapper(this PlanItem weeklyPlan)
    {
        return new GetAllPlansPagedBetweenDatesResponse(
            Id: weeklyPlan.Id,
            Title:weeklyPlan.Title,
            StartDate: weeklyPlan.StartDate,
            EndDate: weeklyPlan.EndDate,
            ColorCode: weeklyPlan.ColorCode,
            Note: weeklyPlan.Note,
            IsCompleted: weeklyPlan.IsCompleted
            );
    }
    public static IEnumerable<GetOverduePlansResponse> GetOverduePlansResponseMapper(this IEnumerable<PlanItem> weeklyPlans)
    {
        return weeklyPlans.Select(weeklyPlan => new GetOverduePlansResponse(
            Id: weeklyPlan.Id,
            Title: weeklyPlan.Title,
            StartDate: weeklyPlan.StartDate,
            EndDate: weeklyPlan.EndDate,
            ColorCode: weeklyPlan.ColorCode,
            Note: weeklyPlan.Note
        ));
    }
}