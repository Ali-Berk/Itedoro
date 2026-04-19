namespace Itedoro.Application.Services.UserServices.Dtos.Responses;

public record UpdateUserResponse(
    Guid Id,
    string Username,
    string? Name,
    string? Surname,
    string Email,
    DateTime UpdatedAt);