using Itedoro.Application.Common.Shared.Results;
namespace Itedoro.Application.Common.Errors;

public static class CommonErrors
{
    public static Error NotFound => new("NOT_FOUND", "The requested resource was not found.");
    public static Error UpdateRequestEmpty => new("UPDATE_REQUEST_EMPTY", "The update request must contain at least one field to update.");
}
