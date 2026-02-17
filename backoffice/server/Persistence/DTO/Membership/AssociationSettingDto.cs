using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Membership;
using System;

namespace ECCSA.BackOffice.Server.Persistence.DTO.Membership
{
	public record CreateAssociationSettingInput (
		Guid AssociationId
	, String BankName
	, String AccountNumber
	, ECCSAModuleFlag SubSystem
);


public record UpdateAssociationSettingInput (
	Guid Id
	, Guid AssociationId
	, String BankName
	, String AccountNumber
	, ECCSAModuleFlag SubSystem
);




	
	
	public record AssociationSettingPayload(AssociationSetting PayLoad);
	
}
