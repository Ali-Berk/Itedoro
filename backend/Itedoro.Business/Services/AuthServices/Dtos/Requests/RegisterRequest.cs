namespace Itedoro.Business.Services.AuthServices.Dtos.Requests;


public record RegisterRequest(
    string Email,
    string Username,
    string Password,
    string? Name,
    string? Surname
);