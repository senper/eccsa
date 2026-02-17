using System;

namespace ECCSA.BackOffice.Server.Model.Membership
{
	public class Contact
	{
		public Guid Id { get; set; }
		public Guid MemberId { get; set; }
		public ContactTypeEnum ContactType { get; set; }
		public String Address { get; set; }
		public int Priority { get; set; }


		

		public virtual Stakeholder Stakeholder { get; set; }

	}
}