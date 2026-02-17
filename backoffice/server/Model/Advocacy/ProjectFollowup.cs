using ECCSA.BackOffice.Server.Model.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECCSA.BackOffice.Server.Model.Advocacy
{
    public class ProjectFollowup: Entity
    {
        public String Title { get; set; }

        public DateTime PlannedStartDate { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime PlannedEndDate { get; set; }

        [NotMapped]
        public String EndDateString => EndDate.HasValue ? EndDate.Value.ToString("yyyy-MM-dd") : "";
        public DateTime? EndDate { get; set; }
        public String Remark { get; set; }

        public Guid ProjectId { get; set; }
        public virtual Project Project { get; set; }
    }
}
