using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECCSA.BackOffice.Server.Model.Common.Servicing
{
    public class ServiceFollowup : Entity
    {

        public Guid ServiceId { get; set; }

        public Guid ServiceTypeId { get; set; }

        public string Title { get; set; }

        public DateTime StartDate { get; set; }

        public string StartTime { get; set; }

        [NotMapped]
        public string StartDateStr => StartDate.ToString("yyyy-MM-yy");
        [NotMapped]
        public string StartTimeStr => StartTime;

        [NotMapped]
        public DateTime StartDateTime => StartDate.Date;

       

        public string Description { get; set; }

        public string? Extension { get; set; }
                
        public TaskStatusEnum Status { get; set; }

        
        public virtual ServiceRequest ServiceRequest { get; set; }

        

        [NotMapped]
        public bool HasAttachment => !string.IsNullOrEmpty(Extension) && Extension.Length > 2;

    }

}