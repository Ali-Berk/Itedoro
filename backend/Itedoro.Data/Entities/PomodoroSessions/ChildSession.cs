using System.Text.Json.Serialization;
using Itedoro.Data.Entities.PomodoroSessions.Enums;

namespace Itedoro.Data.Entities.PomodoroSessions;

public class ChildSession
{
    public Guid Id { get; init; }
    public Guid ParentSessionId { get; init; }

    public PomodoroType Type { get; init; }
    public PomodoroStatus Status { get; private set; }

    public int Order { get; init; }
    public int PlannedDurationMinutes { get; init; }

    [JsonIgnore] public virtual ParentSession ParentSession { get; init; } = null!;

    public ChildSession(
        Guid parentSessionId,
        int plannedDurationMinutes,
        PomodoroType type,
        int order,
        PomodoroStatus status = PomodoroStatus.Running,
        Guid id = default)
    {
        Id = id == Guid.Empty ? Guid.NewGuid() : id;

        ParentSessionId = parentSessionId;
        PlannedDurationMinutes = plannedDurationMinutes;
        Type = type;
        Order = order;
        Status = status;
    }

    public void Pause()
    {
        if (Status is (PomodoroStatus.Complated or PomodoroStatus.Canceled or PomodoroStatus.Paused))
            return;

        Status = PomodoroStatus.Paused;
    }

    public void Resume()
    {
        if (Status != PomodoroStatus.Paused) 
            return;
        
        Status = PomodoroStatus.Running;
    }
    public void Stop()
    {
        if (Status is (PomodoroStatus.Canceled or PomodoroStatus.Complated))
            return;
        
        Status = PomodoroStatus.Canceled;
    }

    public void SkipBreak()
    {
        if (Status is PomodoroStatus.Complated or PomodoroStatus.Canceled)
            return;
        Status = PomodoroStatus.Complated;
    }
}