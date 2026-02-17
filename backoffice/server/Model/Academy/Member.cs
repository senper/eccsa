using System;
using System.Collections.Generic;

namespace ECCSA.BackOffice.Server.Model.Academy
{
	public class Member
	{
		public Guid Id { get; set; }
		public String NameEng { get; set; }
		public String NameLocal { get; set; }
		public int MemberType { get; set; }
		public Guid MembershipId { get; set; }
		public int AddressType { get; set; }
		public Guid LocationId { get; set; }
		public String AddressLine { get; set; }
		public int ContactType { get; set; }
		public String Contact { get; set; }


		public virtual ICollection<NeedAssessmentDetail> NeedAssessmentDetails { get; set; }


		
	}
}