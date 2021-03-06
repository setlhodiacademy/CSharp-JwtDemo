using Jwt.Demo.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jwt.Demo.Persistence.Configurations
{
    public class RoleClaimConfigurations : IEntityTypeConfiguration<RoleClaim>
    {
        public void Configure(EntityTypeBuilder<RoleClaim> builder)
        {
            builder.Property(rc => rc.RoleClaimId).HasColumnName("RoleClaimId");
            builder.Property(rc => rc.ClaimId).IsRequired();
            builder.Property(rc => rc.RoleId).IsRequired();
            builder.Property(rc => rc.DateCreated).IsRequired();
            builder.Property(rc => rc.CreatedBy).IsRequired();
        }

    }
}
