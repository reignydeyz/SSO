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

            builder.HasIndex(x => x.Name).IsUnique();

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
            builder.Property(x => x.DateCreated).HasColumnType("timestamp without time zone").HasDefaultValueSql("now()");
            builder.Property(x => x.DateModified).HasColumnType("timestamp without time zone").HasDefaultValueSql("now()");
        }
    }
}
