namespace Itedoro.Business.Services.RegisterService.Dtos;

public record RegisterRequestDto(
    string Email,
    string Password,
    Guid RoleId,
    string? Name,
    string? Surname,
    string Username
);