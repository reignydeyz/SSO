using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SSO.Domain.Models;

namespace SSO.Infrastructure.Configs
{
    public class ApplicationRoleConfig : IEntityTypeConfiguration<ApplicationRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationRole> builder)
        {
            builder.HasIndex(x => new { x.ApplicationId, x.Name }).IsUnique();

            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.NormalizedName).IsRequired();

            builder.HasIndex(x => new { x.Name }).IsUnique(false);
        }
    }
}
