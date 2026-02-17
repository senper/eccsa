using ECCSA.BackOffice.Server.Model.Arbitration;
using ECCSA.BackOffice.Server.Model.Common;
using System;

namespace ECCSA.BackOffice.Server.Persistence.DTO.Arbitration
{
    public record CreateArbitrationServiceInput (
	//	Guid AssociationId
	//, Guid RequestingBodyId
	string TIN 
	, String FullName 
	, String EmailAddress 
	, String MobileNo 
	, Guid ServiceTypeId
	, DateTime RequestDate
	//, String InvoiceNo
	//, String ReceiptNo
	//, PaymentMethodEnum PaymentMethod
	//, WorkflowStateEnum RequestStatus
	, String Description
				);

public record UpdateArbitrationServiceInput (
	Guid Id
	//, Guid AssociationId
	//, Guid RequestingBodyId
	, string TIN
	, String FullName
	, String EmailAddress
	, String MobileNo
	, Guid ServiceTypeId
	, DateTime RequestDate
	//, String InvoiceNo
	//, String ReceiptNo
	//, PaymentMethodEnum PaymentMethod
	//, WorkflowStateEnum RequestStatus
	, String Description
				);

	public record UpdateArbitrationServicePaymentInput(
	Guid Id
	, String ReceiptNo
	, PaymentMethodEnum PaymentMethod
				);
	public record UpdateArbitrationServiceStatusInput(
	Guid Id
	, WorkflowStateEnum status
				);

	public record ArbitrationServicePayload(ArbitrationService PayLoad);
    
}
