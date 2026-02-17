using System;

namespace ECCSA.BackOffice.Server.Model.Common.Planning
{
    public class Progress
    {
        public Guid Id { get; set; }

        public Guid PlanId { get; set; }

        public virtual Plan Plan { get; set; }

        public DateTime ReportingDate { get; set; } = DateTime.Today;

        public int ReportNo { get; set; } = 1;

        public decimal Physical { get; set; }

        public decimal Financial { get; set; }

    }
}