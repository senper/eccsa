using ECCSA.BackOffice.Server.Model.Membership;
using ECCSA.BackOffice.Server.WebApp.Models;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Administration.Model
{
    public class BankAccountApprovalNeedsViewModel : ICTViewModel<AssociationSetting>
    {
        public BankAccountApprovalNeedsViewModel()
        {
            CurrentPage = "External Data";
            ChangeActive(MenuItemEnum.BankAccountApprovalNeeds);
        }
    }
    public class FeeApprovalNeedsViewModel : ICTViewModel<Association>
    {
        public FeeApprovalNeedsViewModel()
        {
            CurrentPage = "External Data";
            ChangeActive(MenuItemEnum.FeeApprovalNeeds);
        }
    }
}
