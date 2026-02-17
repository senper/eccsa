using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Common.Notification;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECCSA.BackOffice.Server.Model.Membership
{
    public class Meeting : Entity
    {
        public String Title { get; set; }
        public DateTime Date { get; set; }

        public string Venue { get; set; }
        public String Agenda { get; set; }
        public string Code { get; set; }
        public MeetingStatusEnum Status { get; set; }
        public DateTime RegistrationDeadline { get; set; }
        public DateTime MinuteDeadline { get; set; }
        public List<Guest> DistinguishedGuests { get; set; } = new List<Guest>();


        public virtual ICollection<Artifact> Artifacts { get; set; }
        public virtual ICollection<Participant> Participants { get; set; }

        [NotMapped]
        public List<EmailMessage> Emails { get; set; } = new List<EmailMessage>();
        public virtual ICollection<MeetingAttendance> Attendances { get; set; }
        public virtual ICollection<Nominee> Nominees { get; set; }
    }
    
    public class Nominee
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public Guid MeetingId { get; set; }
        public Guid AssociationId { get; set; }
        public String Position { get; set; }
        public int YeaVotes { get; set; }
        public int NayVotes { get; set; }
        public int AbstainVotes { get; set; }

        public virtual Meeting Meeting { get; set; }
    }
    public class Participant
    {
        public Guid Id { get; set; }
        public Guid MeetingId { get; set; }
        public String FullName { get; set; } = String.Empty;
        public String Email { get; set; } = String.Empty;
        public String Mobile { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public String Remark { get; set; } = string.Empty;
        public Guid AssociationId { get; set; }
        public MinuteStatusEnum MinuteStatus { get; set; } = MinuteStatusEnum.NotSet;
        public List<Guest> OtherParticipants { get; set; } = new List<Guest>();
        public virtual Meeting Meeting { get; set; }

        [NotMapped]
        public String Association { get; set; }
    }
    public class MeetingAttendance
    {
        public Guid Id { get; set; }
        public DateTime AttendaceDate { get; set; }
        public string No { get; set; }
        public Guid MeetingId { get; set; }
        public Guid ParticipantId { get; set; }
        public virtual Meeting Meeting { get; set; }
        public virtual Participant Participant { get; set; }
    }
    public class Guest
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string OrderNo { get; set; } = string.Empty;
        public String FullName { get; set; } = string.Empty;
        public String Title { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
   

    public class Artifact
    {
        public Guid Id { get; set; }
        public Guid MeetingId { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }

        [NotMapped]
        public String RelativePath
        {
            get; set;
        }
        public virtual Meeting Meeting { get; set; }

    }

}
