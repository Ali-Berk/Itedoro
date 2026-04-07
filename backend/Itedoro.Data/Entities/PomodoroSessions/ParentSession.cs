using Itedoro.Data.Entities.Users;

namespace Itedoro.Data.Entities.PomodoroSessions;
public class ParentSession
{
    public Guid Id {get; set;}
    public Guid UserId {get; set;}

    public int TotalPlannedMinutes {get; set;}

    public PomodoroStatus Status { get; set; }

    public DateTime StartTime { get; init;}
    public DateTime EndTime {get; set;}
    public DateTime? PauseStart { get; set; }
    public DateTime? PauseStop { get; set; }
    
    public string? Note {get; set;}

    public virtual ICollection<ChildSession> ChildSessions {get; set;} = new List<ChildSession>();
    public virtual User User {get; set;} = null!;

    protected ParentSession() {}

    public ParentSession(Guid userId, int totalPlannedMinutes, string? note = "")
    {
        Id = Guid.NewGuid();
        UserId = userId;
        TotalPlannedMinutes = totalPlannedMinutes;
        Note = note;
        Status = PomodoroStatus.Running;
        StartTime = DateTime.UtcNow;
        EndTime = StartTime.AddMinutes(totalPlannedMinutes);
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
