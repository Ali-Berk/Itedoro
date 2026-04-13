using Itedoro.Data.Entities.Users;
using Itedoro.Data.Entities.PomodoroSessions.Enums;

namespace Itedoro.Data.Entities.PomodoroSessions;
public class ParentSession
{
    public Guid Id {get; init;}
    public Guid UserId {get; init;}

    public int TotalPlannedMinutes {get; private set;}

    public PomodoroStatus Status { get; private set; }

    public DateTime StartTime { get; init;}
    public DateTime EndTime {get; private set;}
    public DateTime? PauseStart { get; private set; }
    public DateTime? PauseStop { get; private set; }
    
    public string? Note {get; private set;}

    public virtual ICollection<ChildSession> ChildSessions {get; init;} = new List<ChildSession>();
    public virtual User User {get; init;} = null!;

    public ParentSession(
        Guid userId,
        int totalPlannedMinutes,
        string? note = null,
        Guid id = default,
        PomodoroStatus status = PomodoroStatus.Running,
        DateTime startTime = default,
        DateTime endTime = default,
        DateTime? pauseStart = null,
        DateTime? pauseStop = null
    )
    {
        Id = id == Guid.Empty ? Guid.NewGuid() : id;

        UserId = userId;
        TotalPlannedMinutes = totalPlannedMinutes;
        Note = note;
        Status = status;

        StartTime = startTime == default ? DateTime.UtcNow : startTime;
        EndTime = endTime == default ? StartTime.AddMinutes(totalPlannedMinutes) : endTime;

        PauseStart = pauseStart;
        PauseStop = pauseStop;
        
    }

    public void Pause()
    {
        if (Status != PomodoroStatus.Running)
            return;
        
        Status = PomodoroStatus.Paused;
        PauseStart = DateTime.UtcNow;
        foreach (var child in ChildSessions)
        {
            child.Pause();
        }
    }

    public void Resume()
    {
        if (Status != PomodoroStatus.Paused)
            return;
            
        Status = PomodoroStatus.Running;
        PauseStop = DateTime.UtcNow;
        TimeSpan? diff = PauseStop - PauseStart;
        double diffMinutes = diff?.TotalMinutes ?? 0;
        EndTime = EndTime.AddMinutes(diffMinutes);

        foreach (var child in ChildSessions)
        {
            child.Resume();
        }
    }
    public void Stop()
    {
        if (Status is (PomodoroStatus.Complated or PomodoroStatus.Canceled))
            return;
        
        Status = PomodoroStatus.Canceled;
        EndTime = DateTime.UtcNow;
        foreach (var child in ChildSessions)
        {
            child.Stop();
        }
    }
    public void SkipBreak(Guid childId)
    {
        var child = ChildSessions.FirstOrDefault(c => c.Id == childId);
        if (child == null || child.Type == PomodoroType.Work || child.Status == PomodoroStatus.Complated)
            return;
        
        child.SkipBreak();
        EndTime = EndTime.AddMinutes(-child.PlannedDurationMinutes);
    }
}
