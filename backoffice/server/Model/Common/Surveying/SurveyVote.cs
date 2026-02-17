using System;
using System.Text.Json.Serialization;

namespace ECCSA.BackOffice.Server.Model.Common
{
    public class SurveyVote
    {
		public Guid Id { get; set; }
		public Guid SurveyId { get; set; }
		[JsonIgnore]
		public virtual Survey Survey { get; set; }
		public Guid QuestionId { get; set; }
		[JsonIgnore]
		public virtual SurveyQuestion Question { get; set; }
		public Guid ChoiceId { get; set; }
		[JsonIgnore]
		public virtual SurveyChoice Choice { get; set; }
		public long Votes { get; set; }

	}
}