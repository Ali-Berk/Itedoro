using Itedoro.Domain.Exceptions;
using Itedoro.Domain.Entities.Users;
namespace Itedoro.Domain.Entities.WeeklyPlans;

public class PlanItem
{
    public Guid Id { get; init; }
    public Guid UserId { get; private init; }
    public string Title { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public DateTime CreatedAt { get; private init; }
    public string ColorCode { get; private set; }
    public string? Note { get; private set; }
    public bool IsCompleted { get; private set; }
    public virtual User User { get; init; } = null!;

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
        UserId = DomainException.ThrowIfEmpty(userId, nameof(userId));
        Title = DomainException.ThrowIfNullOrWhiteSpace(title, nameof(title));
        SetSchedule(startDate, endDate);
        Note = NormalizeOptional(note);
        ColorCode = NormalizeColorCode(colorCode);
        IsCompleted = isCompleted;
        CreatedAt = createdAt == default ? DateTime.UtcNow : createdAt;
        UpdatedAt = updatedAt == default ? CreatedAt : updatedAt;
    }

    public void UpdatePlan(
        string? title,
        DateTime? startDate,
        DateTime? endDate,
        string? note,
        string? colorCode)
    {
        var nextStart = startDate ?? StartDate;
        var nextEnd = endDate ?? EndDate;

        Title = title is null ? Title : DomainException.ThrowIfNullOrWhiteSpace(title, nameof(title));
        SetSchedule(nextStart, nextEnd);
        ColorCode = colorCode is null ? ColorCode : NormalizeColorCode(colorCode);
        Note = note is null ? Note : NormalizeOptional(note);
        Touch();
    }

    public void MarkCompleted()
    {
        if (IsCompleted)
        {
            return;
        }

        IsCompleted = true;
        Touch();
    }

    public void MarkIncomplete()
    {
        if (!IsCompleted)
        {
            return;
        }

        IsCompleted = false;
        Touch();
    }

    public void UpdateStatus()
    {
        if (IsCompleted)
        {
            MarkIncomplete();
            return;
        }

        MarkCompleted();
    }

    private void SetSchedule(DateTime startDate, DateTime endDate)
    {
        DomainException.ThrowIf(endDate < startDate, "endDate cannot be earlier than startDate.");
        StartDate = startDate;
        EndDate = endDate;
    }

    private void Touch()
    {
        UpdatedAt = DateTime.UtcNow;
    }

    private static string NormalizeColorCode(string? colorCode)
    {
        var normalized = string.IsNullOrWhiteSpace(colorCode) ? "#327ec9" : colorCode.Trim();
        DomainException.ThrowIf(!normalized.StartsWith('#') || normalized.Length is not (4 or 7), "colorCode must be a valid hex color.");
        return normalized;
    }

    private static string? NormalizeOptional(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }
}
