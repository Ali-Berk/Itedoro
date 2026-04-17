using Itedoro.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Itedoro.Domain.Entities.WeeklyPlans;

namespace Itedoro.Persistence.Configurations;
public class PlanItemConfiguration : IEntityTypeConfiguration<PlanItem>
{
    public void Configure(EntityTypeBuilder<PlanItem> builder)
    {
        builder.ToTable("PlanItems");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(200);
        builder.Property(x => x.Note)
            .HasMaxLength(1000);
        builder.Property(x => x.ColorCode)
            .IsRequired()
            .HasMaxLength(7);
        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasQueryFilter(x => !x.User.IsDeleted);
        builder.HasIndex(x => new { x.UserId, x.StartDate, x.EndDate });
    }
}