using ECCSA.BackOffice.Server.Model.Trade;
using System;

namespace ECCSA.BackOffice.Server.Persistence.DTO.Trade
{
    public record CreatePriceUnitRateInput (
		String Name
	, String NameLocal
);


public record UpdatePriceUnitRateInput (
	Guid Id
	, String Name
	, String NameLocal
);




    
    
    public record PriceUnitRatePayload(PriceUnitRate PayLoad);
    
}
