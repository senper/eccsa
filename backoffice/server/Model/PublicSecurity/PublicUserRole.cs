using Microsoft.AspNetCore.Identity;
using System;

namespace ECCSA.BackOffice.Server.Model.PublicSecurity
{
    public class PublicUserRole : IdentityUserRole<Guid>
    {
        public virtual PublicUser User { get; set; }
        public virtual PublicRole Role { get; set; }

    }

    public class PublicRoleClam : IdentityRoleClaim<Guid>
    {

    }


    public class PublicUserToken : IdentityUserToken<Guid>
    {

    }
}