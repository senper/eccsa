using ECCSA.BackOffice.Server.Model.Security;
using System;

namespace ECCSA.BackOffice.Server.Persistence.DTO.Security
{
    public record CreateUserInput (
		String UserName
	, String Email
	, String PasswordHash
	, String SecurityStamp
	, String PhoneNumber
	, Boolean EmailConfirmed
	, Boolean PhoneNumberConfirmed
	, Boolean TwoFactorEnabled
	, Boolean LockoutEnabled
	, int AccessFailedCount
	, DateTime LockoutEndDateUtc
);


public record UpdateUserInput (
	Guid Id
	, String UserName
	, String Email
	, String PasswordHash
	, String SecurityStamp
	, String PhoneNumber
	, Boolean EmailConfirmed
	, Boolean PhoneNumberConfirmed
	, Boolean TwoFactorEnabled
	, Boolean LockoutEnabled
	, int AccessFailedCount
	, DateTime LockoutEndDateUtc
);




    
    
    public record UserPayload(User payLoad);
    
}
