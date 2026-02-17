using ECCSA.BackOffice.Server.WebApp.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Service.Model
{
    public class ServiceViewModel<T> : BaseViewModel
    {
        public ServiceViewModel()
        {
            ChangeActive(MenuItemEnum.Services);
        }
        public void ChangeActive(MenuItemEnum menuItem)
        {
            MenuItem = new Dictionary<MenuItemEnum, string>
            {
                { MenuItemEnum.Dashboard, "" },
                { MenuItemEnum.ServiceType, "" },
                { MenuItemEnum.Services, "" },
                { MenuItemEnum.Appointments, "" },
            };
            if (MenuItem.ContainsKey(menuItem))
            {
                MenuItem[menuItem] = MenuItemActive;
            }
        }
        
    
        public List<T> TheList { get; set; }

        public T TheModel { get; set; }

        public IFormFile Photo { get; set; }

        public string DefaultPersonPath { get; set; } = "/assets/img/survey/person.png";
        public string DefaultLogo { get; set; } = "/assets/img/survey/logo.png";


    }


}
