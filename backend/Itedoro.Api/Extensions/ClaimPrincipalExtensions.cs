using System.Security.Claims;

namespace Itedoro.Api.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static bool TryGetUserId(this ClaimsPrincipal user, out Guid userId)
    {
        return Guid.TryParse(user.FindFirstValue(ClaimTypes.NameIdentifier), out userId);
    }
}