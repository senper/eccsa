using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Membership;
using ECCSA.BackOffice.Server.WebApp.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Advocacy.Model
{
    public class AdvocacyViewModel<T>: BaseViewModel
    {
        public AdvocacyViewModel()
        {
            ChangeActive(MenuItemEnum.Dashboard);
        }
        public void ChangeActive(MenuItemEnum menuItem)
        {
            MenuItem = new Dictionary<MenuItemEnum, string>
            {
                { MenuItemEnum.Dashboard, "" },
                { MenuItemEnum.AdvocacyDialog, "" },
                { MenuItemEnum.AdvocacyProject, "" },
                { MenuItemEnum.AdvocacyPublication, "" },
                { MenuItemEnum.AdvocacyResearch, "" },
                { MenuItemEnum.Events, "" },
                { MenuItemEnum.Resource, "" },
                { MenuItemEnum.Issue, "" },
                { MenuItemEnum.EmailMessage, "" },
                { MenuItemEnum.RecipientSource, "" },
                { MenuItemEnum.Plan, "" },
                { MenuItemEnum.Progress, "" },
                { MenuItemEnum.DownloadReport, "" },

            };
            if (MenuItem.ContainsKey(menuItem))
            {
                MenuItem[menuItem] = MenuItemActive;
            }
        }

     

        public List<T> TheList { get; set; }

        public T TheModel { get; set; }

        public IFormFile Photo { get; set; }


        public Contact Contact { get; set; }

        public string Area => "Advocacy";

    }

    public class DashboardViewModel : AdvocacyViewModel<string>
    {
        public string Controller => "Dashboard";
        public int Publications { get; set; }
        public int Researches { get; set; }
        public int Dialogs { get; set; }

        public int Projects { get; set; }
        public SummaryModel Summary { get; set; }
        public string SummaryJson { get; set; }

        public SummaryModel Summary2 { get; set; }
        public string SummaryJson2 { get; set; }

        
    }
}
