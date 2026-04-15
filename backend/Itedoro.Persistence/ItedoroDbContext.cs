using Microsoft.EntityFrameworkCore;
using Itedoro.Domain.Entities.RefreshTokens;
using Itedoro.Domain.Entities.Users;
using Itedoro.Domain.Entities.Roles;
using Itedoro.Domain.Entities.PomodoroSessions;
using Itedoro.Domain.Entities.WeeklyPlans;


namespace Itedoro.Persistence;

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

    public DbSet<PlanItem> PlanItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ItedoroDbContext).Assembly);
    }
    
}
