using ECCSA.BackOffice.Server.Model.Trade;
using System;

namespace ECCSA.BackOffice.Server.Persistence.DTO.Trade
{
	public record CreateExchangeRateInput (
		DateTime TransactionDate
	, String Currency
	, Decimal Buying
	, Decimal Selling
);


public record UpdateExchangeRateInput (
	Guid Id
	, DateTime TransactionDate
	, String Currency
	, Decimal Buying
	, Decimal Selling
);




	
	
	public record ExchangeRatePayload(ExchangeRate PayLoad);
	
}
