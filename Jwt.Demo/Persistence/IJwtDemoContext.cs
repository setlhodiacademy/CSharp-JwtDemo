using Jwt.Demo.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Jwt.Demo.Persistence
{
    public interface IJwtDemoContext
    {
        DbSet<Claim> Claims { get; set; }
        DbSet<RoleClaim> RoleClaims { get; set; }
        DbSet<Role> Roles { get; set; }
        DbSet<User> Users { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}