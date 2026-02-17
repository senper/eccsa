using System;
using System.Collections.Generic;

namespace ECCSA.BackOffice.Server.Model.Arbitration
{
    public class ArbitrationTaskType
    {
        public Guid Id { get; set; }

        public String NameEng { get; set; }

        public String NameLocal { get; set; }

        public String DescriptionEng { get; set; }

        public String DescriptionLocal { get; set; }

        public decimal ServiceCharge { get; set; }

        public virtual ICollection<ArbitrationFollowup> Services { get; set; }
    }
}
