using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Membership;
using ECCSA.BackOffice.Server.WebApp.Models;
using System.Collections.Generic;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Administration.Model
{
    public class LookupSettingsViewModel : ICTViewModel<LookupType>
    {
        public LookupSettingsViewModel()
        {
            CurrentPage = "Lookup Types";
            ChangeActive(MenuItemEnum.Lookups);
        }
    }

    public class IssueLocationViewModel : ICTViewModel<IssueLocation>
    {
        public IssueLocationViewModel()
        {
            CurrentPage = "Issue Location";
            ChangeActive(MenuItemEnum.IssueLocation);
        }
        public string SelectedCountry { get; set; } = string.Empty;
        public List<LookupValue> LocationTypes { get; set; } = new List<LookupValue>();
        public List<Country> Countries { get; set; } = Country.Countries;
    }

    public class IssueTypeViewModel : ICTViewModel<IssueType>
    {
        public IssueTypeViewModel()
        {
            CurrentPage = "Issue Type";
            ChangeActive(MenuItemEnum.IssueType);
        }
        public string SelectedCountry { get; set; } = string.Empty;
        
    }
}
