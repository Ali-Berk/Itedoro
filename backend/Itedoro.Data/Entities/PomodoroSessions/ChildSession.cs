using System.Text.Json.Serialization;

namespace Itedoro.Data.Entities.PomodoroSessions;

public class ChildSession
{
    public Guid Id {get; init;}
    public Guid ParentSessionId {get; init;}

    public PomodoroType Type {get; init;}
    public PomodoroStatus Status {get; set;}
    
    public int Order { get; init; }
    public int PlannedDurationMinutes {get; init;}

    [JsonIgnore]
    public virtual ParentSession ParentSession {get; set;} = null!;

    protected ChildSession(){}

    public ChildSession(Guid parentSessionId, int plannedDurationMinutes, PomodoroType type, int order)
    {
        Id = Guid.NewGuid();
        ParentSessionId = parentSessionId;
        Order = order;
        Type = type;
        Status = PomodoroStatus.Running;
        PlannedDurationMinutes = plannedDurationMinutes;
    }
}