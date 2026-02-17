using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Membership;
using ECCSA.BackOffice.Server.WebApp.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Bmo.Model
{
    public class BusinessMembershipViewModel<T>: BaseViewModel
    {
        public BusinessMembershipViewModel()
        {
            ChangeActive(MenuItemEnum.Bmo);
            CurrentModule = ECCSAModuleFlag.Bmo;
            CurrentSubsystem = ECCSAModuleEnum.Bmo;

        }
        public void ChangeActive(MenuItemEnum menuItem)
        {
            MenuItem = new Dictionary<MenuItemEnum, string>
            {
                { MenuItemEnum.Bmo, "" },
                { MenuItemEnum.BmoType, "" },

            };
            if (MenuItem.ContainsKey(menuItem))
            {
                MenuItem[menuItem] = MenuItemActive;
            }
        }



        public List<T> TheList { get; set; } = new List<T>();

        public T TheModel { get; set; }

        public IFormFile Photo { get; set; }

        public string DefaultPersonPath { get; set; } = "/assets/img/membership/person.png";
        public string DefaultLogo { get; set; } = "/assets/img/membership/logo.png";

        public Contact Contact { get; set; }

        public string Area => "Bmo";


    }

    public enum FolderTypeEnum
    {
        Photo = 1,
        Document = 2
    }
}
