using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Membership;
using System;

namespace ECCSA.BackOffice.Server.Persistence.DTO.Membership
{
	public record CreateAssociationInput (
		String NameEng
	, String NameLocal
	, String ShortName
	, String ShortName_Local
	, DateTime MemberSince
	, AssociationTypeEnum AssociationType
	, LevelEnum AssociationLevel
	, Guid ParentId
	, Guid LocationId
	, byte[] Logo
	, string Extension
	, StatusEnum Status
	, decimal RegistrationFee
	, decimal RenewalFee
				);


public record UpdateAssociationInput (
	Guid Id
	, String NameEng
	, String NameLocal
	, String ShortName
	, String ShortName_Local
	, DateTime MemberSince
	, AssociationTypeEnum AssociationType
	, LevelEnum AssociationLevel
	, Guid ParentId
	, Guid LocationId
	, byte[] Logo
	, string Extension
	, StatusEnum Status
	, decimal RegistrationFee
	, decimal RenewalFee
	);
	public record UpdateProfileInput(
		Guid Id
		, String Profile
		);





	public record AssociationPayload(Association PayLoad);
	
}
