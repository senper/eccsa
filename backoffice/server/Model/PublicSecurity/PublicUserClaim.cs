using Microsoft.AspNetCore.Identity;
using System;

namespace ECCSA.BackOffice.Server.Model.PublicSecurity
{
    public class PublicUserClaim : IdentityUserClaim<Guid>
    {
        public Guid Id { get; set; }
        public virtual PublicUser User { get; set; }

    }
}