using ECCSA.BackOffice.Server.Model.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECCSA.BackOffice.Server.Model.Membership
{
	public class PositionType
	{
		public Guid Id { get; set; }

		public String OrderNo { get; set; }
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
		public bool ForBoard { get; set; }
		public bool ForStaff { get; set; }
		
		public StatusEnum Status { get; set; }


		public ICollection<BoardMember> BoardMembers { get; set; }

		public ICollection<Staff> Employees { get; set; }

		
	}

	
}