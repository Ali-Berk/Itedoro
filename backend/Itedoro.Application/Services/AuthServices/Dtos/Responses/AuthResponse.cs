namespace Itedoro.Application.Services.AuthServices.Dtos.Responses;

public record AuthResponse(
    string AccessToken,
    string RefreshToken,
    DateTime ExpiresAt,
    Guid UserId,
    string UserName
    );