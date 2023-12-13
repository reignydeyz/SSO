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
            builder.Property(x => x.LastSessionId).HasMaxLength(200).HasDefaultValue("35c7c988-7c48-4f13-bf41-4edbd060a394");
            builder.Property(x => x.CreatedBy).HasMaxLength(200).HasDefaultValue("admin");
            builder.Property(x => x.ModifiedBy).HasMaxLength(200).HasDefaultValue("admin");
            builder.Property(x => x.DateCreated).HasDefaultValueSql("getdate()");
            builder.Property(x => x.DateModified).HasDefaultValueSql("getdate()");

            builder.HasIndex(x => new { x.FirstName, x.LastName }).IsUnique();
        }
    }
}
