using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SSO.Domain.Models;

namespace SSO.Infrastructure.Configs
{
    public class ApplicationPermissionConfig : IEntityTypeConfiguration<ApplicationPermission>
    {
        public void Configure(EntityTypeBuilder<ApplicationPermission> builder)
        {
            builder.HasIndex(x => new { x.ApplicationId, x.Permission }).IsUnique();

            builder.Property(x => x.Permission).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(200);

            builder.HasOne(x => x.Application).WithMany(x => x.Permissions)
                .HasForeignKey(d => d.ApplicationId);
        }
    }
}
