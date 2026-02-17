using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ECCSA.BackOffice.Server.Model.Common
{
	public class SurveyQuestion
	{
		public Guid Id { get; set; }

		public Guid SurveySettingId { get; set; }
		public string OrderNo { get; set; }
		public String  Question { get; set; }

		public QuestionType QuestionType { get; set; }
		[NotMapped]
		public int QuestionTypeValue => (int)QuestionType;

		public decimal? MinRange { get; set; }
		public decimal? MaxRange { get; set; }

		[NotMapped]
		public List<SimpleChoice> SimpleChoices { get; set; }

		[JsonIgnore]
		public virtual ICollection<SurveyChoice> Choices { get; set; }
		[JsonIgnore]
		public virtual ICollection<SurveyVote> Votes { get; set; }

		public bool Cascaded { get; set; }

		public Guid? ParentChoiceId { get; set; }

		[NotMapped]
		public String ParentText { get; set; }

		[JsonIgnore]
		public virtual SurveySetting Setting { get; set; }
		[NotMapped]
		public SurveyResponse Response { get; set; }
		[NotMapped]
		public Guid? ParentQuestionId { get; set; }
    }
}