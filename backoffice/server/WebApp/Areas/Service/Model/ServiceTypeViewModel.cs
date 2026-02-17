using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Common.Servicing;
using ECCSA.BackOffice.Server.Model.Membership;
using System;
using System.Collections.Generic;
using System.Linq;
using ECCSA.BackOffice.Server.Model.Security;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Service.Model
{
    public class ServiceTypeViewModel : ServiceViewModel<ServiceType>
    {
        public bool HasServiceRequest { get; set; }


        public List<LookupType> LookupTypes { get; set; }

        public List<ServiceRequirement> Groups { get; set; }

        public List<SimpleLookup> InputeTypes
        {
            get
            {
                List < SimpleLookup > lst = new List < SimpleLookup >();
                foreach (ServiceInputTypeEnum val in Enum.GetValues(typeof(ServiceInputTypeEnum)))
                {
                    if (val == ServiceInputTypeEnum.None || val == ServiceInputTypeEnum.Payment) continue;
                    lst.Add(new SimpleLookup()
                    {
                        Id = (int)val,
                        Name = val.GetDisplayName(),
                    });
                }
                return lst.OrderBy(l=> l.Name).ToList();
            }
        }

        public List<string> ReportTemplates { get; set; } = new List<string>();

        public List<User> Users { get; set; } = new List<User>();
    }
}
