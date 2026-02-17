using System;

namespace ECCSA.BackOffice.Server.Model.Membership
{
    public class RepresentativeContact
    {
        public Guid Id { get; set; }
		public Guid RepresentativeId { get; set; }
		public int ContactType { get; set; }
		public String Address { get; set; }
		public int Priority { get; set; }


        

        public virtual Representative Representative { get; set; }

    }
}