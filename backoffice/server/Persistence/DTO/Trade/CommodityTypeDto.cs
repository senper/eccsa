using ECCSA.BackOffice.Server.Model.Trade;
using System;

namespace ECCSA.BackOffice.Server.Persistence.DTO.Trade
{
    public record CreateCommodityTypeInput (
		String Name
	, String NameLocal
);


public record UpdateCommodityTypeInput (
	Guid Id
	, String Name
	, String NameLocal
);




    
    
    public record CommodityTypePayload(CommodityType PayLoad);
    
}
