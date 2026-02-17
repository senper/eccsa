using System;
using System.Collections.Generic;

namespace ECCSA.BackOffice.Server.Model.Academy
{
    public class NeedAssessment
    {
        public Guid Id { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public String Remark { get; set; }
		public int Status { get; set; }


        public virtual ICollection<NeedAssessmentDetail> NeedAssessmentDetails { get; set; }


        
    }
}