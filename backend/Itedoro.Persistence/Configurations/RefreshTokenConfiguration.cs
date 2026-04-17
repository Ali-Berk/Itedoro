using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Itedoro.Domain.Entities.RefreshTokens;

namespace Itedoro.Persistence.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshTokens");
        builder.HasKey(r => r.TokenId);
        builder.Property(r => r.Token)
            .IsRequired()
            .HasMaxLength(512);
        builder.HasIndex(r => r.Token)
            .IsUnique();
        builder.HasOne(r => r.User)
            .WithMany()
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasQueryFilter(x => !x.User.IsDeleted);
        builder.Property(r => r.CreatedAt)
            .IsRequired();
        builder.Property(r => r.ExpiryTime)
            .IsRequired();
    }
}