using ECCSA.BackOffice.Server.Model.Payment;
using ECCSA.BackOffice.Server.Model.Common;
using System;

namespace ECCSA.BackOffice.Server.Persistence.DTO.Payment
{
	public record CreateInvoiceInput (
		ECCSAModuleFlag SubSystem
	, Guid ReferenceId
	, Boolean IsCreditService
	, int CreditDueDays
		, String CurrencyId
	, Decimal BasePrice
	, Decimal Discount
	, VATOptionEnum VATOption
	, Decimal NetAmount
	, Boolean Cancelled
	, Guid CancelledBy
);

	public record CreateSimpleInvoiceInput(
		ECCSAModuleFlag SubSystem
	, Guid ReferenceId
	, Decimal BasePrice
);


	public record UpdateInvoiceInput (
	Guid Id
	, ECCSAModuleFlag SubSystem
	, Guid ReferenceId
	, Boolean IsCreditService
	, int CreditDueDays
		, String CurrencyId
	, Decimal BasePrice
	, Decimal Discount
	, VATOptionEnum VATOption
	, Decimal NetAmount
);


	public record CancelInvoiceInput(
	Guid Id
	, ECCSAModuleFlag SubSystem
	, Guid ReferenceId
	, Guid CancelledBy
);



	public record InvoicePayload(Invoice PayLoad);
	
}
