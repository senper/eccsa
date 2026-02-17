using ECCSA.BackOffice.Server.Model.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECCSA.BackOffice.Server.Model.Advocacy
{
    public class Project : Entity
    {
        public String Title { get; set; }
        public String Description { get; set; }


        public DateTime PlannedStartDate { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime PlannedEndDate { get; set; }
        [NotMapped]
        public String EndDateString => EndDate.HasValue ? EndDate.Value.ToString("yyyy-MM-dd") : "";
        public DateTime? EndDate { get; set; }
        public StatusEnum Status { get; set; }

        public String Remark { get; set; }

        [NotMapped]
        public ProjectStatusEnum ProjectStatus
        {
            get
            {
                if (StartDate > DateTime.Today)
                    return ProjectStatusEnum.NotStarted;
                if (EndDate.HasValue && EndDate.Value < DateTime.Today)
                    return ProjectStatusEnum.Completed;
                if (Status == StatusEnum.Inactive)
                    return ProjectStatusEnum.StartedInactive;
                return ProjectStatusEnum.Started;
            }
        }

        public virtual ICollection<ProjectFollowup> Followups { get; set; }
    }
}
