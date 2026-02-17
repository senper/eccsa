using Microsoft.AspNetCore.Identity;
using System;

namespace ECCSA.BackOffice.Server.Model.Security
{
    public class UserLogin : IdentityUserLogin<Guid>
    {
        public virtual User User { get; set; }

    }
}