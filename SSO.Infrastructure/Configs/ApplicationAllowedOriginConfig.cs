using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SSO.Domain.Models;

namespace SSO.Infrastructure.Configs
{
    public class ApplicationAllowedOriginConfig : IEntityTypeConfiguration<ApplicationAllowedOrigin>
    {
        public void Configure(EntityTypeBuilder<ApplicationAllowedOrigin> builder)
        {
            builder.HasKey(x => new { x.ApplicationId, x.Origin });

            builder.Property(x => x.Origin).HasMaxLength(200);
        }
    }
}
