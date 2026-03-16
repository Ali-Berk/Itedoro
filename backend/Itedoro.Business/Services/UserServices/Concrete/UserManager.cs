using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Itedoro.Data;
using Itedoro.Data.Entities.Users;
using Microsoft.EntityFrameworkCore.Query;


namespace Itedoro.Business.Services.UserServices
{
    public class UserManager : IUserService
    {
        private readonly ItedoroDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserManager(ItedoroDbContext context, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }
        public async Task<IdentityResult> CreateAsync(User user, string password)
        {
            var isUserExist = await _context.Users.AnyAsync(u => u.Email == user.Email || u.Username == user.Username);
            if (isUserExist)
            {
                return IdentityResult.Failed(new IdentityError 
                { 
                    Code = "DuplicateUser", 
                    Description = "That username or email is already in use." 
                });
            }
            user.PasswordHash = _passwordHasher.HashPassword(user ,password);

            await _context.Users.AddAsync(user);

            var result = await _context.SaveChangesAsync();

            return result > 0 
                ? IdentityResult.Success 
                : IdentityResult.Failed(new IdentityError { Description = "User could'nt be created." });
            
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.AsNoTracking().Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _context.Users.AsNoTracking().Include(u => u.Role).FirstOrDefaultAsync(u => u.Username == username);
        }

        public bool VerifyPassword(User user, string password)
        {
            var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            switch (verificationResult)
            {
                case PasswordVerificationResult.Success:            
                return true;
                case PasswordVerificationResult.SuccessRehashNeeded:            
                // hash yenilenecek.
                return true;
                case PasswordVerificationResult.Failed:
                default:
                return false;
            }
        }
    
        public async Task<bool> saveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> CreateRefreshTokenAsync(RefreshToken newToken)
        {
            if (!await RevokeRefreshTokenAsync(newToken.UserId)){return false;}
            await _context.RefreshTokens.AddAsync(newToken);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> RevokeRefreshTokenAsync(Guid userId){
            try
            {
                var userTokens = await _context.RefreshTokens.Where(t => t.UserId == userId).ToListAsync();
                if (userTokens.Any())
                {
                    _context.RefreshTokens.RemoveRange(userTokens);
                    await _context.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}