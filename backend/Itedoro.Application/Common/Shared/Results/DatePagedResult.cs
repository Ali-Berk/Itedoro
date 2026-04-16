namespace Itedoro.Application.Common.Shared.Results;

public record DatePagedResult<T>(
    List<T> Items,
    DateTime CurrentStartDate,
    DateTime CurrentEndDate,
    DateTime? NextStartDate)
{
    public bool HasNextPage => NextStartDate.HasValue;
}