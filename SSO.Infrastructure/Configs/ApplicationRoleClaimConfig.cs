using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SSO.Domain.Models;

namespace SSO.Infrastructure.Configs
{
    public class ApplicationRoleClaimConfig : IEntityTypeConfiguration<ApplicationRoleClaim>
    {
        public void Configure(EntityTypeBuilder<ApplicationRoleClaim> builder)
        {
            builder.Property(x => x.ClaimType).HasMaxLength(100);
            builder.Property(x => x.ClaimValue).HasMaxLength(100);

            builder.HasOne(x => x.Permission)
                .WithMany()
                .HasForeignKey(x => x.PermissionId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
