using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SSO.Domain.Models;
using SSO.Infrastructure.Enums;

namespace SSO.Infrastructure.Configs
{
    public class ApplicationConfig : IEntityTypeConfiguration<Application>
    {
        readonly DatabaseType _dbType;

        public ApplicationConfig(DatabaseType? dbType = DatabaseType.SqlServer)
        {
            _dbType = dbType.Value;
        }

        public void Configure(EntityTypeBuilder<Application> builder)
        {
            builder.HasKey(x => x.ApplicationId);

            if (_dbType == DatabaseType.SqlServer)
            {
                builder.Property(x => x.ApplicationId)
                    .HasDefaultValueSql("NEWID()");
            }

            builder.Property(x => x.Name).HasMaxLength(200);
            builder.Property(x => x.CreatedBy).HasMaxLength(200);
            builder.Property(x => x.ModifiedBy).HasMaxLength(200);

            if (_dbType == DatabaseType.MySql)
            {
                builder.Property(x => x.DateCreated).HasColumnType("datetime").HasDefaultValueSql("CURRENT_TIMESTAMP");
                builder.Property(x => x.DateModified).HasColumnType("datetime").HasDefaultValueSql("CURRENT_TIMESTAMP");
            }

            builder.HasIndex(x => x.Name).IsUnique();
        }
    }
}
