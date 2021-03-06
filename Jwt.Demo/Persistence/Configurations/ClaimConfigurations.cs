using Jwt.Demo.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jwt.Demo.Persistence.Configurations
{
    public class ClaimConfigurations : IEntityTypeConfiguration<Claim>
    {
        public void Configure(EntityTypeBuilder<Claim> builder)
        {
            builder.Property(c => c.ClaimId);
            builder.Property(c => c.ClaimName).IsRequired().HasMaxLength(100);
            builder.Property(c => c.DateCreated).IsRequired();
        }
    }
}
