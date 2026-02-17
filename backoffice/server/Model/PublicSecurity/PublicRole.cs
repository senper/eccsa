

using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace ECCSA.BackOffice.Server.Model.PublicSecurity 
{
    public class PublicRole : IdentityRole<Guid>
    {
        public virtual ICollection<PublicUserRole> UserRoles { get; set; }        
    }
}