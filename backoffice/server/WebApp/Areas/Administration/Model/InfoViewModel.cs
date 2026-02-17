using ECCSA.BackOffice.Server.Model.ExternalData;
using ECCSA.BackOffice.Server.Model.Trade;
using ECCSA.BackOffice.Server.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Administration.Model
{
    public class InfoViewModel : ICTViewModel<ExternalDataType>
    {
        public InfoViewModel()
        {
            CurrentPage = "External Data";
            ChangeActive(MenuItemEnum.ExternalData);
        }
        
        
        public Guid InfoTypeId { get; set; }
       
        public DataTable TheData { get; set; }

        public ExternalDataType ExternalDataType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string Title { get; set; }
    }
    public class CertificateViewModel:ICTViewModel<CertificateOfOrigin>
    {
        public CertificateViewModel()
        {
            CurrentPage = "Certificate Of Origin";
            ChangeActive(MenuItemEnum.ESWRequest);
        }
        public DateTime ImportDate { get; set; }

        public List<ESWRequest> EswRequests { get; set; }
    }


}
