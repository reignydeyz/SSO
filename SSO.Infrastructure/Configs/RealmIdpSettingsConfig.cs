using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SSO.Domain.Models;
using SSO.Infrastructure.Enums;

namespace SSO.Infrastructure.Configs
{
    public class RealmIdpSettingsConfig : IEntityTypeConfiguration<RealmIdpSettings>
    {
        readonly DatabaseType _dbType;

        public RealmIdpSettingsConfig(DatabaseType? dbType = DatabaseType.SqlServer)
        {
            _dbType = dbType.Value;
        }

        public void Configure(EntityTypeBuilder<RealmIdpSettings> builder)
        {
            builder.HasKey(x => new { x.RealmId, x.IdentityProvider });

            (_dbType switch
            {
                DatabaseType.MySql => (Action<EntityTypeBuilder<RealmIdpSettings>>)UseMySql,
                DatabaseType.Postgres => UsePostgres,
                _ => UseSqlServer
            })(builder);
        }

        void UseSqlServer(EntityTypeBuilder<RealmIdpSettings> builder)
        {
            builder.Property(x => x.IdentityProvider).HasColumnType("tinyint");
        }

        void UseMySql(EntityTypeBuilder<RealmIdpSettings> builder)
        {
            // Implementation
        }

        void UsePostgres(EntityTypeBuilder<RealmIdpSettings> builder)
        {
            // Implementation
        }
    }
}
