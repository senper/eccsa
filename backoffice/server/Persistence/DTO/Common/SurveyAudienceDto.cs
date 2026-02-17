using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Membership;
using System;

namespace ECCSA.BackOffice.Server.Persistence.DTO.Common
{
    public record CreateSurveyAudienceInput (
		Guid SurveyId
	, StakeholderTypeEnum StakeHolderType
	, AssociationTypeEnum AssociationType
	, Boolean National
	, Boolean Regional
	, Boolean Woreda
	, Boolean IncludeMembers
);


public record UpdateSurveyAudienceInput (
	Guid Id
	, Guid SurveyId
	, StakeholderTypeEnum StakeHolderType
	, AssociationTypeEnum AssociationType
	, Boolean National
	, Boolean Regional
	, Boolean Woreda
	, Boolean IncludeMembers
);




    
    
    public record SurveyAudiencePayload(SurveyAudience PayLoad);
    
}
