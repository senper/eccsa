using System.Collections.Generic;
using ECCSA.BackOffice.Server.Model.Website;
using ECCSA.BackOffice.Server.WebApp.Models;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Bmo.Model
{

    public sealed class BmoViewModel : BusinessMembershipViewModel<OtherMedia>
    {
        public BmoViewModel()
        {
            
            CurrentPage = "Index";
            ChangeActive(MenuItemEnum.Bmo);
        }
        public string Controller => "Home";
        public List<MediaType> MediaTypes { get; set; }

        public List<string> ProfileImages = new List<string>();

    }

    public class BmoTypeViewModel : BusinessMembershipViewModel<MediaType>
    {
        public BmoTypeViewModel()
        {
            ChangeActive(MenuItemEnum.BmoType);
            CurrentPage = "Index";
        }

        public List<OtherMedia> Contents { get; set; } = new List<OtherMedia>();

        public string Controller => "BmoType";
    }
}
