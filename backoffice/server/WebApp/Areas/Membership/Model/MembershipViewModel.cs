using ECCSA.BackOffice.Server.Model.Membership;
using ECCSA.BackOffice.Server.WebApp.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Membership.Model
{
    public class MembershipViewModel<T>: BaseViewModel
    {
        public MembershipViewModel()
        {
            ChangeActive(MenuItemEnum.Dashboard);
            
        }
        public void ChangeActive(MenuItemEnum menuItem)
        {
            MenuItem = new Dictionary<MenuItemEnum, string>
            {
                { MenuItemEnum.Dashboard, "" },
                { MenuItemEnum.Association, "" },
                { MenuItemEnum.Individual, "" },
                { MenuItemEnum.Applicant, "" },
                { MenuItemEnum.Issue, "" },
                { MenuItemEnum.Meeting, "" },
                {MenuItemEnum.MembershipTrade, "" },
                 {MenuItemEnum.SurveySetting, "" },
                {MenuItemEnum.Survey, "" },
                {MenuItemEnum.BusinessSector, "" },
                { MenuItemEnum.WebsiteContent, "" },
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

        public string Area => "Membership";

        public List<Association> Associations { get; set; } = new List<Association>();
        public List<Individual> Individuals { get; set; } = new List<Individual>();

    }

    public enum FolderTypeEnum
    {
        Photo = 1,
        Document = 2
    }
}
