using ECCSA.BackOffice.Server.Model.Common;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Academy.Model
{
    public class AcademyDashboardViewModel : AcademyViewModel<string>
    {
        public int TopicsCount { get; set; }
        public int CategoriesCount { get; set; }
        public int TrainingsCount { get; set; }
        public int RegistrationsCount { get; set; }
        public SummaryModel Summary { get; set; }
        public string SummaryJson { get; set; }
        public string L1Json { get; set; }
        public string L2Json { get; set; }
    }
}
