using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Itedoro.Data.Entities.WeeklyPlan;

namespace Itedoro.Data.Configurations;
public class PlanItemConfiguration : IEntityTypeConfiguration<PlanItem>
{
    public void Configure(EntityTypeBuilder<PlanItem> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.ColorCode)
            .IsRequired()
            .HasMaxLength(10);
        
        builder.Property(x => x.Note)
            .HasMaxLength(2000);

        builder.HasIndex(x => new { x.UserId, x.StartDate, x.EndDate });
    }
}