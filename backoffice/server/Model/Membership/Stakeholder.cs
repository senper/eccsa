using ECCSA.BackOffice.Server.Model.Common;
using System;
using System.Collections.Generic;

namespace ECCSA.BackOffice.Server.Model.Membership
{
    public class Stakeholder
    {
        public Guid Id { get; set; }
        public StakeholderTypeEnum StakeholderType { get; set; }
        public StatusEnum Status { get; set; }

        public virtual Individual Individual { get; set; } 

        public virtual Association Association { get; set; }
        public virtual ICollection<Contact> Contacts { get; set; }
        public virtual ICollection<Representative> Representatives { get; set; }
        public virtual ICollection<Address> Addresses { get; set; }
        public virtual ICollection<MembershipEnrollment> MembershipEnrollments { get; set; }

    }
}