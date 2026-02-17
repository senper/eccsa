using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Membership;
using System;

namespace ECCSA.BackOffice.Server.Persistence.DTO.Membership
{
    public record CreateBoardMemberInput (
		String NameEng
	, String NameLocal
	, Guid PositionTypeId
	, Guid AssociationId
	, DateTime StartDate
	, DateTime EndDate
	, StatusEnum Status
);


public record UpdateBoardMemberInput (
	Guid Id
	, String NameEng
	, String NameLocal
	, Guid PositionTypeId
	, Guid AssociationId
	, DateTime StartDate
	, DateTime EndDate
	, StatusEnum Status
);




    
    
    public record BoardMemberPayload(BoardMember PayLoad);
    
}
