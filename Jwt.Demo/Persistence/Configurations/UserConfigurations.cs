using Jwt.Demo.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jwt.Demo.Persistence.Configurations
{
    public class UserConfigurations : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(o => o.UserId);
            builder.Property(o => o.Username).HasMaxLength(100).IsRequired();
            builder.Property(o => o.FirstName).HasMaxLength(255).IsRequired();
            builder.Property(o => o.Surname).HasMaxLength(255).IsRequired();
            builder.Property(o => o.RoleId).IsRequired();
        }
    }
}
