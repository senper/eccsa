using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Membership;
using System;

namespace ECCSA.BackOffice.Server.Persistence.DTO.Membership
{
    public record CreateRenewalHistoryInput(
        Guid EnrollmentId

);

    public record RenewalInvoiceInput(
        Guid Id
        , Decimal Fee
        , String InvoiceNo
    );

    public record RenewalPaymentInput(
    Guid Id
    , String ReceiptNo
    , PaymentMethodEnum PaymentMethod

);

    public record ApproveRejectRenewalInput(
    Guid Id
    , MembershipWorkflowEnum RenewalStatus
    , String Remark
);

    public record UpdateRenewalHistoryInput(
    Guid Id
    , Guid EnrollmentId
    , Decimal Fee
    , String InvoiceNo
    , DateTime InvoiceDate
    , String ReceiptNo
    , DateTime PaymentDate
    , PaymentMethodEnum PaymentMethod
    , MembershipWorkflowEnum RenewalStatus
    , String Remark
);





    public record RenewalHistoryPayload(RenewalHistory PayLoad);

}
