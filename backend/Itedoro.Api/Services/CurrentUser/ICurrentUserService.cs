namespace Itedoro.Api.Services.CurrentUser;
public interface ICurrentUserService
{
    Guid? UserId { get; }
    bool IsAuthenticated { get; }
}