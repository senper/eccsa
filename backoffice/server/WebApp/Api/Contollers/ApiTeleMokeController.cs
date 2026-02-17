using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Servicing;
using ECCSA.BackOffice.Server.Persistence.Repositories.Payment;
using ECCSA.BackOffice.Server.Persistence.Repositories.Security;
using ECCSA.BackOffice.Server.WebApp.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Notification;

namespace ECCSA.BackOffice.Server.WebApp.Api.Contollers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ApiTeleMokeController : BaseController
    {
        private readonly ServiceRequestRepository _serviceRequestRepository;
        private readonly InvoiceRepository _invoiceRepository;
        public ApiTeleMokeController(ServiceRequestRepository serviceRequestRepository,
             InvoiceRepository invoiceRepository,
            UserManager<User> userManager,
            PublicUserRepository userRepository, NotificationRepository notificationRepository) : base(userManager, userRepository, notificationRepository)
        {

            _serviceRequestRepository = serviceRequestRepository;
            _invoiceRepository = invoiceRepository;
        }
        [HttpPost]
        public async Task<IActionResult> PayNotify([FromBody] NotifyModel model)
        {
            Request.Headers.TryGetValue("Origin", out var values);
            if (string.IsNullOrWhiteSpace(model.data.outTradeNo) || string.IsNullOrWhiteSpace(model.data.tradeNo))
            {
                return Ok(new
                {
                    code = 103013,
                    msg = "The transactionNo or outTradeNo of request parameters cannot be null"
                });
            }
            Server.Model.Payment.Invoice invoice = await _invoiceRepository.InvoiceByOutTradeNo(model.data.outTradeNo);
            if (invoice == null)
            {
                return Ok(new
                {
                    code = 103014,
                    msg = "The transactionNo or outTradeNo of request parameter is incorrect"
                });
            }
            if (invoice.DueRemaining > 0)
            {
                try
                {
                    var settlement = await _invoiceRepository.MakeAutoUserPayment(invoice.InvoiceNo, invoice.DueRemaining,
                    DateTime.UtcNow, model.data.tradeNo, Server.Model.Common.PaymentMethodEnum.MobileTransfer);
                    if (settlement != null)
                    {
                        bool processes = false;
                        processes = await ProcessRequestPayment(invoice, settlement);
                        if (!processes)
                        {
                            processes = await ProcessRequirementPayment(invoice, settlement);
                        }
                        //if (!processes)
                        //{
                        //    call other 
                        //}
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest("Unable to update payment notification");
                }
            }
            return Ok(new
            {
                code = 0,
                msg = "success"
            });
        }
        private async Task<bool> ProcessRequestPayment(Server.Model.Payment.Invoice invoice, Server.Model.Payment.Settlment settlement)
        {
            var request = await _serviceRequestRepository.ServiceRequestByInvoice(invoice.InvoiceNo, invoice.ReferenceId);
            if (request == null) return false;
            await _serviceRequestRepository.RecordUserPayment(request.Id, invoice.InvoiceNo, settlement.ReceiptNo,
                    settlement.PaymentMethod, "", Guid.Empty);
            await _serviceRequestRepository.ApproveReceiptPrint(request.Id, settlement.Invoice.TotalPaid, settlement.Status, Guid.Empty);
            return true;
        }
        private async Task<bool> ProcessRequirementPayment(Server.Model.Payment.Invoice invoice, Server.Model.Payment.Settlment settlement)
        {
            var response = await _serviceRequestRepository.ServiceRequestResponseByInvoice(invoice.InvoiceNo, invoice.ReferenceId);
            if (response == null) return false;
            await _serviceRequestRepository.RecordRequirementUserPayment(response.ServiceRequestId, response.RequirementId,
                    invoice.InvoiceNo, settlement.ReceiptNo, settlement.PaymentMethod, "", Guid.Empty);
            await _serviceRequestRepository.ApproveRequirementReceiptPrint(response.ServiceRequestId, response.Id,
                settlement.Invoice.TotalPaid, settlement.Status, Guid.Empty);
            return true;
        }
    }
    public class RequestModel
    {
        public string appId { get; set; }
        public string sign { get; set; }
        public string ussd { get; set; }
    }
    public class NotifyModel
    {
        public int code { get; set; }
        public string msg { get; set; }
        public NotifyData data { get; set; }
    }
    public class NotifyData
    {
        public string outTradeNo { get; set; }
        public string tradeNo { get; set; }
    }
}
