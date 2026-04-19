namespace Itedoro.Application.Services.UserServices.Dtos.Responses;

public record UpdateMeResponse(
    string Username,
    string? Name,
    string? Surname, 
    string Email);
    
