using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Itedoro.Domain.Entities.Roles;

namespace Itedoro.Persistence.Configurations;
public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    private static readonly Guid AdminRoleId = Guid.Parse("A1B2C3D4-E5F6-4A5B-8C9D-0E1F2A3B4C5D");
    private static readonly Guid UserRoleId = Guid.Parse("F9E8D7C6-B5A4-4F3E-2D1C-0B9A8F7E6D5C");
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasData(
            new Role("Admin") { Id = AdminRoleId },
            new Role("User") { Id = UserRoleId }
        );
    }
}