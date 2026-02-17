using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Membership;
using System;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Service.Model
{
    public class SimpleLookupViewModel : SimpleLookup
    {
        public Guid ServiceTypeId { get; set; }
        public Guid RequirementId { get; set; }
    }

    public class RequirementHeaderViewModel 
    {
        public Guid ServiceTypeId { get; set; }
        public Guid Id { get; set; }

        public string OrderNo { get; set; }
        public String Name { get; set; }
        public String NameLocal { get; set; }

        
    }

    public class RequirementInvoiceViewModel : RequirementHeaderViewModel
    {
        public String Description { get; set; }

        public String DescriptionLocal { get; set; }

        public bool IsForOpponent { get; set; }
        public StatusEnum Status { get; set; }
    }
}
