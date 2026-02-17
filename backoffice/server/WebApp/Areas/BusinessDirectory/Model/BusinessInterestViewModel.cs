using ECCSA.BackOffice.Server.Model.BusinessDirectory;
using ECCSA.BackOffice.Server.Model.Common;
using System;
using System.Collections.Generic;

namespace ECCSA.BackOffice.Server.WebApp.Areas.BusinessDirectory.Model
{
    public class BusinessInterestViewModel : DirectoryViewModel<BusinessInterestSource>
    {
        public List<BusinessSector> Sectors { get; set; }
        public List<BusinessInterestProduct> Products { get; set; }
        public Guid? SectorId { get; set; }
        public List<LookupValue> BusinessTypes { get; set; }
    }
    public class BusinessInterestProductViewModel : DirectoryViewModel<BusinessInterestProduct>
    {
    }
}
