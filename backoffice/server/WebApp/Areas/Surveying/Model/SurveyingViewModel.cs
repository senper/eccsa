using ECCSA.BackOffice.Server.WebApp.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Surveying.Model
{
    public class SurveyingViewModel<T>: BaseViewModel
    {
        public SurveyingViewModel()
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
                { MenuItemEnum.Meeting, "" },
                {MenuItemEnum.SurveySetting, "" },
                
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
