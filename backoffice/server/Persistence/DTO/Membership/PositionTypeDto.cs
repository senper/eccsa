using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Membership;
using System;

namespace ECCSA.BackOffice.Server.Persistence.DTO.Membership
{
    public record CreatePositionTypeInput (
		String OrderNo
	, String NameEng
	, String NameLocal
	, Boolean ForBoard
	, Boolean ForStaff
	, StatusEnum Status
);


public record UpdatePositionTypeInput (
	Guid Id
	, String OrderNo
	, String NameEng
	, String NameLocal
	, Boolean ForBoard
	, Boolean ForStaff
	, StatusEnum Status
);




    
    
    public record PositionTypePayload(PositionType PayLoad);
    
}
