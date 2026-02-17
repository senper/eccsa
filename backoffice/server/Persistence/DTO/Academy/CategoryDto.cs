using ECCSA.BackOffice.Server.Model.Academy;
using System;

namespace ECCSA.BackOffice.Server.Persistence.DTO.Academy
{
    public record CreateCategoryInput (
		String Code
	, String NameEng
	, String NameLocal
	, int? ParentId
	, int Status
);


public record UpdateCategoryInput (
	int Id
	, String Code
	, String NameEng
	, String NameLocal
	, int? ParentId
	, int Status
);




    
    
    public record CategoryPayload(Category PayLoad);
    
}
