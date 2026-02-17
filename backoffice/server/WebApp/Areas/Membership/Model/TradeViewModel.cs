using ECCSA.BackOffice.Server.Model.Membership;
using System;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Membership.Model
{
    public class TradingViewModel:MembershipViewModel<BusinessInformation>
    {
        public DateTime EndDateAfter { get; set; } = DateTime.Today;
        public string Controller { get; set; }
    }
}
