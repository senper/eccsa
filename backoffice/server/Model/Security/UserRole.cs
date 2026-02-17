using Microsoft.AspNetCore.Identity;
using System;

namespace ECCSA.BackOffice.Server.Model.Security
{
    public class UserRole : IdentityUserRole<Guid>
    {
        public virtual User User { get; set; }
        public virtual Role Role { get; set; }

    }

    public class RoleClaim : IdentityRoleClaim<Guid>
    {

    }


    public class UserToken : IdentityUserToken<Guid>
    {

    }
}