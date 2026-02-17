using ECCSA.BackOffice.Server.Model.Common;
using System;

namespace ECCSA.BackOffice.Server.Model.Arbitration
{
    public class ArbitrationFollowup: Entity
    {

        public Guid ServiceId { get; set; }

        public Guid TaskTypeId { get; set; }

        public DateTime PlannedDate { get; set; }

        public Guid FollowupResultId { get; set; }

        public String Description { get; set; }

        public String Remark { get; set; }

        public virtual ArbitrationService ArbitrationService { get; set; }

        public virtual ArbitrationTaskType TaskType { get; set; }

        public virtual ArbitrationFollowupResult FollowupResult { get; set; }
    }
}
