using ECCSA.BackOffice.Server.Model.Security;
using System;

namespace ECCSA.BackOffice.Server.Persistence.DTO.Security
{
    public record CreateRoleInput (
		String Name
);


public record UpdateRoleInput (
	Guid Id
	, String Name
);




    
    
    public record RolePayload(Role payLoad);
    
}
