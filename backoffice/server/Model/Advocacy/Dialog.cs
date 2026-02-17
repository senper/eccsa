using ECCSA.BackOffice.Server.Model.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECCSA.BackOffice.Server.Model.Advocacy
{
    public class Dialog : Entity
    {
        public String Title { get; set; }
        public String Description { get; set; }

        public String ResponsiblePerson { get; set; }

        public String TargetInstitution { get; set; }
        public String Outcome { get; set; }

        public String ImplementationPeriod { get; set; }

        public String ConsultationDates { get; set; }

        public String Remark { get; set; }

        public StatusEnum Status { get; set; } = StatusEnum.Active;
        public virtual ICollection<Recommendation> Recommendations { get; set; }
        public virtual ICollection<PPDMeeting> Meetings { get; set; }
        public virtual ICollection<PPDAttachment> Documents { get; set; }
    }


    public class Recommendation : Entity
    {
        public Guid DialogId { get; set; }
        public int No { get; set; }
        public string Title { get; set; } = String.Empty;

        [NotMapped]
        public String NoTitle => $"{No} {Title}";
        public PPDRecommendationEnum Status { get; set; }
        public String ImplementationText { get; set; } = String.Empty;
        public String ResponsibleBody { get; set; } = String.Empty;
        public String FollowupPerson { get; set; } = String.Empty;
        public String Remark { get; set; } = String.Empty;

        public virtual Dialog Dialog { get; set; }
        public virtual ICollection<MeetingPoint> Points { get; set; }
    }

    public class PPDMeeting : Entity
    {
        public Guid DialogId { get; set; }
        public DateTime Date { get; set; }
        public String Agendas { get; set; } = String.Empty;
        public String Participants { get; set; } = String.Empty;
        public String ChairPersons { get; set; } = String.Empty;
        public String Consultant { get; set; } = String.Empty;
        public String TWGMembers { get; set; } = String.Empty;
        public String Remark { get; set; } = String.Empty;
        public virtual Dialog Dialog { get; set; }


        public virtual ICollection<PPDAttachment> Documents { get; set; }
        public virtual ICollection<MeetingPoint> Points { get; set; }
    }

    public class PPDAttachment : Entity
    {
        public Guid DialogId { get; set; }
        public Guid MeetingId { get; set; }
        public String Title { get; set; } = String.Empty;
        public String Extension { get; set; } = String.Empty;
        public Boolean IsPublic { get; set; }

        public virtual PPDMeeting Meeting { get; set; }
        public virtual Dialog Dialog { get; set; }
    }

    public class MeetingPoint
    {
        public Guid Id { get; set; }
        public Guid MeetingId { get; set; }
        public Guid RecommendationId { get; set; }
        public PPDRecommendationEnum Declaration { get; set; }
        public String ResponsibleBody { get; set; } = String.Empty;
        public String FollowupPerson { get; set; } = String.Empty;

        [NotMapped]
        public String ImplementationText { get; set; }
        

        [NotMapped]
        public String Remark { get; set; }

        public virtual PPDMeeting Meeting { get; set; }

        public virtual Recommendation Recommendation { get; set; }
    }
}
