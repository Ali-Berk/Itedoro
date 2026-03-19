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

    public ParentSession(out Guid Id ,Guid userId, int totalPlannedMinutes, string? note = "")
    {
        Id = Guid.NewGuid();
        this.Id = Id;
        UserId = userId;
        TotalPlannedMinutes = totalPlannedMinutes;
        Note = note;
        Status = PomodoroStatus.Running;
        StartTime = DateTime.UtcNow;
        EndTime = StartTime.AddMinutes(totalPlannedMinutes);

    }
}
