using Itedoro.Domain.Entities.Users;
using Itedoro.Domain.Enums;
using Itedoro.Domain.Exceptions;

namespace Itedoro.Domain.Entities.PomodoroSessions;

public class ParentSession
{
    private readonly List<ChildSession> _childSessions = [];

    public Guid Id { get; init; }
    public Guid UserId { get; private init; }
    public int TotalPlannedMinutes { get; private set; }
    public PomodoroStatus Status { get; private set; }
    public DateTime StartTime { get; private init; }
    public DateTime EndTime { get; private set; }
    public DateTime? PauseStart { get; private set; }
    public DateTime? PauseStop { get; private set; }
    public string? Note { get; private set; }
    public virtual IReadOnlyCollection<ChildSession> ChildSessions => _childSessions;
    public virtual User User { get; private init; } = null!;

    public ParentSession(
        Guid userId,
        int totalPlannedMinutes,
        string? note = null,
        Guid id = default,
        PomodoroStatus status = PomodoroStatus.Running,
        DateTime startTime = default,
        DateTime endTime = default,
        DateTime? pauseStart = null,
        DateTime? pauseStop = null)
    {
        Id = id == Guid.Empty ? Guid.NewGuid() : id;
        UserId = DomainException.ThrowIfEmpty(userId, nameof(userId));
        TotalPlannedMinutes = DomainException.ThrowIfNonPositive(totalPlannedMinutes, nameof(totalPlannedMinutes));
        Note = NormalizeOptional(note);
        Status = status;
        StartTime = startTime == default ? DateTime.UtcNow : startTime;
        EndTime = endTime == default ? StartTime.AddMinutes(totalPlannedMinutes) : endTime;
        PauseStart = pauseStart;
        PauseStop = pauseStop;
        DomainException.ThrowIf(EndTime < StartTime, "endTime cannot be earlier than startTime.");
    }

    public ChildSession AddChildSession(int plannedDurationMinutes, PomodoroType type, int order, Guid id = default)
    {
        DomainException.ThrowIf(_childSessions.Any(x => x.Order == order), $"A child session with order {order} already exists.");

        var child = new ChildSession(Id, plannedDurationMinutes, type, order, PomodoroStatus.Running, id);
        _childSessions.Add(child);
        return child;
    }

    public void UpdateNote(string? note)
    {
        Note = NormalizeOptional(note);
    }

    public void Pause()
    {
        if (Status != PomodoroStatus.Running)
        {
            return;
        }

        Status = PomodoroStatus.Paused;
        PauseStart = DateTime.UtcNow;

        foreach (var child in _childSessions)
        {
            child.Pause();
        }
    }

    public void Resume()
    {
        if (Status != PomodoroStatus.Paused)
        {
            return;
        }

        var resumedAt = DateTime.UtcNow;
        var pauseDuration = PauseStart is null ? TimeSpan.Zero : resumedAt - PauseStart.Value;

        Status = PomodoroStatus.Running;
        PauseStop = resumedAt;
        PauseStart = null;
        EndTime = EndTime.Add(pauseDuration);

        foreach (var child in _childSessions)
        {
            child.Resume();
        }
    }

    public void Stop()
    {
        if (Status is PomodoroStatus.Completed or PomodoroStatus.Canceled)
        {
            return;
        }

        Status = PomodoroStatus.Canceled;
        EndTime = DateTime.UtcNow;

        foreach (var child in _childSessions)
        {
            child.Stop();
        }
    }

    public void Complete()
    {
        if (Status is PomodoroStatus.Completed or PomodoroStatus.Canceled)
        {
            return;
        }

        Status = PomodoroStatus.Completed;
        EndTime = DateTime.UtcNow;
    }

    public void SkipBreak(Guid childId)
    {
        var child = _childSessions.FirstOrDefault(c => c.Id == childId);
        if (child is null || child.IsCompleted)
        {
            return;
        }

        child.SkipBreak();
        EndTime = EndTime.AddMinutes(-child.PlannedDurationMinutes);
    }

    private static string? NormalizeOptional(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }
}
