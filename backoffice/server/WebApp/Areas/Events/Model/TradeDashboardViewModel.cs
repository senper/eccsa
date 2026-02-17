using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Common.Servicing;
using ECCSA.BackOffice.Server.WebApp.Models;
using System.Collections.Generic;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Events.Model
{
    public class TradeDashboardViewModel : BaseViewModel
    {

        public TradeDashboardViewModel()
        {
            ChangeActive(MenuItemEnum.Events);
        }
        public void ChangeActive(MenuItemEnum menuItem)
        {
            MenuItem = new Dictionary<MenuItemEnum, string>
            {
                { MenuItemEnum.Dashboard, "" },
                { MenuItemEnum.EventType, "" },
                { MenuItemEnum.Events, "" },
            };
            if (MenuItem.ContainsKey(menuItem))
            {
                MenuItem[menuItem] = MenuItemActive;
            }
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
