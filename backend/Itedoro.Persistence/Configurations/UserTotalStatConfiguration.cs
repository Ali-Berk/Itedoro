using Itedoro.Domain.Entities.UserStats;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Itedoro.Persistence.Configurations;

public class UserTotalStatConfiguration : IEntityTypeConfiguration<UserTotalStat>
{
    public void Configure(EntityTypeBuilder<UserTotalStat> builder)
    {
        builder.ToTable("UserTotalStats");

        builder.HasKey(x => x.UserId);

        builder.Property(x => x.UserId)
            .IsRequired();

        builder.Property(x => x.TotalCompletedPomodoros)
            .IsRequired();

        builder.Property(x => x.TotalStudyTimeInMinutes)
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .IsRequired();

        builder.HasIndex(x => x.UserId)
            .IsUnique();
    }
}