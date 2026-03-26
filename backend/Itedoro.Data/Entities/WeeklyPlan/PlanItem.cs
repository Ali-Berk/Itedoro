namespace Itedoro.Data.Entities.WeeklyPlan;

public class PlanItem
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    
    public string Title { get; private set; }
    
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public DateTime CreatedAt { get; init; }

    public string ColorCode { get; private set; }
    public string? Note { get; private set; }
    public bool IsCompleted { get; private set;}
    
    public PlanItem(
        Guid userId, 
        string title, 
        DateTime startDate, 
        DateTime endDate, 
        string? note = null, 
        string? colorCode = null,
        Guid id = default,
        DateTime createdAt = default,
        DateTime updatedAt = default,
        bool isCompleted = false)
    {
        Id = id == Guid.Empty ? Guid.NewGuid() : id;
        
        UserId = userId;
        Title = title;
        StartDate = startDate;
        EndDate = endDate;
        Note = note;
        ColorCode = colorCode ?? "#327ec9";
        IsCompleted = isCompleted;
        CreatedAt = createdAt == default ? DateTime.UtcNow : createdAt;
        UpdatedAt = updatedAt == default ? DateTime.UtcNow : updatedAt;
    }

    public void UpdateTitle(string title)
    {
        Title = title;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateColorCode(string colorCode)
    {
        ColorCode = colorCode;
        UpdatedAt = DateTime.UtcNow;
    }
    public void UpdateNote(string note)
    {
        Note = note;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateDateRange(DateTime startDate, DateTime endDate)
    {
        StartDate = startDate;
        EndDate = endDate;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateStatus()
    {
        IsCompleted = !IsCompleted;
        UpdatedAt = DateTime.UtcNow;
    }
}