using System;

namespace ECCSA.BackOffice.Server.Model.Common
{
    public class SurveyAudience
	{
		public Guid Id { get; set; }

		public Guid SurveyId { get; set; }

		public string Email { get; set; }
		public string MobileNo { get; set; }
		public Guid UserId { get; set; }



		public virtual Survey Survey { get; set; }
	}
}