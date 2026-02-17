using ECCSA.BackOffice.Server.Model.Payment;
using System;

namespace ECCSA.BackOffice.Server.Persistence.DTO.Payment
{
    public record CreateDiscountCategoryInput (
		Decimal Rate
);


public record UpdateDiscountCategoryInput (
	int Id
	, Decimal Rate
);




    
    
    public record DiscountCategoryPayload(DiscountCategory PayLoad);
    
}
