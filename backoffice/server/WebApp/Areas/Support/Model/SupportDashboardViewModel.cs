using ECCSA.BackOffice.Server.WebApp.Models;
using System.Collections.Generic;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Support.Model
{
    public class SupportViewModel<T> : BaseViewModel
    {
        public SupportViewModel()
        {
            ChangeActive(MenuItemEnum.Dashboard);
        }
        public void ChangeActive(MenuItemEnum menuItem)
        {
            MenuItem = new Dictionary<MenuItemEnum, string>
            {
                { MenuItemEnum.Dashboard, "" },
            };
            if (MenuItem.ContainsKey(menuItem))
            {
                MenuItem[menuItem] = MenuItemActive;
            }
        }
        public string GetStatusText(int status)
        {
            return status == 1 ? "Active" : "Inactive";
        }
        public List<T> TheList { get; set; }

        public T TheModel { get; set; }
    }
    public class SupportDashboardViewModel : SupportViewModel<string>
    {
        public SupportDashboardViewModel()
        {
            ChangeActive(MenuItemEnum.Dashboard);
        }
       
        
    }
}
