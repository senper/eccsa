using ECCSA.BackOffice.Server.Model.BusinessDirectory;
using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.WebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace ECCSA.BackOffice.Server.WebApp.Areas.BusinessDirectory.Model
{
    public enum AutoMapBy
    {
        Telephone=0,
        Mobile = 1
    }
    public class BusinessSectorViewModel : DirectoryViewModel<BusinessSector>
    {
        public Guid SectorId { get; set; }
        public string Name { get; set; }
        public List<TempCompany> TempCompanies { get; set; }
        public List<Company> Companies { get; set; }
        public List<BusinessSectorCompany> SectorCompanies { get; set; }
        public List<BusinessSector> RootBusinessSectors { get; set; }
    }

    public class CompanyViewModel : DirectoryViewModel<Company>
    {
        public Guid SectorId { get; set; }
        public string Name { get; set; }
        public string LogoPath { get; set; }
        public CompanyImportModel Company { get; set; } = new CompanyImportModel();
        public List<BusinessSectorCompany> SectorCompanies { get; set; }
        public List<BusinessSector> BusinessSectors { get; set; }
        public List<SelectListItem> SelectSource { get; set; } = new List<SelectListItem>();
    }
    public class CompanyImportModel : IId
    {
        public CompanyImportModel()
        {

        }
        public Guid Id { get; set; }

        public String Name { get; set; }
        public String Telephone { get; set; }
        public String Mobile { get; set; }
        public String Fax { get; set; }
        public String Email { get; set; }
        public String PoBox { get; set; }
        public String Web { get; set; }
        public Company ToCompany ()
        {
            return new Company()
            {
                Id = Id,
                BusinessSectors = new List<BusinessSectorCompany>(),
                Email = Email,
                Extension = String.Empty,
                Fax = Fax,
                Mobile = Mobile,
                Name = Name,
                PoBox = PoBox,
                Telephone = Telephone,
                Web = Web
            };
        }
    }
    public class DirectoryViewModel<T> : BaseViewModel
    {
        public DirectoryViewModel()
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
                {MenuItemEnum.BusinessInterestSource, "" },
                {MenuItemEnum.BusinessInterestProduct, "" },

            };
            if (MenuItem.ContainsKey(menuItem))
            {
                MenuItem[menuItem] = MenuItemActive;
            }
        }



        public List<T> TheList { get; set; }

        public T TheModel { get; set; }

        public IFormFile Logo { get; set; }



        public string Area => "BusinessDirectory";

    }

    public class DashboardViewModel : DirectoryViewModel<string>
    {
        public string Controller => "Dashboard";
        public int Sectors { get; set; }
        public int Companies { get; set; }
        public int Associations { get; set; }

        public int Projects { get; set; }
        public SummaryModel Summary { get; set; }
        public string SummaryJson { get; set; }

        public SummaryModel Summary2 { get; set; }
        public string SummaryJson2 { get; set; }
    }
}
