namespace Itedoro.Application.Services.UserServices.Dtos.Requests;

public record UpdateMeRequest(
    string? Username,
    string? Name,
    string? Surname, 
    string? Email);