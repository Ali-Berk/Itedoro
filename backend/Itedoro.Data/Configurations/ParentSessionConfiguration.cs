using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Itedoro.Data.Entities.PomodoroSessions;
using Itedoro.Data.Entities.Users;

namespace Itedoro.Data.Configurations;

public class ParentSessionConfiguration : IEntityTypeConfiguration<ParentSession>
{
    public void Configure(EntityTypeBuilder<ParentSession> builder)
    {
        builder.HasKey(x => x.Id);

        // Foreign Key uyuşmazlığını kökten çözen o ilişki:
        builder.HasOne(x => x.User)
               .WithMany()
               .HasForeignKey(x => x.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        // Çocuklarla olan bağ
        builder.HasMany(x => x.ChildSessions)
               .WithOne(c => c.ParentSession)
               .HasForeignKey(c => c.ParentSessionId);
    }
}