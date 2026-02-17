using ECCSA.BackOffice.Server.Model.Communication;
using System;

namespace ECCSA.BackOffice.Server.Persistence.DTO.Communication
{
    public record CreateCampaignPartnerAgencyInput (
		Guid CampaignId
	, Guid MediaAgencyId
	, int Status
	, int CommunicatedThrough
				);


public record UpdateCampaignPartnerAgencyInput (
	Guid Id
	, Guid CampaignId
	, Guid MediaAgencyId
	, int Status
	, int CommunicatedThrough
				);




    
    
    public record CampaignPartnerAgencyPayload(CampaignPartnerAgency PayLoad);
    
}
