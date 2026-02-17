using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ECCSA.BackOffice.Server.Model.Common
{

    public class SimpleChoice
	{

		public Guid Id { get; set; }

		public Guid SurveySettingId { get; set; }
		public string OrderNo { get; set; }
		public Guid QuestionId { get; set; }

		[NotMapped]
		public String QuestionText { get; set; }

		public String ChoiceText { get; set; }

		[NotMapped]
		public bool Referenced { get; set; }

	
	}
	public class SurveyChoice : SimpleChoice
	{
		

		[JsonIgnore]
		public virtual SurveyQuestion  Question { get; set; }
		
		[JsonIgnore]
		public virtual SurveySetting  SurveySetting { get; set; }
		
		[JsonIgnore]
		public ICollection<SelectedChoice> Responses { get; set; }
		[JsonIgnore]
		public ICollection<SurveyVote> Votes { get; set; }
	}
}