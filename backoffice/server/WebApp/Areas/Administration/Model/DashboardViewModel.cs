using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Common.Servicing;
using ECCSA.BackOffice.Server.WebApp.Models;
using System.Collections.Generic;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Administration.Model
{
    public class DashboardViewModel : ICTViewModel<string>
    {
        public DashboardViewModel()
        {
            CurrentPage = "Dashboard";
            ChangeActive(MenuItemEnum.Dashboard);
        }
        public decimal MonthlyEarnings { get; set; }
        public decimal AnnualEarnings { get; set; }
        public int MonthlyNoOfProvidedServices { get; set; }
        public int AnnualNoOfProvidedServices { get; set; }
        public SummaryModel Summary { get; set; }
        public string SummaryJson { get; set; }

        public SummaryModel Summary2 { get; set; }
        public string SummaryJson2 { get; set; }
        public List<ServiceType> ServiceTypes { get; set; }
    }
}
