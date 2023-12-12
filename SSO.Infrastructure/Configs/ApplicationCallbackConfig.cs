using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SSO.Domain.Models;

namespace SSO.Infrastructure.Configs
{
    public class ApplicationCallbackConfig : IEntityTypeConfiguration<ApplicationCallback>
    {
        public void Configure(EntityTypeBuilder<ApplicationCallback> builder)
        {
            builder.HasKey(x => new { x.ApplicationId, x.Url });

            builder.Property(x => x.Url).HasMaxLength(200).IsRequired();
        }
    }
}
