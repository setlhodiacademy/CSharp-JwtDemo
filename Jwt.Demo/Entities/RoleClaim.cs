using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jwt.Demo.Entities
{
    public class RoleClaim
    {
        public int RoleClaimId { get; set; }
        public int ClaimId { get; set; }
        public Claim Claim { get; set; }
        public int RoleId { get; set; }
        public DateTime DateCreated { get; set; }
        public int CreatedBy { get; set; }
    }
}
