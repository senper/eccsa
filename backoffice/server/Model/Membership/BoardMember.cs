using ECCSA.BackOffice.Server.Model.Common;
using System;

namespace ECCSA.BackOffice.Server.Model.Membership
{
    public class BoardMember
    {
        public Guid Id { get; set; }
        public String NameEng { get; set; }
        public String NameLocal { get; set; }
        public Guid PositionTypeId { get; set; }
        public Guid AssociationId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public StatusEnum Status { get; set; }
        public virtual Association Association { get; set; }

        public virtual PositionType PositionType { get; set; }

    }

   
}