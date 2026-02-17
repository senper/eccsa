using ECCSA.BackOffice.Server.Model.Membership;
using System;

namespace ECCSA.BackOffice.Server.Persistence.DTO.Membership
{
    public record CreateLegalDocumentTypeInput (
		String NameEng
	, String NameLocal
	, Boolean HasExpiry
	, Boolean AllowMultiple
);


public record UpdateLegalDocumentTypeInput (
	int Id
	, String NameEng
	, String NameLocal
	, Boolean HasExpiry
	, Boolean AllowMultiple
);




    
    
    public record LegalDocumentTypePayload(LegalDocumentType PayLoad);
    
}
