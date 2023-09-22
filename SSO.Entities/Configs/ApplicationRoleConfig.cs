﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace SSO.Entities.Configs
{
    public class ApplicationRoleConfig : IEntityTypeConfiguration<ApplicationRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationRole> builder)
        {
            builder.HasIndex(x => new { x.ApplicationId, x.Name }).IsUnique();

            builder.HasIndex(x => new { x.Name }).IsUnique(false);
        }
    }
}
