using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SSO.Domain.Models;
using SSO.Infrastructure.Enums;

namespace SSO.Infrastructure.Configs
{
    public class RealmConfig : IEntityTypeConfiguration<Realm>
    {
        readonly DatabaseType _dbType;

        public RealmConfig(DatabaseType? dbType = DatabaseType.SqlServer)
        {
            _dbType = dbType.Value;
        }

        public void Configure(EntityTypeBuilder<Realm> builder)
        {
            builder.HasKey(x => x.RealmId);

            builder.Property(x => x.Name).HasMaxLength(200);
            builder.Property(x => x.CreatedBy).HasMaxLength(200);
            builder.Property(x => x.ModifiedBy).HasMaxLength(200);

            builder.HasIndex(x => x.Name).IsUnique();

            (_dbType switch
            {
                DatabaseType.MySql => (Action<EntityTypeBuilder<Realm>>)UseMySql,
                DatabaseType.Postgres => UsePostgres,
                _ => UseSqlServer
            })(builder);

            builder.HasMany(x => x.Applications)
                .WithOne(x => x.Realm)
                .HasForeignKey(x => x.RealmId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Groups)
                .WithOne(x => x.Realm)
                .HasForeignKey(x => x.RealmId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Users)
                .WithOne(x => x.Realm)
                .HasForeignKey(x => x.RealmId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        void UseSqlServer(EntityTypeBuilder<Realm> builder)
        {
            builder.Property(x => x.RealmId).HasDefaultValueSql("NEWID()");
        }

        void UseMySql(EntityTypeBuilder<Realm> builder)
        {
            builder.Property(x => x.DateCreated).HasColumnType("datetime").HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.Property(x => x.DateModified).HasColumnType("datetime").HasDefaultValueSql("CURRENT_TIMESTAMP");
        }

        void UsePostgres(EntityTypeBuilder<Realm> builder)
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
