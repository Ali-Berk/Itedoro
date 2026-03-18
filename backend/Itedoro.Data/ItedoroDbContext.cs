using Microsoft.EntityFrameworkCore;
using Itedoro.Data.Entities.Users;
using Itedoro.Data.Entities.Roles;
using Itedoro.Data.Entities.PomodoroSessions;

using System.Reflection.Metadata;
using System.Data.Common;


namespace Itedoro.Data;

public class ItedoroDbContext : DbContext
{
    public ItedoroDbContext(DbContextOptions<ItedoroDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set;} 
    public DbSet<Role> Roles { get; set;}

    public DbSet<RefreshToken> RefreshTokens { get; set;}
    public DbSet<ParentSession> ParentSessions { get; set;}
    public DbSet<ChildSession> ChildSessions { get; set;}


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ItedoroDbContext).Assembly);
    }
    
}
