using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SSO.Domain.Models;

namespace SSO.Infrastructure.Configs
{
    public class GroupConfig : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.HasKey(x => x.GroupId);
            builder.Property(x => x.GroupId).HasDefaultValueSql("NEWID()");

            builder.Property(x => x.Name).HasMaxLength(200);
            builder.Property(x => x.Description).HasMaxLength(500);
            builder.Property(x => x.CreatedBy).HasMaxLength(200);
            builder.Property(x => x.ModifiedBy).HasMaxLength(200);

            builder.HasIndex(x => x.Name).IsUnique();
        }
    }
}
