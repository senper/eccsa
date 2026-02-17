using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECCSA.BackOffice.Server.Model.Common.EventManagement
{
    public class EventDetail
    {
        public Guid DetailId { get; set; }
        public Guid EventId { get; set; }

        public String Title { get; set; }

        public String TitleLocal { get; set; }

        [NotMapped]
        public String TitleFull => $"{Title} {TitleLocal}";

        [NotMapped]
        public bool PublishAsService => ServiceTypeId.HasValue && ServiceTypeId.Value != Guid.Empty;

        public Guid? ServiceTypeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        [NotMapped]
        public string StartDateStr => StartDate.ToString("dd/MM/yyyy");
        [NotMapped]
        public string EndDateStr => EndDate.ToString("dd/MM/yyyy");
        [NotMapped] 
        public bool IsFuture => DateTime.Today < StartDate;
        [NotMapped]
        public bool IsEnded => DateTime.Today >= EndDate;

        [NotMapped]
        public RateBasisEnum RateBasis { get; set; } = RateBasisEnum.NotSet;
        [NotMapped]
        public decimal MemberRate { get; set; }
        [NotMapped]
        public decimal OtherRate { get; set; }
        public virtual Event Event { get; set; }
    }
    public class EventScheduleDetail
    {
        public String Title { get; set; }

        public String TitleLocal { get; set; }

        [NotMapped]
        public String TitleFull => $"{Title} {TitleLocal}";
        public DateTime Date { get; set; }
        public string DateStr => Date.ToString("dd/MM/yyyy");
        public bool IsEnded { get; set; }
        public bool IsFuture { get; set; }
        public bool IsEndDate { get; set; }
    }
}
