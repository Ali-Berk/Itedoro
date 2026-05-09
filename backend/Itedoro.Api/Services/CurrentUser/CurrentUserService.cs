using System.Security.Claims;

namespace Itedoro.Api.Services.CurrentUser;

public class CurrentUserService(
    IHttpContextAccessor ctx) : ICurrentUserService
{
    public Guid? UserId {
        get {
            var id = ctx.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(id, out var g) ? g : (Guid?)null;
        }
    }
    public bool IsAuthenticated => ctx.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
}