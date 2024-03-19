using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SSO.Domain.Models;

namespace SSO.Infrastructure.Configs
{
    public class GroupUserConfig : IEntityTypeConfiguration<GroupUser>
    {
        public void Configure(EntityTypeBuilder<GroupUser> builder)
        {
            builder.HasKey(e => new { e.GroupId, e.UserId });

            builder.HasOne(x => x.Group).WithMany(x => x.Users)
                .HasForeignKey(d => d.GroupId);
        }
    }
}
