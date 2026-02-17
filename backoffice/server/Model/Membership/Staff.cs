using ECCSA.BackOffice.Server.Model.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECCSA.BackOffice.Server.Model.Membership
{
    public class Staff
    {
        public Guid Id { get; set; }

        public String Title { get; set; }
        public String NameEng { get; set; }
        public String NameLocal { get; set; }

        [NotMapped]
        public String Name
        {
            get
            {
                return $"{(string.IsNullOrEmpty(NameEng) ? string.Empty : NameEng)} {(string.IsNullOrEmpty(NameLocal) ? string.Empty : NameLocal)}";
            }
        }
        public Guid PositionTypeId { get; set; }
        public Guid AssociationId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public StatusEnum Status { get; set; }
        public virtual Association Association { get; set; }

        public virtual PositionType PositionType { get; set; }

        public string Email { get; set; }
        public string Mobile { get; set; }

        public byte[] Photo { get; set; }

        public string Extension { get; set; }
    }

   
}