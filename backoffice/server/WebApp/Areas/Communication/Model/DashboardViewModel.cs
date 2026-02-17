using ECCSA.BackOffice.Server.Model.Common;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Communication.Model
{
    public class DashboardViewModel : CommunicationViewModel<string>
    {
        public string Controller => "Dashboard";
        public int Publications { get; set; }
        public int Medias { get; set; }
        public int MonthlyCoverage { get; set; }
        public int AnnualCoverage { get; set; }

        public int MonthlyCampaings { get; set; }
        public int AnnualCampaings { get; set; }
        public SummaryModel Summary { get; set; }
        public string SummaryJson { get; set; }

        public SummaryModel Summary2 { get; set; }
        public string SummaryJson2 { get; set; }
    }
}
