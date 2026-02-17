using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Common.Notification;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECCSA.BackOffice.Server.Model.Communication 
{
	public class Campaign : Entity
	{
		public String Title { get; set; }
		public String Description { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public String Purpose { get; set; }
		public CampaignStatusEnum Status { get; set; }


		public virtual ICollection<CampaignPartnerAgency> CampaignPartnerAgencies { get; set; }

		[NotMapped]
		public List<EmailMessage> Emails { get; set; } = new List<EmailMessage>();

        public string Extension { get; set; } = string.Empty;
    }
}