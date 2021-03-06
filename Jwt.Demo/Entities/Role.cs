using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jwt.Demo.Entities
{
    public class Role
    {
        public Role()
        {
            RoleClaims = new List<RoleClaim>();
        }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public ICollection<RoleClaim> RoleClaims { get; set; }

    }
}
