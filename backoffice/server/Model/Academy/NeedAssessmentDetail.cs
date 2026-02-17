using System;

namespace ECCSA.BackOffice.Server.Model.Academy
{
    public class NeedAssessmentDetail
	{
		public Guid Id { get; set; }
		public int CategoryId { get; set; }
		public Guid MemberId { get; set; }
		public Guid AssessmentId { get; set; }
		public int NeedRating { get; set; }
		public DateTime BestBefore { get; set; }
		public String Remark { get; set; }
		public int Status { get; set; }

		public virtual Category Category { get; set; }
		public virtual NeedAssessment NeedAssessment { get; set; }
		public virtual Member Member { get; set; }

	}
}