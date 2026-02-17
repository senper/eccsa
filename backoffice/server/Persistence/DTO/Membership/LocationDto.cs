using ECCSA.BackOffice.Server.Model.Membership;
using System;

namespace ECCSA.BackOffice.Server.Persistence.DTO.Membership
{
    public record CreateLocationInput (
		String Code
	, String NameEng
	, String NameLocal
	, Guid ParentId
	, int Level
	,Guid? Level1Id
	, String Level1NameEng
	, String Level1NameLocal
	,Guid? Level2Id
	, String Level2NameEng
	, String Level2NameLocal
);


public record UpdateLocationInput (
	Guid Id
	, String Code
	, String NameEng
	, String NameLocal
	, Guid ParentId
	, int Level
	,Guid? Level1Id
	, String Level1NameEng
	, String Level1NameLocal
	,Guid? Level2Id
	, String Level2NameEng
	, String Level2NameLocal
);




    
    
    public record LocationPayload(Location PayLoad);
    
}
