namespace Itedoro.Business.Services.TokenService;

using Microsoft.EntityFrameworkCore;
using Itedoro.Business.Services.TokenService.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using Itedoro.Data;
using Itedoro.Data.Entities.Users;
using Itedoro.Business.Shared.Result;

public class TokenManager : ITokenService
{
    private readonly ItedoroDbContext context;
    private readonly IConfiguration config;
    private readonly SymmetricSecurityKey _key;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly int _accessTokenExpiresMinutes;
    private readonly int _refreshTokenExpiresDays;

    public TokenManager(ItedoroDbContext _context, IConfiguration _config)
    {
        context = _context;
        config = _config;
        
        var tokenKey = config.GetValue<string>("AppSettings:Token") 
                       ?? throw new Exception("Token key not found in settings");
            
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
        _issuer = config.GetValue<string>("AppSettings:Issuer") ?? "";
        _audience = config.GetValue<string>("AppSettings:Audience") ?? "";
        _accessTokenExpiresMinutes = config.GetValue<int>("AppSettings:ExpireMinutes");
        _refreshTokenExpiresDays = config.GetValue<int>("AppSettings:ExpireDays");
    }

    //DONE: getvalue'lar constructor içinde tanımlanacak.
    //WARN: Refresh Token ile sınırsız AccessToken üretiliyor.
    public string GenerateAccessToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role?.Name ?? "User"),
        };

        var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512);

        var jwtToken = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_accessTokenExpiresMinutes),
            signingCredentials: credentials
        );
        return new JwtSecurityTokenHandler().WriteToken(jwtToken);
    }

    public (RefreshToken Entity, string rawToken) CreateRefreshToken(Guid userId)
    {
        var expireDays = config.GetValue<int>("AppSettings:ExpireDays");
        
        string rawToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
        
        return
        (
            new RefreshToken(
                Sha256Hasher.ComputeHash(rawToken),
                userId,
                DateTime.UtcNow.AddDays(expireDays)),
                rawToken);
    }

    public async Task<Result<string>> RefreshAsync(string refreshToken)
    {
        var hashedToken = Sha256Hasher.ComputeHash(refreshToken);
        var exist = await context.RefreshTokens
            .Include(t => t.User).Include(t => t.User.Role)
            .FirstOrDefaultAsync(t => t.Token == hashedToken);

        if (exist == null || exist.IsExpired)
        {
            return Result<string>.Failure(errors: new[] { "Invalid Refresh Token." });
        }
        
        var accessToken = GenerateAccessToken(exist.User);
        return Result<string>.Success(accessToken);
    }
}