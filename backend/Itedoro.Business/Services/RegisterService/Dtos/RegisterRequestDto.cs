namespace Itedoro.Business.Services.RegisterService.Dtos;

public record RegisterRequestDto(
    string Email,
    string Password,
    string? Name,
    string? Surname,
    string Username
);