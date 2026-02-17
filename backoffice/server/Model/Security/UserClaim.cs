using Microsoft.AspNetCore.Identity;
using System;

namespace ECCSA.BackOffice.Server.Model.Security
{
    public class UserClaim : IdentityUserClaim<Guid>
    {
        public Guid Id { get; set; }
        public virtual User User { get; set; }

    }
}