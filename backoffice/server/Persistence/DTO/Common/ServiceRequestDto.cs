using ECCSA.BackOffice.Server.Model.Common.Servicing;
using System;

namespace ECCSA.BackOffice.Server.Persistence.DTO.Common
{
    public record CreateServiceRequestInput (
		Guid ServiceTypeId
	, String Description
);


public record UpdateServiceRequestInput (
	Guid Id
	, Guid ServiceTypeId
	, String Description
);




    
    
    public record ServiceRequestPayload(ServiceRequest PayLoad);
    
}
