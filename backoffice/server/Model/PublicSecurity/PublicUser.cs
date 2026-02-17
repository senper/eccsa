using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace ECCSA.BackOffice.Server.Model.PublicSecurity
{
    public class PublicUser : IdentityUser<Guid>
    {
        public String FullName { get; set; }

        public Guid MemberId { get; set; }
        public DateTime LockoutEndDateUtc { get; set; }

        public virtual ICollection<PublicUserRole> UserRoles { get; set; }
        public virtual ICollection<PublicUserClaim> UserClaims { get; set; }
        public virtual ICollection<PublicUserLogin> UserLogins { get; set; }



    }
}