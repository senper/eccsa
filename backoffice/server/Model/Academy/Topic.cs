using ECCSA.BackOffice.Server.Model.Common;
using System;
using System.Collections.Generic;

namespace ECCSA.BackOffice.Server.Model.Academy
{
	public class Topic : Entity
	{
		public String Code { get; set; }
		public String NameEng { get; set; }
		public String NameLocal { get; set; }
		public int CategoryId { get; set; }
		public String Decription { get; set; }
		public String DecriptionLocal { get; set; }
		public int Status { get; set; }
		public bool IsPublic { get; set; } = true;		

		public virtual ICollection<LearningResource> Resources { get; set; }
		public virtual ICollection<TopicSetting> TopicSettings { get; set; }
		public virtual ICollection<Training> Trainings { get; set; }


		public virtual Category Category { get; set; }

	}
}