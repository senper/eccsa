using System;

namespace ECCSA.BackOffice.Server.Model.Common
{
	public class SelectedChoice
	{
		public Guid ResponseId { get; set; }

		public Guid ChoiceId { get; set; }

		public int Rank { get; set; }

		public virtual SurveyResponse Response { get; set; }

		public virtual SurveyChoice Choice { get; set; }
	}
}