using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ECCSA.BackOffice.Server.Model.Common
{
	public class SurveySetting : Entity
	{
		public String Title { get; set; }
		public String Description { get; set; }

		public ECCSAModuleFlag ResponsibleUnit { get; set; }


		public FrequencyEnum Recurrence { get; set; }

		public int OpenDays { get; set; }

		public MediaOutletFlag PublishThrough { get; set; }

		public ICollection<SurveyQuestion> Questions { get; set; }

		public ICollection<Survey> Surveys { get; set; }
		[NotMapped]
		public Survey CurrentSurvey => Surveys?.FirstOrDefault(s => s.StartDate <= DateTime.Today && s.EndDate >= DateTime.Today);
		[NotMapped]
		public Survey SelectedSurvey { get; set; }

		public virtual ICollection<SurveyChoice> Choices { get; set; }

		public StatusEnum Status { get; set; }

		public bool IsPublic { get; set; } = true;

		[NotMapped]
		public int ResponsesCount { get; set; }
		[NotMapped]
		public int QuestionsCount { get; set; }
	}


}