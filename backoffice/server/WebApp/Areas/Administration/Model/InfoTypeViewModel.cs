using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.ExternalData;
using ECCSA.BackOffice.Server.WebApp.Models;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Administration.Model
{
    public class InfoTypeViewModel : ICTViewModel<ExternalDataType>
    {
        public InfoTypeViewModel()
        {
            CurrentPage = "External Data Source";
            ChangeActive(MenuItemEnum.ExternalData);
        }
       
        public ECCSAModuleFlag Module { get; set; }

     
    }
}
