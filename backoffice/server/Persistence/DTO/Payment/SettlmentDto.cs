using ECCSA.BackOffice.Server.Model.Payment;
using ECCSA.BackOffice.Server.Model.Common;
using System;

namespace ECCSA.BackOffice.Server.Persistence.DTO.Payment
{
    public record CreateSettlmentInput (
		Guid InvoiceId
	, String ReceiptNo
	, DateTime PaymentDate
	, PaymentMethodEnum PaymentMethod
	, Decimal AmountPaid
	, String Remark
		, DateTime CreatedOn
);


public record UpdateSettlmentInput (
	Guid Id
	, Guid InvoiceId
	, String ReceiptNo
	, DateTime PaymentDate
	, PaymentMethodEnum PaymentMethod
	, Decimal AmountPaid
	, String Remark
		, DateTime CreatedOn
);




    
    
    public record SettlmentPayload(Settlment PayLoad);
    
}
