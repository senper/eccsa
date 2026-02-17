using ECCSA.BackOffice.Server.Model.Common;
using System;

namespace ECCSA.BackOffice.Server.Model.Arbitration
{
    public class ArbitrationDocument
    {
        public Guid Id { get; set; }

        public Guid ServiceId { get; set; }

        public string OrderNo { get; set; }

        public Guid DocumentTypeId { get; set; }

        public String Name { get; set; }

        public String Content { get; set; }

        public DateTime? DateSubmitted { get; set; }

        public Guid? ReviewedBy { get; set; }

        public DateTime? DateReviewed { get; set; }
        public String Remark { get; set; }
        public WorkflowStateEnum ReviewStatus { get; set; }

        public virtual ArbitrationService ArbitrationService { get; set; }
    }
}
