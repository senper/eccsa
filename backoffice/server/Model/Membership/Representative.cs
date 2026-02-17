using ECCSA.BackOffice.Server.Model.Common;
using System;
using System.Collections.Generic;

namespace ECCSA.BackOffice.Server.Model.Membership
{
    public class Representative
    {
        public Guid Id { get; set; }
        public Guid MemberId { get; set; }

        public string OrderNo { get; set; }
        public String Title { get; set; }
        public String NameEng { get; set; }
        public String NameLocal { get; set; }
        public StatusEnum Status { get; set; }
        public String Position { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }


        public virtual ICollection<RepresentativeContact> RepresentativeContacts { get; set; }


        public virtual Stakeholder Stakeholder { get; set; }

    }
}