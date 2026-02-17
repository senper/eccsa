using ECCSA.BackOffice.Server.Model.Common;
using System;

namespace ECCSA.BackOffice.Server.Model.Communication
{
	public class MediaCoverage : Entity
	{
		public Guid MediaAgencyId { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public int DurationMinutes { get; set; }
		public String Frequency { get; set; }
		public String CoveredIn { get; set; }
		public String About { get; set; }
		public String Remark { get; set; }

		public String Extension { get; set; }
		public virtual MediaAgency MediaAgency { get; set; }

	}
}