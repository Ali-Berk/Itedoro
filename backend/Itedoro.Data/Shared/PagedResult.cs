namespace Itedoro.Data.Shared;

public record PagedResult<T>(
    List<T> Items,
    int TotalCount,
    int CurrentPage,
    int PageSize)
{
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasNextPage => CurrentPage < TotalPages;
    public bool HasPreviousPage => CurrentPage > 1;
}