using Itedoro.Application.Services.UserServices.Dtos.Responses;
using Itedoro.Domain.Entities.Users;
using Itedoro.Domain.Entities.UserStats;

namespace Itedoro.Application.Services.UserServices.Mappers;

public static class UserMappingExtensions
{
    //TODO: İstatistikler için 2. bir tablo oluşturulacak.
    public static GetMeResponse GetMeResponseMapper(
        this User user, 
        UserWeekStat weekStat, 
        UserTotalStat totalStat)
    {
        var pomodoroRate = weekStat.PlannedPomodoros == 0
            ? 0
            : (double)weekStat.CompletedPomodoros / weekStat.PlannedPomodoros * 100;

        var planRate = weekStat.PlannedPlans == 0
            ? 0
            : (double)weekStat.CompletedPlans / weekStat.PlannedPlans * 100;

        return new GetMeResponse(
            Id: user.Id,
            Username: user.Username,
            Name: user.Name,
            Surname: user.Surname,
            Email: user.Email,

            PomodoroCompletionRate: pomodoroRate,
            WeeklyCompletedPomodoros: weekStat.CompletedPomodoros,
            WeeklyPlanCount: weekStat.PlannedPlans,
            WeeklyPlanCompletionRate: planRate,

            WeeklyStudyInMinutes: weekStat.WeeklyStudyTimeInMinutes,
            TotalCompletedPomodoros: totalStat.TotalCompletedPomodoros,
            TotalStudyTimeInMinutes: totalStat.TotalStudyTimeInMinutes
        );
    }

    public static GetUserResponse GetUserResponseMapper(this User user)
    {
        return new GetUserResponse(
            UserId: user.Id,
            Username: user.Username,
            Name: user.Name,
            Surname: user.Surname,
            CreatedAt: user.CreatedAt,
            TotalCompletedPomodoros: 0,
            TotalCompletedWeeklyPlans:0,
            TotalStudyTimeInMinutes:0);
    }

    public static UpdateUserResponse UpdateUserResponseMapper(this User user)
    {
        return new UpdateUserResponse(
        Id: user.Id,
        Username: user.Username,
        Name: user.Name,
        Surname: user.Surname,
        Email: user.Email,
        UpdatedAt: user.UpdatedAt ?? DateTime.UtcNow);
    }

    public static UpdateMeResponse UpdateMeResponseMapper(this User user)
    {
        return new UpdateMeResponse(
            Username: user.Username,
            Name: user.Name,
            Surname: user.Surname,
            Email: user.Email);
    }
}