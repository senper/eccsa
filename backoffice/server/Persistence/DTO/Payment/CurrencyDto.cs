using ECCSA.BackOffice.Server.Model.Payment;
using System;

namespace ECCSA.BackOffice.Server.Persistence.DTO.Payment
{
    public record CreateCurrencyInput (
		String Name
);


public record UpdateCurrencyInput (
	String Id
	, String Name
);




    
    
    public record CurrencyPayload(Currency PayLoad);
    
}
