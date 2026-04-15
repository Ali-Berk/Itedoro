using Microsoft.EntityFrameworkCore;
using Itedoro.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Itedoro.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> entity)
        {
            entity.Property(u => u.Username)
            .HasMaxLength(25);

            entity.Property(u => u.Name)
            .HasMaxLength(25);

            entity.Property(u => u.Email)
            .HasMaxLength(255)
            .IsRequired();

            entity.HasIndex(u => u.Email)
            .IsUnique();

            entity.HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId);

            entity.Property(u => u.Surname)
            .HasMaxLength(25);

            entity.Property(u => u.PasswordHash)
            .IsRequired();

        }
    }
}