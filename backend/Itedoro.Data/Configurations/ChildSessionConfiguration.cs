using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Itedoro.Data.Entities.PomodoroSessions;

namespace Itedoro.Data.Configurations;

public class ChildSessionConfiguration : IEntityTypeConfiguration<ChildSession>
{
    public void Configure(EntityTypeBuilder<ChildSession> builder)
    {
        builder.HasKey(x => x.Id);
        
        // Parent ile ilişkiyi mühürle
        builder.HasOne(x => x.ParentSession)
               .WithMany(p => p.ChildSessions)
               .HasForeignKey(x => x.ParentSessionId);
    }
}