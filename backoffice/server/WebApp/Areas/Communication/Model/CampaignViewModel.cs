using ECCSA.BackOffice.Server.Model.Communication;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Communication.Model
{
    public class CampaignViewModel : CommunicationViewModel<Campaign>
    {
        public string Controller => "Campaign";

        public List<MediaAgency> MediaAgencies = new List<MediaAgency>();



        public string Checked(Guid id)
        {
            if(TheModel.CampaignPartnerAgencies == null) return "";
            return TheModel.CampaignPartnerAgencies.Any(x => x.MediaAgencyId == id) ? "checked" : "";
        }
    }

   
}
