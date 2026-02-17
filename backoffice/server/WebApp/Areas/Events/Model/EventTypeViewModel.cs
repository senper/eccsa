using ECCSA.BackOffice.Server.Model.Common.EventManagement;
using ECCSA.BackOffice.Server.WebApp.Models;
using System.Collections.Generic;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Events.Model
{
    public class EventTypeViewModel : BaseViewModel
    {
        public EventTypeViewModel()
        {
            ChangeActive(MenuItemEnum.EventType);
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


        public List<EventType> TheList { get; set; }

        public EventType TheModel { get; set; }
    }
}
