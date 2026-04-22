using Itedoro.Domain.Entities.Users;
namespace Itedoro.Domain.Entities.UserStats;

public class UserWeekStat
{
    public Guid UserId { get; private set; }
    public string WeekId { get; private set; }

    public int CompletedPomodoros { get; private set; }
    public int PlannedPomodoros { get; private set; }
    public int CompletedPlans { get; private set; }
    public int PlannedPlans { get; private set; }

    public int WeeklyStudyTimeInMinutes { get; private set;}

    public DateTime? UpdatedAt { get; private set; }

    public UserWeekStat(
        int completedPomodoros,
        int plannedPomodoros,
        int completedPlans,
        int plannedPlans,
        string weekId,
        Guid userId = default,
        DateTime? updatedAt = null
        )
    {
        UserId = userId == Guid.Empty ? Guid.NewGuid() : userId;
        WeekId = weekId;
        CompletedPomodoros = completedPomodoros;
        PlannedPomodoros = plannedPomodoros;
        CompletedPlans = completedPlans;
        PlannedPlans = plannedPlans;
        UpdatedAt = updatedAt == default ? DateTime.UtcNow : updatedAt;
    }
}