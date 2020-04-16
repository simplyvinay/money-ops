using MoneyOps.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MoneyOps.Infrastructure.Data.Config
{
    public class UserRoleConfig : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(
            EntityTypeBuilder<UserRole> builder)
        {
            builder
                .ToTable("UserRole")
                .HasKey(r => new {r.UserId, r.RoleId});
        }
    }
}