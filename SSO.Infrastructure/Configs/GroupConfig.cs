using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SSO.Domain.Models;
using SSO.Infrastructure.Enums;

namespace SSO.Infrastructure.Configs
{
    public class GroupConfig : IEntityTypeConfiguration<Group>
    {
        readonly DatabaseType _dbType;

        public GroupConfig(DatabaseType? dbType = DatabaseType.SqlServer)
        {
            _dbType = dbType.Value;
        }

        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.HasKey(x => x.GroupId);

            builder.Property(x => x.RealmId).HasDefaultValue(new Guid("afb6d3ad-92a3-4ea3-aa90-88071a3ee8aa"));
            builder.Property(x => x.Name).HasMaxLength(200);
            builder.Property(x => x.Description).HasMaxLength(500);
            builder.Property(x => x.CreatedBy).HasMaxLength(200);
            builder.Property(x => x.ModifiedBy).HasMaxLength(200);

            builder.HasIndex(x => new { x.RealmId, x.Name }).IsUnique();

            (_dbType switch
            {
                DatabaseType.MySql => (Action<EntityTypeBuilder<Group>>)UseMySql,
                DatabaseType.Postgres => UsePostgres,
                _ => UseSqlServer
            })(builder);
        }

        void UseSqlServer(EntityTypeBuilder<Group> builder)
        {
            builder.Property(x => x.GroupId).HasDefaultValueSql("NEWID()");
        }

        void UseMySql(EntityTypeBuilder<Group> builder)
        {
            builder.Property(x => x.DateCreated).HasColumnType("datetime").HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.Property(x => x.DateModified).HasColumnType("datetime").HasDefaultValueSql("CURRENT_TIMESTAMP");
        }

        void UsePostgres(EntityTypeBuilder<Group> builder)
        {
            builder.Property(x => x.DateCreated)
                .HasColumnType("timestamp with time zone")
                .HasConversion(x => x.ToUniversalTime(), x => DateTime.SpecifyKind(x, DateTimeKind.Utc))
                .HasDefaultValueSql("now() AT TIME ZONE 'UTC'");

            builder.Property(x => x.DateModified)
                .HasColumnType("timestamp with time zone")
                .HasConversion(x => x.ToUniversalTime(), x => DateTime.SpecifyKind(x, DateTimeKind.Utc))
                .HasDefaultValueSql("now() AT TIME ZONE 'UTC'");

            builder.Property(x => x.DateInactive)
                .HasColumnType("timestamp with time zone")
                .HasConversion(x => x.HasValue ? x.Value.ToUniversalTime() : (DateTime?)null, x => x.HasValue ? DateTime.SpecifyKind(x.Value, DateTimeKind.Utc) : (DateTime?)null);
        }
    }
}
