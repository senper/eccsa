using ECCSA.BackOffice.Server.WebApp.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Administration.Model
{
    public class ICTViewModel<T> : BaseViewModel
    {
        public ICTViewModel()
        {
            CurrentModule = Server.Model.Common.ECCSAModuleFlag.Administration;
        }
        protected void ChangeActive(MenuItemEnum menuItem)
        {
            MenuItem = new Dictionary<MenuItemEnum, string>
            {
                { MenuItemEnum.Dashboard, "" },
                { MenuItemEnum.EmailMessage, "" },
                { MenuItemEnum.ExternalData, "" },
                { MenuItemEnum.RecipientSource, "" },
                { MenuItemEnum.SMS, "" },
                { MenuItemEnum.Users, "" },
                { MenuItemEnum.MemberUsers, "" },
                { MenuItemEnum.PublicUser, "" },
                { MenuItemEnum.Lookups, "" },
                { MenuItemEnum.IssueLocation, "" },
                { MenuItemEnum.IssueType, "" },
                { MenuItemEnum.ESWRequest, "" },
                { MenuItemEnum.BankAccountApprovalNeeds, "" },
                { MenuItemEnum.FeeApprovalNeeds, "" },
                { MenuItemEnum.ReportSetup, "" },
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
