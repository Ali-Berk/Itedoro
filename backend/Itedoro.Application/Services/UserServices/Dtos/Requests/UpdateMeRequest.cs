namespace Itedoro.Application.Services.UserServices.Dtos.Requests;

public record UpdateMeRequest(
    string? Name,
    string? Surname);