using Itedoro.Domain.Entities.UserStats;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Itedoro.Persistence.Configurations;

public class UserWeekStatConfiguration : IEntityTypeConfiguration<UserWeekStat>
{
    public void Configure(EntityTypeBuilder<UserWeekStat> builder)
    {
        builder.ToTable("UserWeekStats");

        builder.HasKey(x => new { x.UserId, x.WeekId });

        builder.Property(x => x.UserId)
            .IsRequired();

        builder.Property(x => x.WeekId)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.CompletedPomodoros)
            .IsRequired();

        builder.Property(x => x.PlannedPomodoros)
            .IsRequired();

        builder.Property(x => x.CompletedPlans)
            .IsRequired();

        builder.Property(x => x.PlannedPlans)
            .IsRequired();

        builder.Property(x => x.WeeklyStudyTimeInMinutes)
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .IsRequired(false);

        builder.HasIndex(x => new { x.UserId, x.WeekId })
            .IsUnique();
    }
}