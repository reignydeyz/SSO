using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SSO.Domain.Models;

namespace SSO.Infrastructure.Configs
{
    public class RealmUserConfig : IEntityTypeConfiguration<RealmUser>
    {
        public void Configure(EntityTypeBuilder<RealmUser> builder)
        {
            builder.HasKey(e => new { e.RealmId, e.UserId });

            builder.HasOne(x => x.Realm).WithMany(x => x.Users)
                .HasForeignKey(d => d.RealmId);
        }
    }
}
