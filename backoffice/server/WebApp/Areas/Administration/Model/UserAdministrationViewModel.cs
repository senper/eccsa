using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Common.Servicing;
using ECCSA.BackOffice.Server.Model.Membership;
using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.WebApp.Models;
using System.Collections.Generic;
using ECCSA.BackOffice.Server.Model.Common.Reports;
using System.ComponentModel.DataAnnotations;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Administration.Model
{
    public class ReportViewModel : ICTViewModel<Server.Model.Common.Reports.Report>
    {
        public ReportViewModel()
        {
            CurrentPage = "Report Setup";
            ChangeActive(MenuItemEnum.ReportSetup);
        }
        public List<ReportSource> Sources { get; set; }
        public List<SimpleLookup> AllModules { get; set; } = new List<SimpleLookup>();

        public List<ECCSAModuleFlag> AssignedModules
        {
            get
            {
                var lst = new List<ECCSAModuleFlag>();
                if (TheModel == null) return lst;
                if( 0 == (int)TheModel.ReportOwner ) return lst;
                return lst;
            }
        } 

        public List<LookupType> LookupTypes { get; set; }
    }
    public class UserAdministrationViewModel : ICTViewModel<User>
    {
        public UserAdministrationViewModel()
        {
            CurrentPage = "Backoffice User Administration";
            ChangeActive(MenuItemEnum.Users);
        }
        public List<Role> Roles { get; set; }

        public List<ECCSAModuleFlag> AssignedModules { get; set; } = new List<ECCSAModuleFlag>();

        public List<SimpleLookup> AllModules { get; set; } = new List<SimpleLookup>();
        public List<UserLoginHistory> UserLoginHistories { get; set; }
        public List<LookupValue> SupportingUnits { get; set; }
    }
    public class MemberUserAdministrationViewModel : ICTViewModel<User>
    {
        public MemberUserAdministrationViewModel()
        {
            CurrentPage = "Member User Administration";
            ChangeActive(MenuItemEnum.MemberUsers);
        }
        public List<Role> Roles { get; set; }

        public List<ECCSAModuleFlag> AssignedModules { get; set; } = new List<ECCSAModuleFlag>();

        public List<Association> Associations { get; set; } = new List<Association>();

        public List<SimpleLookup> AllModules { get; set; } = new List<SimpleLookup>();
        public List<UserLoginHistory> UserLoginHistories { get; set; }
    }
    public class PublicUserAdministrationViewModel : ICTViewModel<User>
    {
        public PublicUserAdministrationViewModel()
        {
            CurrentPage = "Public User Administration";
            ChangeActive(MenuItemEnum.PublicUser);
        }
        public List<ExportAgreement> ExportAgreements { get; set; } = new();
        public List<LegalProfile> LegalProfiles { get; set; } = new();
        public List<UserContact> UserContacts { get; set; } = new();
        public List<UserLoginHistory> UserLoginHistories { get; set; }
        public PublicUserFilter Filter { get; set; }
    }
    public enum PublicUserFilter
    {
        All = 0,
        [Display(Name = "Email/Mobile Not Confirmed")]
        EmailMobileNotConfirmed = 1,
        [Display(Name = "Email Change Not Confirmed")]
        EmailChangeNotConfirmed = 2,
        [Display(Name = "Mobile Change Not Confirmed")]
        MobileChangeNotConfirmed = 3
    }

    public class RegisterInputModel
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public string TIN { get; set; }
        public string Address { get; set; }
        public string MobileNo { get; set; }
        public string CompanyName { get; set; }
    }
    public class UpdateProfileModel
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        //public string Tin { get; set; }
        public string PhoneNo { get; set; }
    }
    public class ChangePasswordModel
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }

    
}
