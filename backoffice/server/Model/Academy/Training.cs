using ECCSA.BackOffice.Server.Model.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECCSA.BackOffice.Server.Model.Academy
{
    public class Training: Entity
    {
        public Guid TopicId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public String Instructor { get; set; }
        public InstructionMedia InstructionMedia { get; set; }
        public TrainingStatus Status { get; set; }

        public RateBasisEnum RateBasis { get; set; }
        public decimal MemberFee { get; set; }
        public decimal NonMemberFee { get; set; }
        public string Schedule { get; set; }
        public Guid? ServiceTypeId { get; set; }
        public Guid? SurveyId { get; set; }
        public bool HasEvening { get; set; }
        public bool HasAfternoon { get; set; }
        public bool HasMorning { get; set; }

        public virtual ICollection<Attendance> Attendances { get; set; }
        public virtual ICollection<Assessment> Assessments { get; set; }
        public virtual ICollection<TrainingSponsor> TrainingSponsors { get; set; }
        public virtual ICollection<Registration> Registrations { get; set; }

        [NotMapped]
        public string ServiceCharge
        {
            get
            {
                if (RateBasis == RateBasisEnum.Adhoc)
                    return "Adhoc";
                return $"Members=[{MemberFee}]  Others [{NonMemberFee}]";
            }
        }

        public virtual Topic Topic { get; set; }

    }
}