using ECCSA.BackOffice.Server.Model.Common.Servicing;
using System;

namespace ECCSA.BackOffice.Server.Persistence.DTO.Common
{
    public record CreateServiceTypeInput (
		String NameEng
		, String NameLocal
		, String DescriptionEng
		, String DescriptionLocal
	);


public record UpdateServiceTypeInput (
	Guid Id
	, String NameEng
	, String NameLocal
	, String DescriptionEng
	, String DescriptionLocal
);




	
	
	public record ServiceTypePayload(ServiceType PayLoad);
	
}
