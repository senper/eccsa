using ECCSA.BackOffice.Server.Model.Communication;
using System;

namespace ECCSA.BackOffice.Server.Persistence.DTO.Communication
{
    public record CreateCampaignInput (
		String Title
	, String TitleLocal
	, String Description
	, DateTime StartDate
	, DateTime EndDate
	, String Purpose
	, CampaignStatusEnum Status
				);


public record UpdateCampaignInput (
	Guid Id
	, String Title
	, String TitleLocal
	, String Description
	, DateTime StartDate
	, DateTime EndDate
	, String Purpose
	, CampaignStatusEnum Status
				);




    
    
    public record CampaignPayload(Campaign PayLoad);
    
}
