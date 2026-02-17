using ECCSA.BackOffice.Server.Model.Membership;
using System;

namespace ECCSA.BackOffice.Server.Persistence.DTO.Membership
{
    public record CreateMembershipRequirementInput (
		Guid AssociationId
	, int DocumentTypeId
	, RequirementLevelEnum ApplicationRequirementLevel
	, RequirementLevelEnum RenewalRequirementLevel
);


public record UpdateMembershipRequirementInput (
	Guid Id
	, Guid AssociationId
	, int DocumentTypeId
	, RequirementLevelEnum ApplicationRequirementLevel
	, RequirementLevelEnum RenewalRequirementLevel
);




    
    
    public record MembershipRequirementPayload(MembershipRequirement PayLoad);
    
}
