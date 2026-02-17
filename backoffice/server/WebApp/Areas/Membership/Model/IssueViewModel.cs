using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Membership;
using System;
using System.Collections.Generic;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Membership.Model
{
    public class IssueViewModel : MembershipViewModel<Issue>
    {
        public IssueViewModel()
        {
            CurrentPage = "Issue";
        }
        public IssueStatus Status { get; set; }
        public Guid IssueTypeId { get; set; }
        public List<IssueType> IssueTypes { get; set; } = new List<IssueType>();
        public string Controller => "Issue";
    }

    public class IssueReportViewModel : MembershipViewModel<IssueReport>
    {
        public IssueReportViewModel()
        {
            CurrentPage = "Issue Report";
        }
        public List<Issue> Issues { get; set; } = new List<Issue>();
        public IssueStatus Status { get; set; }
        public Guid IssueTypeId { get; set; }
        public List<IssueType> IssueTypes { get; set; } = new List<IssueType>();
        public string ReportModel { get; set; }
        public string Controller => "Issue";
        public DateTime FromDate { get; set; } = DateTime.Today.AddYears(-1);
    }
}
