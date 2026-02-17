using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECCSA.BackOffice.Server.Model.Common
{


	public class Survey : Entity
	{
		public Guid SettingId { get; set; }

		public String Title { get; set; }

		public String Description { get; set; }
		public String GroupId { get; set; }
		public DateTime StartDate { get; set; }

		public DateTime EndDate { get; set; }

		public String AnalysisDescription { get; set; }

		public String Attachment { get; set; }

		public virtual SurveySetting Setting { get; set; }
		
		public virtual ICollection<SurveyAudience> Audiences { get; set; } = new List<SurveyAudience>();

		public virtual ICollection<Attachment> Attachments { get; set; }

		public virtual ICollection<SurveyRespondent> Respondents { get; set; }

		public virtual ICollection<SurveyResponse> Responses { get; set; }
		public virtual ICollection<SurveyVote> Votes { get; set; }
		public DateTime? TelegramPublishDate { get; set; }
		public List<String> PollIds { get; set; }

		[NotMapped]
		public string ResponseCount
        {
			get
            {
				if(Respondents == null) return  string.Empty;
				var count = Respondents.Count;
				if (count > 0)
					return count.ToString();
				return string.Empty;
            }
        }
		[NotMapped]
		public string AttachmentsCount
		{
			get
			{
				if (Attachments == null) return string.Empty;
				var count = Attachments.Count;
				if (count > 0)
					return count.ToString();
				return string.Empty;
			}
		}
	}
	public class Attachment : Entity
	{
		public string Title { get; set; }
		public string Description { get; set; }
		public string Extension { get; set; }
		public Guid SurveyId { get; set; }
		public virtual Survey Survey { get; set; }
	}
}