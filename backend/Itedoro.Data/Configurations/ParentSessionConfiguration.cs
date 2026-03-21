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

        builder.HasOne(x => x.User)
               .WithMany()
               .HasForeignKey(x => x.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.ChildSessions)
               .WithOne(c => c.ParentSession)
               .HasForeignKey(c => c.ParentSessionId);
    }
}