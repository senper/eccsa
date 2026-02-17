using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Membership;
using System;

namespace ECCSA.BackOffice.Server.Persistence.DTO.Membership
{
    public record CreateStakeholderInput (
		StakeholderTypeEnum StakeholderType
	, StatusEnum Status
);


public record UpdateStakeholderInput (
	Guid Id
	, StakeholderTypeEnum StakeholderType
	, StatusEnum Status
);




    
    
    public record StakeholderPayload(Stakeholder PayLoad);
    
}
