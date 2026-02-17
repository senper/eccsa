using ECCSA.BackOffice.Server.Model.Advocacy;
using System.Collections.Generic;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Advocacy.Model
{
    public class ResearchViewModel : AdvocacyViewModel<Research>
    {
        public string Controller => "Research";
        public ResearchViewModel()
        {
            CurrentPage = Controller;
        }
    }
    public class DialogViewModel : AdvocacyViewModel<Dialog>
    {
        public string Controller => "Dialog";
        public DialogViewModel()
        {
            CurrentPage = Controller;
        }
    }
    public class DialogMeetingViewModel : AdvocacyViewModel<PPDMeeting>
    {
        public string Controller => "Dialog";
        public DialogMeetingViewModel()
        {
            CurrentPage = Controller;
        }
        public Dialog TheDialog { get; set; }
        public List<Recommendation> Issues { get; set; }
    }
    public class PublicationViewModel : AdvocacyViewModel<Publication>
    {
        public string Controller => "Publication";
        public PublicationViewModel()
        {
            CurrentPage = Controller;
        }
    }
    public class ProjectViewModel : AdvocacyViewModel<Project>
    {
        public string Controller => "Project";
        public ProjectViewModel()
        {
            CurrentPage = Controller;
        }
    }
}
