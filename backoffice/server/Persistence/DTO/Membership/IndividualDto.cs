using ECCSA.BackOffice.Server.Model.Membership;
using System;

namespace ECCSA.BackOffice.Server.Persistence.DTO.Membership
{
	public record CreateIndividualInput (
		String NameEng
	, String NameLocal
	, DateTime MemberSince
	, String TaxIdNumber
	, String BusinessSector
	, Guid LocationId
	, String TradeRegistrationNumber
	, Guid AssociatedUserId
				);


public record UpdateIndividualInput (
	Guid Id
	, String NameEng
	, String NameLocal
	, DateTime MemberSince
	, String TaxIdNumber
	, Guid LocationId
	, String TradeRegistrationNumber
	, Guid AssociatedUserId
				);



	


	public record IndividualPayload(Individual PayLoad);
	
}
