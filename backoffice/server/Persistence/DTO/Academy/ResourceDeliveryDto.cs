using ECCSA.BackOffice.Server.Model.Academy;
using System;

namespace ECCSA.BackOffice.Server.Persistence.DTO.Academy
{
    public record CreateResourceDeliveryInput (
		DeliveryMethod DeliveryMethod
	, Guid ResourceId
	, Guid RegistrationId
	, DateTime DeliveryDate
);


public record UpdateResourceDeliveryInput (
	Guid Id
	, DeliveryMethod DeliveryMethod
	, Guid ResourceId
	, Guid RegistrationId
	, DateTime DeliveryDate
);




    
    
    public record ResourceDeliveryPayload(ResourceDelivery PayLoad);
    
}
