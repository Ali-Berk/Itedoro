using Itedoro.Application.Services.AuthServices.Dtos.Responses;
using Itedoro.Domain.Entities.Users;

namespace Itedoro.Application.Services.AuthServices.Mappers;

internal static class AuthMappingExtensions
{
    public static AuthResponse ToAuthResponse(
        this User user,
        string accessToken,
        string refreshToken,
        DateTime expiresAt)
        => new(accessToken, refreshToken, expiresAt, user.Id, user.Username);
}
