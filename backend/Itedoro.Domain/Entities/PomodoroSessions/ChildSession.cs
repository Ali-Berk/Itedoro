using Itedoro.Domain.Enums;
using Itedoro.Domain.Exceptions;

namespace Itedoro.Domain.Entities.PomodoroSessions;

public class ChildSession
{
    public Guid Id { get; init; }
    public Guid ParentSessionId { get; private init; }
    public PomodoroType Type { get; private init; }
    public PomodoroStatus Status { get; private set; }
    public int Order { get; private init; }
    public int PlannedDurationMinutes { get; private init; }
    public virtual ParentSession ParentSession { get; private init; } = null!;

    public ChildSession(
        Guid parentSessionId,
        int plannedDurationMinutes,
        PomodoroType type,
        int order,
        PomodoroStatus status = PomodoroStatus.Running,
        Guid id = default)
    {
        Id = id == Guid.Empty ? Guid.NewGuid() : id;
        ParentSessionId = DomainException.ThrowIfEmpty(parentSessionId, nameof(parentSessionId));
        PlannedDurationMinutes = DomainException.ThrowIfNonPositive(plannedDurationMinutes, nameof(plannedDurationMinutes));
        DomainException.ThrowIf(order <= 0, "order must be greater than zero.");
        Type = type;
        Order = order;
        Status = status;
    }
    public bool IsCompleted => Status is PomodoroStatus.Completed;

    public void Pause()
    {
        if (Status is PomodoroStatus.Completed or PomodoroStatus.Canceled or PomodoroStatus.Paused)
        {
            return;
        }

        Status = PomodoroStatus.Paused;
    }

    public void Resume()
    {
        if (Status != PomodoroStatus.Paused)
        {
            return;
        }

        Status = PomodoroStatus.Running;
    }

    public void Stop()
    {
        if (Status is PomodoroStatus.Canceled or PomodoroStatus.Completed)
        {
            return;
        }

        Status = PomodoroStatus.Canceled;
    }

    public void Complete()
    {
        if (Status is PomodoroStatus.Canceled or PomodoroStatus.Completed)
        {
            return;
        }

        Status = PomodoroStatus.Completed;
    }

    public void SkipBreak()
    {
        DomainException.ThrowIf(Type == PomodoroType.Work, "Only break sessions can be skipped.");
        Complete();
    }
}
