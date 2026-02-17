using ECCSA.BackOffice.Server.Model.Common;
using System;

namespace ECCSA.BackOffice.Server.Model.Advocacy
{
    public class Research : Entity
    {
        public String Title { get; set; }
        public String Description { get; set; }

        public String ConductedBy { get; set; }

        public String Target { get; set; }
        public String Finding { get; set; }

        public ResearchStageEnum Stage { get; set; }

        public StatusEnum Status { get; set; }

        public String Remark { get; set; }
    }
}
