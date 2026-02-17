using ECCSA.BackOffice.Server.Model.Common;
using System;

namespace ECCSA.BackOffice.Server.Model.Communication
{
    public class CampaignPartnerAgency : Entity
	{
		public Guid CampaignId { get; set; }
		public Guid MediaAgencyId { get; set; }
		public int Status { get; set; }
		public int CommunicatedThrough { get; set; }
		public virtual Campaign Campaign { get; set; }
		public virtual MediaAgency MediaAgency { get; set; }

	}
}