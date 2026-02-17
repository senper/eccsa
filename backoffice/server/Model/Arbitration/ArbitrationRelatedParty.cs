using System;

namespace ECCSA.BackOffice.Server.Model.Arbitration
{
    public class ArbitrationRelatedParty
    {
        public Guid Id { get; set; }

        public Guid ServiceId { get; set; }

        public string OrderNo { get; set; }

        public String Name { get; set; }

        public string TINumber { get; set; }
        public String Address { get; set; }

        public virtual ArbitrationService ArbitrationService { get; set; }
    }
}
