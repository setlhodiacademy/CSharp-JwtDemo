using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jwt.Demo.Entities
{
    public class Claim
    {
        public int ClaimId { get; set; }
        public string ClaimName { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
