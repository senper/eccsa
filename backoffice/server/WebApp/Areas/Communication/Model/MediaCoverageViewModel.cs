using ECCSA.BackOffice.Server.Model.Communication;
using System.Collections.Generic;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Communication.Model
{
    public class MediaCoverageViewModel : CommunicationViewModel<MediaCoverage>
    {
        public string Controller => "MediaCoverage";

        public List<MediaAgency> Agencies { get; set; } = new List<MediaAgency>();
    }
}
