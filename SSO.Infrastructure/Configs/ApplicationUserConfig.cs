using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SSO.Domain.Models;

namespace SSO.Infrastructure.Configs
{
    public class ApplicationUserConfig : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasIndex(x => x.Email).IsUnique();

            builder.Property(x => x.FirstName).HasMaxLength(200).HasDefaultValue("admin");
            builder.Property(x => x.LastName).HasMaxLength(200).HasDefaultValue("admin");

            builder.HasIndex(x => new { x.FirstName, x.LastName }).IsUnique();
        }
    }
}
