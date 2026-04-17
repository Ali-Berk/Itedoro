using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Itedoro.Domain.Entities.PomodoroSessions;
using Itedoro.Domain.Enums;

namespace Itedoro.Persistence.Configurations;

public class ChildSessionConfiguration : IEntityTypeConfiguration<ChildSession>
{
    public void Configure(EntityTypeBuilder<ChildSession> builder)
    {
        builder.ToTable("ChildSessions");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Type)
            .HasConversion<string>()
            .IsRequired()
            .HasMaxLength(50);
        builder.Property(x => x.Order)
            .IsRequired();
        builder.Ignore(x => x.IsCompleted);
        builder.HasOne(x => x.ParentSession)
            .WithMany(p => p.ChildSessions)
            .HasForeignKey(x => x.ParentSessionId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasQueryFilter(x => !x.ParentSession.User.IsDeleted);
        builder.HasIndex(x => new { x.ParentSessionId, x.Order });
    }
}