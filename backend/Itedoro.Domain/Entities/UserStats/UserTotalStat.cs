namespace Itedoro.Domain.Entities.UserStats;

public class UserTotalStat
{
    public Guid UserId { get; private set; }

    public int TotalCompletedPomodoros { get; private set; }
    public int TotalStudyTimeInMinutes { get; private set; }

    public DateTime UpdatedAt { get; private set; }

    public UserTotalStat(
        int totalCompletedPomodoros,
        int totalStudyTimeInMinutes,
        Guid userId = default,
        DateTime updatedAt = default)
    {
        UserId = userId == Guid.Empty ? Guid.NewGuid() : userId;
        TotalCompletedPomodoros = totalCompletedPomodoros;
        TotalStudyTimeInMinutes = totalStudyTimeInMinutes;
        UpdatedAt = updatedAt == default ? DateTime.UtcNow : updatedAt;
    }
}
