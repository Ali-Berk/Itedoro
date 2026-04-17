using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Itedoro.Domain.Entities.PomodoroSessions;

namespace Itedoro.Persistence.Configurations;

public class ParentSessionConfiguration : IEntityTypeConfiguration<ParentSession>
{
    public void Configure(EntityTypeBuilder<ParentSession> builder)
    {
        builder.ToTable("ParentSessions");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Status)
            .HasConversion<string>()
            .IsRequired()
            .HasMaxLength(50);
        builder.Property(x => x.Note)
            .HasMaxLength(1000)
            .IsRequired(false);
        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasQueryFilter(x => !x.User.IsDeleted);
        builder.HasMany(x => x.ChildSessions)
               .WithOne(c => c.ParentSession)
               .HasForeignKey(c => c.ParentSessionId);
        builder.HasIndex(x => new { x.UserId, x.StartTime })
            .IsDescending(false, true);
        builder.Navigation(x => x.ChildSessions)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}