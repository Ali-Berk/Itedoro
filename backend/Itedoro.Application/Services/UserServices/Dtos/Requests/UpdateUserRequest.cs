namespace Itedoro.Application.Services.UserServices.Dtos.Requests;

public record UpdateUserRequest(
    string? Name,
    string? Surname,
    string? Username);