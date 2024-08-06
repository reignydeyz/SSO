using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SSO.Domain.Models;

namespace SSO.Infrastructure.Configs
{
    public class RootUserConfig : IEntityTypeConfiguration<RootUser>
    {
        public void Configure(EntityTypeBuilder<RootUser> builder)
        {
            builder.HasKey(e => e.UserId);
        }
    }
}
