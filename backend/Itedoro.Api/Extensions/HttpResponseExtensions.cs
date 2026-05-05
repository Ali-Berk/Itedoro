namespace Itedoro.Api.Extensions;

public static class HttpResponseExtensions
{

    public static void SetRefreshTokenCookie(
        this HttpResponse response,
        string value,
        IConfiguration configuration)
    {
        if (!double.TryParse(configuration["JwtSettings:RefreshTokenExpireDays"], out var expireDays))
        {
            expireDays = 7;
        }

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax,
            Expires = DateTime.UtcNow.AddDays(expireDays)
        };
        
        response.Cookies.Append("refreshToken", value, cookieOptions);
    }

    public static void DeleteRefreshTokenCookie(this HttpResponse response)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax
        };
        response.Cookies.Delete("refreshToken", cookieOptions);
    }
}