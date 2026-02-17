using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Common.Servicing;
using Microsoft.AspNetCore.Http;
using System;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Service.Model
{
    public class ChangeStatusModel
    {
        public Guid Id { get; set; }
        public IFormFile File { get; set; }

        public WorkflowStateEnum Status { get; set; }

        public String Remark { get; set; }

        public string Code { get; set; }
        public string Extension { get; set; }

        public SendEmailEnum SendEmail { get; set; }
    }

    public class ConfirmPaymentModel
    {
        public Guid Id { get; set; }
        public Guid RequestId { get; set; }
        public decimal ConfirmedAmount { get; set; }
        public string InvoiceNo { get; set; }
        public string ReceiptNo { get; set; }
        
    }
    public enum SendEmailEnum
    {
        No = 0,
        WithAttachment = 1,
        NoAttachment = 2
    }
}
