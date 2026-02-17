using ECCSA.BackOffice.Server.Model.Trade;
using System;

namespace ECCSA.BackOffice.Server.Persistence.DTO.Trade
{
    public record CreateCommodityPriceInput (
		DateTime MarketDate
	, Guid CommodityTypeId
	, Guid PriceUnitRateId
	, Decimal PriceValue
);


public record UpdateCommodityPriceInput (
	Guid Id
	, DateTime MarketDate
	, Guid CommodityTypeId
	, Guid PriceUnitRateId
	, Decimal PriceValue
);




    
    
    public record CommodityPricePayload(CommodityPrice PayLoad);
    
}
