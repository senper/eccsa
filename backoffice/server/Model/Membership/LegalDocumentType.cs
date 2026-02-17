using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECCSA.BackOffice.Server.Model.Membership
{
    public class LegalDocumentType
	{
		public int Id { get; set; }

		[NotMapped]
		public String Name
		{
			get
			{
				return $"{(string.IsNullOrEmpty(NameEng) ? string.Empty : NameEng)} {(string.IsNullOrEmpty(NameLocal) ? string.Empty : NameLocal)}";
			}
		}
		public String NameEng { get; set; }
		public String NameLocal { get; set; }
		public bool HasExpiry { get; set; }
		public bool AllowMultiple { get; set; }

		

		

	}
}