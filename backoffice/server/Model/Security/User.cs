using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Common.Notification;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ECCSA.BackOffice.Server.Model.Security
{
    public class User : IdentityUser<Guid>, INotificationAddress
    {
        
        public ECCSAModuleFlag AssignedModules { get; set; }

        public ECCSAModuleFlag DefaultModule { get; set; }

        public String FullName { get; set; }
        public bool Disabled { get; set; }

        public int? DepartmentId { get; set; } 
        public Guid AssociationId { get; set; }

        [NotMapped]
        public bool IsHeadOfficeUser => AssociationId == Guid.Parse("00000000-0000-0000-0000-0000001e8480");

       
        [NotMapped]
        public string Association { get;set; }
        [NotMapped]
        public Guid AssociationParentId { get; set; }
        public DateTime LockoutEndDateUtc { get; set; }
        public string TIN { get; set; } = "0000000000"; //"00 53 93 34 11"
        public string Address { get; set; }
        public string MobileNo { get; set; }
        public bool MobileNoConfirmed { get; set; } 
        public string EmailConfirmationCode { get; set; }
        public string MobileConfirmationCode { get; set; }
        public string PasswordResetCode { get; set; }
        public DateTime? ConfirmationCodeExpiryDate { get; set; }
        public DateTime? PasswordResetCodeExpiryDate { get; set; }
        public string OldEmail { get; set; }
        public string OldMobileNo { get; set; }
        public string CompanyName { get; set; }
        public string Title { get; set; }
        public string LogoExtension { get; set; }
        public bool BusinessAccount { get; set; }
        [NotMapped]
        public string Password { get; set; }

        [NotMapped]
        public IList<string> Roles { get; set; }
        public int PasswordTryCount { get; set; }
        public DateTime? PasswordDisabledTime { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<UserClaim> UserClaims { get; set; }
        public virtual ICollection<UserLogin> UserLogins { get; set; }

        [NotMapped]
        public bool IsArbitrator {
            get
            {
                if (Roles == null) return false;
                if (Roles.Count == 0) return DefaultModule.HasFlag(ECCSAModuleFlag.Arbitration);
               var arb = Role.Roles.First(r => r.Id == Role.ArbitratorRoleId);
               return Roles.Any(r => r == arb.NormalizedName);
            }}

    }
}