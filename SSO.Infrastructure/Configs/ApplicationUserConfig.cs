using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SSO.Domain.Models;
using SSO.Infrastructure.Enums;

namespace SSO.Infrastructure.Configs
{
    public class ApplicationUserConfig : IEntityTypeConfiguration<ApplicationUser>
    {
        readonly DatabaseType _dbType;

        public ApplicationUserConfig(DatabaseType? dbType = DatabaseType.SqlServer)
        {
            _dbType = dbType.Value;
        }

        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasIndex(x => x.Email).IsUnique();

            builder.Property(x => x.FirstName).HasMaxLength(200).HasDefaultValue("admin");
            builder.Property(x => x.LastName).HasMaxLength(200).HasDefaultValue("admin");
            builder.Property(x => x.LastSessionId).HasMaxLength(200).HasDefaultValue("35c7c988-7c48-4f13-bf41-4edbd060a394");
            builder.Property(x => x.CreatedBy).HasMaxLength(200).HasDefaultValue("admin");
            builder.Property(x => x.ModifiedBy).HasMaxLength(200).HasDefaultValue("admin");

            builder.HasIndex(x => new { x.FirstName, x.LastName }).IsUnique();

            (_dbType switch
            {
                DatabaseType.MySql => (Action<EntityTypeBuilder<ApplicationUser>>)UseMySql,
                DatabaseType.Postgres => UsePostgres,
                _ => UseSqlServer
            })(builder);
        }

        void UseSqlServer(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(x => x.DateCreated).HasDefaultValueSql("getdate()");
            builder.Property(x => x.DateModified).HasDefaultValueSql("getdate()");
        }

        void UseMySql(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(x => x.DateCreated).HasColumnType("datetime").HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.Property(x => x.DateModified).HasColumnType("datetime").HasDefaultValueSql("CURRENT_TIMESTAMP");
        }

        void UsePostgres(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(x => x.DateCreated).HasColumnType("timestamp without time zone").HasDefaultValueSql("now()");
            builder.Property(x => x.DateModified).HasColumnType("timestamp without time zone").HasDefaultValueSql("now()");
        }
    }
}
