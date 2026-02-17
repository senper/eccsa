using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECCSA.BackOffice.Server.Model.Common
{
    public class SurveyRespondent : Entity
	{

		public Guid SurveyId { get; set; }

		//TODO: will be removed once the UI is migrated to UserId
		[NotMapped]
		public Guid RespondentStakeholderId
        {
            get { return UserId; }
			set { UserId = value; }
		}

		public Guid UserId { get; set; }

		[NotMapped]
		public string UserCode => UserId.ToString()[^10..];

		public DateTime StartDate { get; set; }

		public DateTime EndDate { get; set; }
		public String Remark { get; set; }

		public ICollection<SurveyResponse> Responses { get; set; }

		public virtual Survey Survey { get; set; }
	}
}