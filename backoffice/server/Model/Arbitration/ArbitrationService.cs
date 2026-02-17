using ECCSA.BackOffice.Server.Model.Common;
using System;
using System.Collections.Generic;

namespace ECCSA.BackOffice.Server.Model.Arbitration
{
    public class ArbitrationService : Entity
    {

        public Guid AssociationId { get; set; }
        public string TIN { get; set; }
        public String FullName { get; set; }
        public String EmailAddress { get; set; }
        public String MobileNo { get; set; }

        public Guid ServiceTypeId { get; set; }

        public DateTime RequestDate { get; set; }
        public String InvoiceNo { get; set; }
                         
        public String ReceiptNo { get; set; }

        public PaymentMethodEnum PaymentMethod { get; set; }

        public WorkflowStateEnum RequestStatus { get; set; }

        public String Description { get; set; }


        public ArbitrationServiceType ServiceType { get; set; }
        public virtual ICollection<ArbitrationDocument> Documents { get; set; }

        public virtual ICollection<ArbitrationFollowup> Followups { get; set; }

        public virtual ICollection<ArbitrationRelatedParty> RelatedParties { get; set; }
    }
}
