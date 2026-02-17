using Microsoft.AspNetCore.Identity;
using System;

namespace ECCSA.BackOffice.Server.Model.PublicSecurity
{
    public class PublicUserLogin : IdentityUserLogin<Guid>
    {
        public virtual PublicUser User { get; set; }

    }
}