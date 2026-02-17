using ECCSA.BackOffice.Server.Model.Common;
using System;
using System.Collections.Generic;

namespace ECCSA.BackOffice.Server.Model.Arbitration
{
    public class ArbitrationServiceType : Entity
    {

        public String NameEng { get; set; }

        public String NameLocal { get; set; }

        public YesNoEnum RequiresThirdParty { get; set; }

        public String DescriptionEng { get; set; }

        public String DescriptionLocal { get; set; }

        public decimal ServiceCharge { get; set; }

        public virtual ICollection<ArbitrationService> Services { get; set; }
    }
}
