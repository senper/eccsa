using ECCSA.BackOffice.Server.WebApp.Models;
using System;
using System.Collections.Generic;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Academy.Model
{
    public class AcademyViewModel<T>: BaseViewModel
    {
        public AcademyViewModel()
        {
            ChangeActive(MenuItemEnum.AcademyDashboard);
        }
        public void ChangeActive(MenuItemEnum menuItem)
        {
            MenuItem = new Dictionary<MenuItemEnum, string>
            {
                { MenuItemEnum.AcademyDashboard, "" },
                { MenuItemEnum.AcademyTopicCategory, "" },
                { MenuItemEnum.AcademyTopic, "" },
                { MenuItemEnum.AcademyTraining, "" },
                { MenuItemEnum.AcademyRegistration, "" },
                { MenuItemEnum.AcademyAssessment, "" },
                { MenuItemEnum.AcademySponsors, "" },
                { MenuItemEnum.AcademySettingVariables, "" },
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

    public class Lookup
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class Lookup2
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
