namespace Itedoro.Business.Services.AuthServices.Dtos.Requests;

public record RefreshTokenRequest(
    string AccessToken,
    string RefreshToken
);