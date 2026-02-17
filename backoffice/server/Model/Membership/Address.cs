using System;


namespace ECCSA.BackOffice.Server.Model.Membership
{
	public class Address
	{
		public Guid Id { get; set; }
		public Guid MemberId { get; set; }
		public int AddressType { get; set; }
		public Guid LocationId { get; set; }
		public String AddressLine { get; set; }


		

		public virtual Stakeholder Stakeholder { get; set; }

	}
}