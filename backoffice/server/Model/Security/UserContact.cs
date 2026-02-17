using ECCSA.BackOffice.Server.Model.Membership;
using System;

namespace ECCSA.BackOffice.Server.Model.Security
{
    public class UserContact
    {
		public Guid Id { get; set; }
		public Guid UserId { get; set; }
		public ContactTypeEnum ContactType { get; set; }
		public String Address { get; set; }
		public int Priority { get; set; }
	}
}
