using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace SSO.Entities.Configs
{
    public class ApplicationConfig : IEntityTypeConfiguration<Application>
    {
        public void Configure(EntityTypeBuilder<Application> builder)
        {
            builder.HasKey(x => x.ApplicationId);
            builder.Property(x => x.ApplicationId).HasDefaultValueSql("NEWID()");

            builder.Property(x => x.Name).HasMaxLength(200);
            builder.Property(x => x.RedirectUrl).HasMaxLength(500);
            builder.Property(x => x.CreatedBy).HasMaxLength(200);
            builder.Property(x => x.ModifiedBy).HasMaxLength(200);

            builder.HasIndex(x => x.Name).IsUnique();
        }
    }
}
