using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Itedoro.Domain.Entities.PomodoroSessions;

namespace Itedoro.Persistence.Configurations;

public class ChildSessionConfiguration : IEntityTypeConfiguration<ChildSession>
{
    public void Configure(EntityTypeBuilder<ChildSession> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.HasOne(x => x.ParentSession)
               .WithMany(p => p.ChildSessions)
               .HasForeignKey(x => x.ParentSessionId);
    }
}