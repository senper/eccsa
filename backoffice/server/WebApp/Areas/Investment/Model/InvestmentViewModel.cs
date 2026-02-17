using ECCSA.BackOffice.Server.Model.Investment;
using ECCSA.BackOffice.Server.WebApp.Models;
using System.Collections.Generic;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Investment.Model
{
    public class InvestmentViewModel<T> : BaseViewModel
    {
        public InvestmentViewModel()
        {
            ChangeActive(MenuItemEnum.Services);
        }
        public void ChangeActive(MenuItemEnum menuItem)
        {
            MenuItem = new Dictionary<MenuItemEnum, string>
            {
                { MenuItemEnum.Dashboard, "" },
                { MenuItemEnum.InvestmentBusiness, "" },
                { MenuItemEnum.RegionalProfile, "" },
                { MenuItemEnum.CountryProfile, "" },
                { MenuItemEnum.ServiceType, "" },
                { MenuItemEnum.Services, "" },
            };
            if (MenuItem.ContainsKey(menuItem))
            {
                MenuItem[menuItem] = MenuItemActive;
            }
        }


        public List<T> TheList { get; set; }

        public T TheModel { get; set; }
        public string Area => "Investment";
        public string Layout => "_InvestmentLayout.cshtml";
    }
    public class BusinessViewModel: InvestmentViewModel<Business>
    {
        public string SearchTerm { get; set; }
        public string Controller => "Index";
    }

    public class ProfileViewModel : InvestmentViewModel<RegionSector>
    {
        public List<RegionSector> Regions { get; set; }
        public List<RegionSector> Sectors { get; set; }
        public RegionSector Ethiopia { get; set; }
        public string LogoPath { get; set; } = "/assets/img/membership/logo.png";
        public List<string> ProfileImages = new List<string>();
        public List<string> ProfileAttachments = new List<string>();
        public string Controller => "Profile";
    }
}
