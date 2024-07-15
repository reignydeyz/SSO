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

            builder.Property(x => x.RealmId).HasDefaultValue(new Guid("afb6d3ad-92a3-4ea3-aa90-88071a3ee8aa"));
            builder.Property(x => x.Name).HasMaxLength(200);
            builder.Property(x => x.CreatedBy).HasMaxLength(200);
            builder.Property(x => x.ModifiedBy).HasMaxLength(200);

            builder.HasIndex(x => new {x.RealmId, x.Name}).IsUnique();

            (_dbType switch
            {
                DatabaseType.MySql => (Action<EntityTypeBuilder<Application>>)UseMySql,
                DatabaseType.Postgres => UsePostgres,
                _ => UseSqlServer
            })(builder);
        }

        void UseSqlServer(EntityTypeBuilder<Application> builder)
        {
            builder.Property(x => x.ApplicationId).HasDefaultValueSql("NEWID()");
        }

        void UseMySql(EntityTypeBuilder<Application> builder)
        {
            builder.Property(x => x.DateCreated).HasColumnType("datetime").HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.Property(x => x.DateModified).HasColumnType("datetime").HasDefaultValueSql("CURRENT_TIMESTAMP");
        }

        void UsePostgres(EntityTypeBuilder<Application> builder)
        {
            builder.Property(x => x.DateCreated)
                .HasColumnType("timestamp with time zone")
                .HasConversion(x => x.UtcDateTime, x => new DateTimeOffset(x, TimeSpan.Zero))
                .HasDefaultValueSql("now() AT TIME ZONE 'UTC'");

            builder.Property(x => x.DateModified)
                .HasColumnType("timestamp with time zone")
                .HasConversion(x => x.UtcDateTime, x => new DateTimeOffset(x, TimeSpan.Zero))
                .HasDefaultValueSql("now() AT TIME ZONE 'UTC'");

            builder.Property(x => x.DateInactive)
                .HasColumnType("timestamp with time zone")
                .HasConversion(x => x.HasValue ? x.Value.UtcDateTime : (DateTime?)null, x => x.HasValue ? new DateTimeOffset(x.Value, TimeSpan.Zero) : (DateTimeOffset?)null);
        }
    }
}
