namespace Itedoro.Data.Shared;

public record DatePagedResult<T>(
    List<T> Items,
    DateTime CurrentStartDate,
    DateTime CurrentEndDate,
    DateTime? NextStartDate)
{
    public bool HasNextPage => NextStartDate.HasValue;
}