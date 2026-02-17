using ECCSA.BackOffice.Server.Model.Academy;
using System;

namespace ECCSA.BackOffice.Server.Persistence.DTO.Academy
{
    public record CreateSponsorInput (
		String NameEng
	, String NameLocal
		, String Email
		, String Mobile
	, int Status
);


public record UpdateSponsorInput (
	Guid Id
	, String NameEng
	, String NameLocal
		, String Email
		, String Mobile
	, int Status
);




    
    
    public record SponsorPayload(Sponsor PayLoad);
    
}
