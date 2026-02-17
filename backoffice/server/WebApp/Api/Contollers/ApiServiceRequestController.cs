using DocumentFormat.OpenXml.Office2010.Excel;
using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Common.Notification;
using ECCSA.BackOffice.Server.Model.Common.Servicing;
using ECCSA.BackOffice.Server.Model.Payment;
using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Servicing;
using ECCSA.BackOffice.Server.Persistence.Repositories.Communication;
using ECCSA.BackOffice.Server.Persistence.Repositories.Payment;
using ECCSA.BackOffice.Server.Persistence.Repositories.Security;
using ECCSA.BackOffice.Server.WebApp.Areas.Service.Model;
using ECCSA.BackOffice.Server.WebApp.Controllers;
using ECCSA.BackOffice.Server.WebApp.Models;
using FastReport.Export.PdfSimple;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Notification;
using ECCSA.BackOffice.Server.WebApp.Services;
using Serilog;

namespace ECCSA.BackOffice.Server.WebApp.Api.Contollers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //[AllowAnonymous]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ApiServiceRequestController : BaseController
    {
        private readonly ILogger<ApiServiceRequestController> _logger;
        private readonly ServiceRequestRepository _repository;
        // The Web API will only accept tokens 1) for users, and 2) having the "access_as_user" scope for this API
        static readonly string[] scopeRequiredByApi = new string[] { "Viewer" };
        private readonly ServiceTypeRepository _serviceTypeRepository;
        private readonly InvoiceRepository _invoiceRepository;
        private readonly LegalProfileRepository _legalProfileRepository;
        private readonly ExportAgreementRepository _exportAgreementRepository;
        private readonly ResourceRepository _resourceRepository;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public ApiServiceRequestController(ILogger<ApiServiceRequestController> logger,
            UserManager<User> userManager, ServiceRequestRepository repository,
            ServiceTypeRepository serviceTypeRepository,
             InvoiceRepository invoiceRepository,
            LegalProfileRepository legalProfileRepository,
            ExportAgreementRepository exportAgreementRepository,
             IWebHostEnvironment hostingEnvironment,
             PublicUserRepository userRepository,
             ResourceRepository resourceRepository
            , NotificationRepository notificationRepository) : base(userManager, userRepository, notificationRepository)
        {
            _logger = logger;
            _serviceTypeRepository = serviceTypeRepository;
            _repository = repository;
            _invoiceRepository = invoiceRepository;
            _hostingEnvironment = hostingEnvironment;
            _legalProfileRepository = legalProfileRepository;
            _exportAgreementRepository = exportAgreementRepository;
            _resourceRepository = resourceRepository;
            controllerName = "ApiServiceRequest";
        }
        [HttpPost]
        public async Task<IActionResult> Save(IFormCollection formCollection)
        {

            var message = string.Empty;
            var messageList = new List<string>();
            var isValid = true;
            try
            {
                Guid id = Guid.Parse(formCollection["Id"]);
                Guid serviceTypeId = Guid.Parse(formCollection["ServiceTypeId"].ToString());
                var serviceType = await _serviceTypeRepository.ServiceTypeWithRequiremnts(serviceTypeId);
                var serviceOwner = serviceType.ServiceOwner;
                var legalProfiles = _legalProfileRepository.UserLegalProfileLookup(TheUser.Id, 1);
                var agreements = _exportAgreementRepository.UserExportAgreementLookup(TheUser.Id, 1);
                var request = new ServiceRequest()
                {
                    Id = id,
                    ServiceTypeId = serviceTypeId,
                    RequestingUserId = TheUser.Id,
                    Responses = new List<ServiceRequirementResponse>(),
                    Attachments = new List<RequestAttachment>(),
                    OptionalEntries = new List<OptionalEntry>()
                };
                IQueryable<ServiceRequirementResponse> responses = null;
                if (request.Id != Guid.Empty)
                {
                    responses = _repository.Responses(TheUser.Id, id);
                }
                else
                {
                    //
                    if (serviceOwner == ECCSAModuleFlag.Academy)
                    {
                        var userAlreadyRegistered =
                            await _repository.UserServiceRequestByServiceTypeId(request.ServiceTypeId, TheUser.Id);
                        if (userAlreadyRegistered != null && userAlreadyRegistered.RequestStatus !=
                                                          WorkflowStateEnum.Rejected
                                                          && userAlreadyRegistered.RequestStatus !=
                                                          WorkflowStateEnum.Closed)
                        {
                            isValid = false;
                            message = "Registration already on process or processed.";
                        }
                    }

                }
                //
                var resp = await PopulateAttachment(formCollection, serviceType, request);
                if (!resp.isValid)
                {
                    isValid = false;
                    messageList.AddRange(resp.messages);
                }

                //
                //
                if (serviceType.AllowOptionalEntries)
                {
                    var optionalEntriesResult = await PopulateOptionalEntries(formCollection, serviceType, request);
                    if (!optionalEntriesResult.isValid)
                    {
                        isValid = false;
                        messageList.AddRange(optionalEntriesResult.messages);
                    }
                }
                //

                resp = await PopulateResponses(formCollection, serviceType, serviceOwner, legalProfiles, agreements,
                    request,
                    responses);
                if (!resp.isValid)
                {
                    isValid = false;
                    messageList.AddRange(resp.messages);
                }

                if (isValid)
                {
                    try
                    {
                        if (request.Id == Guid.Empty)
                        {
                            await _repository.Save(request, TheUser.Id);
                        }
                        else
                        {
                            await _repository.UpdateResponse(request, TheUser.Id);
                        }
                    }
                    catch (Exception ex)
                    {
                        LogError(_logger, ex, "ApiServiceRequest", "Save", null);
                        return Ok(new
                        {
                            saved = false,
                            erorMsg = "Unknown error"
                        });
                    }

                    return Ok(new
                    {
                        saved = true,
                        erorMsg = string.Empty
                    });
                }
            }
            catch (Exception ex)
            {

            }
            message += string.Join(".\r\n  ", messageList);
            return Ok(new
            {
                saved = false,
                erorMsg = message
            });
        }

        private async Task<(bool isValid, List<string> messages)> PopulateAttachment(IFormCollection formCollection, ServiceType serviceType, ServiceRequest request)
        {
            var messages = new List<string>();
            var isValid = true;
            if (!string.IsNullOrEmpty(formCollection["AttachmentTitle"]))
            {
                foreach (var title in formCollection["AttachmentTitle"])
                {
                    try
                    {
                        if (string.IsNullOrEmpty(title)) continue;
                        RequestAttachment attachment = new()
                        {
                            Id = Guid.NewGuid(),
                            Title = title,
                            IsOpponent = request.RequestingUserId != TheUser.Id
                        };
                        var file = formCollection.Files.FirstOrDefault(f => f.Name == title);
                        if (file != null)
                        {
                            string extension = Path.GetExtension(Path.GetFileName(file.FileName));
                            attachment.Extension = extension;
                        }
                        request.Attachments.Add(attachment);
                        if (file != null)
                        {
                            var fileName = file.FileName;
                            await SaveDocument(attachment.Id, serviceType.ServiceOwner, file);
                        }
                    }
                    catch (Exception ex)
                    {
                        LogError(_logger, ex, "ApiServiceRequest", "PopulateResponses", null);
                        isValid = false;
                        messages.Add($"Unable to save attachment for {title}");
                    }
                }
            }
            return (isValid, messages);
        }
        private async Task<(bool isValid, List<string> messages)> PopulateOptionalEntries(IFormCollection formCollection, ServiceType serviceType, ServiceRequest request)
        {
            var messages = new List<string>();
            var isValid = true;
            if (!string.IsNullOrWhiteSpace(formCollection["OptionalEntryLabel"]))
            {
                foreach (var label in formCollection["OptionalEntryLabel"])
                {
                    try
                    {
                        if (string.IsNullOrEmpty(label)) continue;
                        OptionalEntry entry;
                        if (request.OptionalEntries.Any(e => e.Label == label))
                        {
                            entry = request.OptionalEntries.FirstOrDefault(e => e.Label == label);
                        }
                        else
                        {
                            entry = new()
                            {
                                Label = label,
                            };
                        }
                        var value = formCollection["OptionalEntryValue" + label];
                        if (!string.IsNullOrWhiteSpace(value))
                        {
                            entry.Value = value;
                        }
                        request.OptionalEntries.Add(entry);

                    }
                    catch (Exception ex)
                    {
                        LogError(_logger, ex, "ApiServiceRequest", "PopulateOptionalEntries", null);
                        isValid = false;
                        messages.Add($"Unable to save optional Requirement for {label}");
                    }
                }
            }
            return (isValid, messages);
        }

        private async Task<(bool isValid, List<string> messages)> PopulateResponses(IFormCollection formCollection, ServiceType serviceType,
            ECCSAModuleFlag serviceOwner, List<LookupValue> legalProfiles,
            List<LookupValue> agreements, ServiceRequest request,
            IQueryable<ServiceRequirementResponse> responses)
        {
            var messages = new List<string>();
            var isValid = true;
            //var isOponent =  request.OpponentUserId.HasValue && request.OpponentUserId.Value  == TheUser.Id;
            foreach (var req in serviceType.ServiceRequirements)
            {
                Guid reqResponseId = Guid.NewGuid();

                if (request.Id != Guid.Empty)
                {
                    reqResponseId = responses?.FirstOrDefault(r => r.RequirementId == req.Id)?.Id
                                    ?? Guid.NewGuid();
                }

                //var key = req.Id.ToString();
                var key = Guid2String(req.Id);
                var val = formCollection[key];

                var resp = new ServiceRequirementResponse
                {
                    Id = reqResponseId,
                    ServiceRequestId = Guid.Empty,
                    RequirementId = req.Id,
                };
                if (!req.IsForOpponent || !string.IsNullOrEmpty(val))
                {
                    switch (req.InputMethod)
                    {
                        case ServiceInputTypeEnum.Upload:
                            try
                            {
                                var file = formCollection.Files.FirstOrDefault(f => f.Name == key);
                                if (file != null)
                                {
                                    var fileName = file.FileName;
                                    await SaveDocument(resp.Id, serviceOwner, file);
                                    resp.TextResponse = Path.GetExtension(Path.GetFileName(fileName));
                                }
                                //else
                                //{
                                //    isValid = false;
                                //    messages.Add($"Upload file for {req.Label} is required!");
                                //}
                            }
                            catch (Exception ex)
                            {
                                LogError(_logger, ex, "ApiServiceRequest", "PopulateResponses", null);
                                isValid = false;
                                messages.Add($"Error uploading {req.Label}");
                            }

                            break;
                        case ServiceInputTypeEnum.Integer:
                            if (long.TryParse(val, out var lval))
                                resp.IntResponse = lval;
                            else
                            {
                                LogError(_logger, new Exception("Value out of range"), "ApiServiceRequest", "PopulateResponses", null);
                                isValid = false;
                                messages.Add($"Value {val} is out of range for {req.Label}");
                            }
                            break;
                        case ServiceInputTypeEnum.Decimal:
                            if (decimal.TryParse(val, out var dval))
                                resp.DecimalResponse = dval;
                            else
                            {
                                LogError(_logger, new Exception("Value out of range"), "ApiServiceRequest", "PopulateResponses", null);
                                isValid = false;
                                messages.Add($"Value {val} is out of range for {req.Label}");
                            }
                            break;
                        case ServiceInputTypeEnum.Date:
                            try
                            {
                                resp.DateResponse = DateTime.Parse(val);
                            }
                            catch (Exception ex)
                            {
                                LogError(_logger, ex, "ApiServiceRequest", "PopulateResponses", null);
                                isValid = false;
                                messages.Add($"Invalid format {val} for {req.Label}");
                            }

                            break;
                        case ServiceInputTypeEnum.SimpleLookup:
                            try
                            {
                                resp.LookupValueId = Guid.Parse(val);
                                resp.TextResponse = serviceType.RequirementLookups
                                    .FirstOrDefault(l => l.Id == req.LookupTypeId)?.LookupValues
                                    .FirstOrDefault(v => v.Id == resp.LookupValueId)?.Name;
                            }
                            catch (Exception ex)
                            {
                                LogError(_logger, ex, "ApiServiceRequest", "PopulateResponses", null);
                                isValid = false;
                                messages.Add($"Invalid format {val} for {req.Label}");
                            }
                            break;
                        case ServiceInputTypeEnum.TradeAgreement:
                            try
                            {
                                resp.LookupValueId = Guid.Parse(val);
                                resp.TextResponse = agreements.FirstOrDefault(v => v.Id == resp.LookupValueId)?.Name;
                            }
                            catch (Exception ex)
                            {
                                LogError(_logger, ex, "ApiServiceRequest", "PopulateResponses", null);
                                isValid = false;
                                messages.Add($"Invalid Trade Agreement {val} for {req.Label}");
                            }
                            break;
                        case ServiceInputTypeEnum.LegalDocument:
                            try
                            {
                                resp.LookupValueId = Guid.Parse(val);
                                resp.TextResponse = legalProfiles.FirstOrDefault(v => v.Id == resp.LookupValueId)?.Name;
                            }
                            catch (Exception ex)
                            {
                                LogError(_logger, ex, "ApiServiceRequest", "PopulateResponses", null);
                                isValid = false;
                                messages.Add($"Invalid legal document {val} for {req.Label}");
                            }
                            break;
                        case ServiceInputTypeEnum.Tabular:
                            try
                            {
                                var tabularData = JArray.Parse(val);
                                var data = JsonConvert.DeserializeObject<List<TabularResponse>>(val);
                                resp.TabularResponse = data;
                                //if (resp.TabularResponse == null || resp.TabularResponse.Count <= 0)
                                //{
                                //    isValid = false;
                                //    messages.Add($"At-least one record is required for {req.Label}!");
                                //}
                            }
                            catch (Exception ex)
                            {
                                LogError(_logger, ex, "ApiServiceRequest", "PopulateResponses", null);
                                isValid = false;
                                messages.Add($"Invalid tabular data for {req.Label}");
                            }
                            break;
                        case ServiceInputTypeEnum.Email:
                            if (IsValidEmailAddress(val))
                            {
                                resp.TextResponse = val;
                            }
                            else
                            {
                                LogError(_logger, new InvalidCastException("Invalid email format"), "ApiServiceRequest", "PopulateResponses", null);
                                isValid = false;
                                messages.Add($"Invalid Email {val} for {req.Label}");
                            }
                            break;
                        case ServiceInputTypeEnum.Mobile:
                        case ServiceInputTypeEnum.Phone:
                            if (IsValidPhoneNo(val))
                            {
                                resp.TextResponse = val;
                            }
                            else
                            {
                                LogError(_logger, new InvalidCastException("Invalid Phone number"), "ApiServiceRequest", "PopulateResponses", null);
                                isValid = false;
                                messages.Add($"Invalid Phone {val} for {req.Label}");
                            }
                            break;
                        default:
                            resp.TextResponse = val;
                            //if(string.IsNullOrWhiteSpace(resp.TextResponse))
                            //{
                            //    isValid = false;
                            //    messages.Add($"Value for {req.Label} is required!");
                            //}
                            break;
                    }
                }
                request.Responses.Add(resp);
            }

            return (isValid, messages);

        }

        [HttpPost]
        public async Task<IActionResult> SaveAttachment(IFormCollection formCollection)
        {
            var message = string.Empty;
            var isValid = true;
            Guid serviceTypeId = Guid.Parse(formCollection["ServiceTypeId"]);
            Guid requestId = Guid.Parse(formCollection["RequestId"]);

            var serviceRequest = await _repository.UserServiceRequest(TheUser.Id, requestId);
            if (serviceRequest == null)
            {
                return BadRequest("Invalid Request");
            }
            var serviceType = await _serviceTypeRepository.ServiceTypeWithRequiremnts(serviceTypeId);
            RequestAttachment data = new()
            {
                Title = formCollection["Title"],
                ServiceRequestId = requestId,
                IsOpponent = serviceRequest.RequestingUserId != TheUser.Id
            };
            try
            {
                var file = formCollection.Files.FirstOrDefault(f => f.Name == "attachment");
                if (file != null)
                {
                    string extension = Path.GetExtension(Path.GetFileName(file.FileName));
                    data.Extension = extension;
                }
                var id = await _repository.Save(data, TheUser.Id);
                if (file != null)
                {
                    var fileName = file.FileName;
                    await SaveDocument(id, serviceType.ServiceOwner, file);
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
                isValid = false;
                _logger.LogError(ex, "Request Attachment Save");
            }
            return Ok(new
            {
                saved = isValid,
                erorMsg = message
            });
        }
        [HttpPost]
        public async Task<IActionResult> SaveOptionalEntry(IFormCollection formCollection)
        {
            var message = string.Empty;
            var isValid = true;
            Guid serviceTypeId = Guid.Parse(formCollection["ServiceTypeId"]);
            Guid requestId = Guid.Parse(formCollection["RequestId"]);

            var serviceRequest = await _repository.UserServiceRequest(TheUser.Id, requestId);
            if (serviceRequest == null)
            {
                return BadRequest("Invalid Request");
            }
            var serviceType = await _serviceTypeRepository.ServiceTypeWithRequiremnts(serviceTypeId);
            if (!serviceType.AllowOptionalEntries)
            {
                return Ok(new
                {
                    saved = false,
                    erorMsg = "Optional Requirements not allowed for the service type"
                });
            }
            OptionalEntry data = new()
            {
                Label = formCollection["Label"],
                Value = formCollection["Value"],
            };
            serviceRequest.OptionalEntries ??= new List<OptionalEntry>();
            if (serviceRequest.OptionalEntries.Any(e => e.Label == data.Label))
            {
                serviceRequest.OptionalEntries.FirstOrDefault(e => e.Label == data.Label).Value = data.Value;
            }
            else
            {
                serviceRequest.OptionalEntries.Add(data);
            }
            try
            {
                await _repository.UpdateOptionalResponse(serviceRequest, TheUser.Id);
            }
            catch (Exception ex)
            {
                message = ex.Message;
                isValid = false;
                _logger.LogError(ex, "Request Optional Requirement Save");
            }
            return Ok(new
            {
                saved = isValid,
                erorMsg = message
            });
        }
        [HttpPost]
        [Route("{id}")]
        public async Task<IActionResult> DeleteAttachment(Guid id)
        {
            var requestAttachment = await _repository.RequestAttachment(id);
            var serviceType = requestAttachment?.Request?.ServiceType;
            if (serviceType == null)
                return BadRequest("Invalid Attachment");
            var shortName = Guid2String(id) + requestAttachment.Extension;
            var result = Path.Combine(AbsolutePath(serviceType.ServiceOwner), shortName);
            await _repository.DeleteAttachment(id, TheUser.Id, result);
            return Ok(new { success = true });
        }
        [HttpPost]
        [Route("{label}/{requestId}")]
        public async Task<IActionResult> DeleteOptionalEntry(string label, Guid requestId)
        {
            var message = string.Empty;
            var isValid = true;
            var serviceRequest = await _repository.UserServiceRequest(TheUser.Id, requestId);
            if (serviceRequest == null)
            {
                return BadRequest("Invalid Request");
            }
            serviceRequest.OptionalEntries = serviceRequest.OptionalEntries.Where(s => s.Label != label).ToList();
            try
            {
                await _repository.UpdateOptionalResponse(serviceRequest, TheUser.Id);
            }
            catch (Exception ex)
            {
                message = ex.Message;
                isValid = false;
                _logger.LogError(ex, "Request Attachment Save");
            }
            return Ok(new
            {
                success = isValid,
                erorMsg = message
            });
        }
        [HttpGet]
        public IActionResult GetRequests(int status = -1)
        {
            //HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
            LogStart(_logger, controllerName, "GetRequests");

            var serviceRequests = _repository.UserServiceRequests(TheUser.Id, status).OrderByDescending(s => s.RequestDate)
                .ThenByDescending(r => r.CreateStamp);

            return Ok(serviceRequests);
        }
        [HttpGet]
        public IActionResult GetResourceRequests(int status = -1)
        {
            //HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
            LogStart(_logger, controllerName, "GetResourceRequests");

            var serviceRequests = _repository.UserResourceRequests(TheUser.Id, status).OrderByDescending(s => s.RequestDate)
                .ThenByDescending(r => r.CreateStamp);

            return Ok(serviceRequests);
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetUserCertificateOfOrigin(long id)
        {
            //HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
            LogStart(_logger, controllerName, "GetUserCertificateOfOrigin");

            var certificateOfOrigin = await _repository.UserCertificateOfOrigin(TheUser.Id, id);

            return Ok(certificateOfOrigin);
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetRequest(Guid id)
        {
            //HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);

            var serviceRequest = await _repository.UserServiceRequest(TheUser.Id, id);

            return Ok(serviceRequest);
        }
        [HttpPost]
        [Route("{id}")]
        public async Task<IActionResult> SubmitRequest(Guid id)
        {
            await _repository.SubmitUserRequest(id, TheUser.Id);
            var request = await _repository.UserServiceRequestForNotification(TheUser.Id, id);
            var subject = $"Service Request {request.TrackingNo} is submitted.";
            var body = $"Dear, This is just to inform you that the your request {request.TrackingNo} is submitted.";
            var recepients = new List<INotificationAddress> { request };
            await SaveNotification(request.ServiceType.ServiceOwner, subject, body,
                recepients,
                null, SendThroughFlag.Email | SendThroughFlag.SMS);
            if (request.ServiceType.WorkerDetails.Count > 0)
            {
                var backOfficeWorkers = new List<INotificationAddress>();
                backOfficeWorkers.AddRange(request.ServiceType.WorkerDetails);
                await SaveNotification(request.ServiceType.ServiceOwner,
                    $"{request.ServiceType.NameEng} submitted {request.TrackingNo}", "", backOfficeWorkers, null,
                    SendThroughFlag.SMS);
            }

            //
            return Ok(new
            {
                submitted = true
            });
        }
        [HttpPost]
        [Route("{id}")]
        public async Task<IActionResult> CloseRequest(Guid id)
        {
            await _repository.CloseUserRequest(id, TheUser.Id);
            return Ok(new
            {
                submitted = true
            });
        }
        [HttpPost]
        [Route("{id}")]
        public async Task<IActionResult> CloseUserCertificateOfOrigin(long id)
        {
            await _repository.CloseUserCertificateOfOrigin(id, TheUser.Id);
            return Ok(new
            {
                submitted = true
            });
        }
        [HttpPost]
        [Route("{id}")]
        public async Task<IActionResult> SubmitRequestDoc(Guid id)
        {
            var res = await _repository.GetResponseServiceType(id, TheUser.Id);
            await _repository.SubmitUserRequestResponse(id, TheUser.Id);
            if (res != null && res.ServiceRequest != null && res.Requirement?.ServiceType?.WorkerDetails?.Count > 0)
            {
                var backOfficeWorkers = new List<INotificationAddress>();
                backOfficeWorkers.AddRange(res.Requirement?.ServiceType.WorkerDetails);
                await SaveNotification(res.Requirement.ServiceType.ServiceOwner,
                    $"Response Submitted for {res.Requirement.NameEng} by {res.ServiceRequest.TrackingNo}", "", backOfficeWorkers, null,
                    SendThroughFlag.SMS);
            }
            return Ok(new
            {
                submitted = true
            });
        }
        [HttpGet]
        public async Task<FileResult> DownLoadFile(string filePath, string title)
        {
            return await base.DownLoadFile(FileRoot + filePath, title);
        }
        [HttpPost]
        public async Task<IActionResult> MakePayment(IFormCollection formCollection)
        {
            Guid requestId = Guid.Parse(formCollection["requestId"]);
            var serviceRequest = await _repository.UserServiceRequestForInvoice(TheUser.Id, requestId);
            if (serviceRequest == null)
            {
                return BadRequest("Invalid Request.");
            }
            var serviceType = await _serviceTypeRepository.ServiceTypeById(serviceRequest.ServiceTypeId);
            if (serviceType == null)
            {
                return BadRequest("Invalid Request.");
            }
            string invoiceNo = formCollection["invoiceNo"];
            string receiptNo = formCollection["receiptNo"];
            decimal amount = decimal.Parse(formCollection["amount"]);
            string receiptExtention = "";
            var file = formCollection.Files.FirstOrDefault(f => f.Name == "receipt");
            if (file != null)
            {
                string extension = Path.GetExtension(Path.GetFileName(file.FileName));
                receiptExtention = extension;
            }
            PaymentMethodEnum paymentMethod = (PaymentMethodEnum)int.Parse(formCollection["paymentMethod"]);
            DateTime paymentDate = DateTime.Now;
            string remark = string.Empty;
            try
            {
                var invoice = await _invoiceRepository.InvoiceByNo(serviceRequest.InvoiceNo, serviceRequest.Id);
                if (invoice.InvoiceDate > paymentDate || paymentDate > DateTime.Now)
                {
                    CommandStatus(CommandStatusEnum.ValidationError);

                    return BadRequest("Payment Date out of range");
                }
                var settlement = await _invoiceRepository.MakeUserPayment(requestId, invoiceNo, amount, paymentDate, receiptNo,
                    paymentMethod, remark, receiptExtention, TheUser.Id);
                if (file != null && settlement != null)
                {
                    var fileName = file.FileName;
                    await SaveDocument(settlement.Id, serviceType.ServiceOwner, file, "receipts");
                }
                if (settlement != null)
                {
                    await _repository.RecordUserPayment(requestId, invoiceNo, settlement.ReceiptNo, paymentMethod, remark, TheUser.Id);
                }
                return Ok("Payment Processed");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest("Unable to save payment");
            }
        }

        [HttpPost]
        public async Task<IActionResult> MakeRequirementPayment(IFormCollection formCollection)
        {

            DateTime paymentDate = DateTime.Today;
            var remark = string.IsNullOrEmpty(formCollection["remark"]) ? string.Empty : formCollection["remark"].ToString();
            var id = Guid.Parse(formCollection["id"]); ;
            var requestId = Guid.Parse(formCollection["requestId"]);
            var invoiceNo = formCollection["invoiceNo"];
            var amount = decimal.Parse(formCollection["amount"]);
            var receiptNo = formCollection["receiptNo"];
            string receiptExtention = "";
            PaymentMethodEnum paymentMethod = (PaymentMethodEnum)int.Parse(formCollection["paymentMethod"]);
            var file = formCollection.Files.FirstOrDefault(f => f.Name == "receipt");
            if (file != null)
            {
                string extension = Path.GetExtension(Path.GetFileName(file.FileName));
                receiptExtention = extension;
            }
            try
            {
                var request = await _repository.ServiceRequestById(requestId, TheUser.Id);
                if (request == null)
                {
                    return BadRequest("Invalid Request.");
                }
                var serviceType = await _serviceTypeRepository.ServiceTypeById(request.ServiceTypeId);
                if (serviceType == null)
                {
                    return BadRequest("Invalid Request.");
                }
                var requirement = request.ServiceType.ServiceRequirements.FirstOrDefault(x => x.Id == id);
                var response = request.Responses.FirstOrDefault(r => r.RequirementId == id);
                if (requirement == null
                    || requirement.InputMethod != ServiceInputTypeEnum.Payment
                    || requirement.RequirementType != RequirementTypeEnum.Invoice
                    || !response.InvoiceAmount.HasValue
                    || response.InvoiceAmount.Value < 0.01m
                    || amount < 0.01m)
                {
                    return Ok("Payment Processed");
                }

                var invoice = await _invoiceRepository.InvoiceByNo(response.InvoiceNo, response.Id);
                if (invoice.InvoiceDate.Date > paymentDate || paymentDate > DateTime.Today)
                {
                    return BadRequest("Payment Date out of range");
                }
                var settlement = await _invoiceRepository.MakeUserPayment(response.Id, invoiceNo, amount, paymentDate, receiptNo, paymentMethod, remark, receiptExtention, TheUser.Id);
                if (file != null && settlement != null)
                {
                    var fileName = file.FileName;
                    await SaveDocument(settlement.Id, serviceType.ServiceOwner, file, "receipts");

                }
                if (settlement != null)
                {
                    await _repository.RecordRequirementUserPayment(requestId, id, invoiceNo, settlement.ReceiptNo, settlement.PaymentMethod, remark, TheUser.Id);
                }
                if (serviceType.WorkerDetails.Count > 0)
                {
                    var backOfficeWorkers = new List<INotificationAddress>();
                    backOfficeWorkers.AddRange(serviceType.WorkerDetails);
                    await SaveNotification(serviceType.ServiceOwner,
                        $"Payment made for {requirement.NameEng} by {request.TrackingNo}", "", backOfficeWorkers, null,
                        SendThroughFlag.SMS);
                }
                return Ok("Payment Processed");
            }
            catch (Exception ex)
            {
                return BadRequest("Unable to save requirement payment");
            }
        }

        [HttpGet]
        public async Task<IActionResult> PrintInvoice(Guid referenceId, string invoiceNo)
        {
            Invoice invoice = await _invoiceRepository.InvoiceByNo(invoiceNo, referenceId);
            var pi = new PrintInvoice(invoice);
            var fileName = $"Invoice_{pi.InvoiceNo}.pdf";
            var stream = GetCurrentFile(fileName, Path.GetTempPath());
            if (stream != null)
            {
                Response.Headers.Add("content-disposition", "inline;filename=" + fileName);
                return File(stream, "application/pdf");
            }
            try
            {
                var request = await _repository.ServiceRequestById(invoice.ReferenceId, TheUser.Id);
                if (request == null)
                {
                    var res = _repository.Response(TheUser.Id, referenceId);
                    pi.PaymentReason = res?.Requirement?.NameEng ?? "";
                }
                else
                {
                    pi.PaymentReason = request.ServiceType.NameEng;
                }
                var data = new List<PrintInvoice>
                {
                    pi
                };
                stream = GetReportStream<PrintInvoice>("Invoice.frx", data, "PrintInvoice", fileName, Path.GetTempPath());

                Response.Headers.Add("content-disposition", "inline;filename=" + fileName);
                return File(stream, "application/octet-stream", fileName);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return BadRequest("Unable to print Invoice");
            }

        }

        [HttpGet]
        public async Task<IActionResult> PrintReceipt(Guid referenceId, string invoiceNo, int no = 0)
        {
            Invoice invoice = await _invoiceRepository.InvoiceByNo(invoiceNo, referenceId);
            var pi = new PrintInvoice(invoice);

            if (invoice.Settlments == null || invoice.Settlments.Count < no + 1)
            {
                return BadRequest("Receipt Not found");
            }
            var fileName = $"Receipt_{pi.InvoiceNo}_{pi.Settlements[no].ReceiptNo}.pdf";
            var stream = GetCurrentFile(fileName, Path.GetTempPath());
            if (stream != null)
            {
                Response.Headers.Add("content-disposition", "inline;filename=" + fileName);
                return File(stream, "application/pdf");
            }
            try
            {

                var request = await _repository.ServiceRequestById(invoice.ReferenceId, TheUser.Id);
                if (request == null)
                {
                    var res = _repository.Response(TheUser.Id, referenceId);
                    pi.PaymentReason = res?.Requirement?.NameEng ?? "";
                }
                else
                {
                    pi.PaymentReason = request.ServiceType.NameEng;
                }
                var data = new List<PrintInvoice>
                {
                    pi
                };
                stream = GetReportStream<PrintInvoice>("Receipt.frx", data, "PrintInvoice", fileName, Path.GetTempPath());

                Response.Headers.Add("content-disposition", "inline;filename=" + fileName);
                return File(stream, "application/pdf");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> SaveTeleOutTradeNo(IFormCollection formCollection)
        {
            Guid refId = Guid.Parse(formCollection["refId"]);
            bool request = bool.Parse(formCollection["request"]);
            var invoiceNo = "";
            if (request)
            {
                var serviceRequest = await _repository.UserServiceRequestForInvoice(TheUser.Id, refId);
                if (serviceRequest == null)
                {
                    return BadRequest("Invalid Request.");
                }
                invoiceNo = serviceRequest.InvoiceNo;
            }
            else
            {
                var response = await _repository.UserResponseById(TheUser.Id, refId);
                if (response == null)
                {
                    return BadRequest("Invalid Request response.");
                }
                invoiceNo = response.InvoiceNo;
            }
            string outTradeNo = formCollection["outTradeNo"];
            string url = formCollection["payUrl"];
            try
            {
                await _invoiceRepository.SaveTeleOutTradeNo(invoiceNo, refId, outTradeNo, url);
                return Ok("outTradeNo Saved");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest("Unable to save Tele out Trade NO");
            }
        }
        private FileStream GetCurrentFile(string fileName, string outFolder)
        {
            //var outFolder = Path.GetTempPath();
            if (!Directory.Exists(outFolder))
                Directory.CreateDirectory(outFolder);

            var tmpFileName = Path.Combine(outFolder, fileName);
            if (System.IO.File.Exists(tmpFileName))
            {
                return new FileStream(tmpFileName, FileMode.Open);
            }
            return null;
        }
        private FileStream GetReportStream<T>(string reportName, IEnumerable<T> data, string dataName, string fileName, string outFolder)
        {
            //var outFolder = Path.GetTempPath();
            if (!Directory.Exists(outFolder))
                Directory.CreateDirectory(outFolder);

            var tmpFileName = Path.Combine(outFolder, fileName);
            FastReport.Report report = new();
            report.Load(Path.Combine(_hostingEnvironment.WebRootPath, "ReportTemplates", reportName));
            report.RegisterData(data, dataName);
            report.Prepare();
            var pdfExport = new PDFSimpleExport();
            pdfExport.Export(report, tmpFileName);
            report.Dispose();

            return new FileStream(tmpFileName, FileMode.Open);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult VerifyCertificate(string code)
        {
            var fileName = $"Certificate_{code}.pdf";
            var stream = GetCurrentFile(fileName, Path.Combine("wwwroot", "certificates"));

            if (stream != null)
            {
                Response.Headers.Add("content-disposition", "inline;filename=" + $"Certificate_{DateTime.Now.ToString("yyMMddHHmmss")}.pdf");
                return File(stream, "application/pdf");
            }
            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> PrintCertificate(Guid serviceId)
        {
            try
            {
                var request = await _repository.ServiceRequestForPrint(serviceId, TheUser.Id);
                if (request == null || request.ServiceType == null)
                {
                    return NotFound();
                }
                var gd = Guid2String(serviceId);

                var fileName = $"Certificate_{gd}.pdf";
                var stream = GetCurrentFile(fileName, Path.Combine("wwwroot", "certificates"));
                var certificateName = request.ServiceType.CertificateType.GetDisplayName();
                if (stream != null && request.ServiceType.CertificateType != CertificateTypeEnum.None)
                {
                    Response.Headers.Add("content-disposition", "inline;filename=" + $"{certificateName}.pdf");
                    return File(stream, "application/pdf");
                }
                switch (request.ServiceType.CertificateType)
                {
                    case CertificateTypeEnum.CommercialInvoice:

                        var countryOfOrg = request.ServiceType.ServiceTypeReports.FirstOrDefault(s => s.EnumValue == (int)CommercialInvoiceReportFields.CountryOfOrigin);
                        var dateOfSh = request.ServiceType.ServiceTypeReports.FirstOrDefault(s => s.EnumValue == (int)CommercialInvoiceReportFields.DateOfShipment);
                        var permitNo = request.ServiceType.ServiceTypeReports.FirstOrDefault(s => s.EnumValue == (int)CommercialInvoiceReportFields.PermitNo);
                        var consignee = request.ServiceType.ServiceTypeReports.FirstOrDefault(s => s.EnumValue == (int)CommercialInvoiceReportFields.ConsigneeName);
                        var consignor = request.ServiceType.ServiceTypeReports.FirstOrDefault(s => s.EnumValue == (int)CommercialInvoiceReportFields.ConsignorName);
                        var bankDet = request.ServiceType.ServiceTypeReports.FirstOrDefault(s => s.EnumValue == (int)CommercialInvoiceReportFields.BankDetail);
                        var importer = request.ServiceType.ServiceTypeReports.FirstOrDefault(s => s.EnumValue == (int)CommercialInvoiceReportFields.Importer);
                        request.OptionalEntries.Insert(0, new OptionalEntry
                        {
                            Label = "Date of Shipment",
                            Value =
                            request.Responses.FirstOrDefault(s => s.RequirementId == dateOfSh.ServiceRequirementId)?.DateResponse?.ToString("yyyy-MM-dd") ?? ""
                        });
                        request.OptionalEntries.Insert(1, new OptionalEntry
                        {
                            Label = "Country of Origin",
                            Value =
                            request.Responses.FirstOrDefault(s => s.RequirementId == countryOfOrg.ServiceRequirementId)?.TextResponse ?? ""
                        });
                        request.OptionalEntries.Insert(2, new OptionalEntry
                        {
                            Label = "Permit No",
                            Value =
                            request.Responses.FirstOrDefault(s => s.RequirementId == permitNo.ServiceRequirementId)?.TextResponse ?? ""
                        });
                        //var consigneeres = request.Responses.FirstOrDefault(s => s.RequirementId == consignee?.ServiceRequirementId);
                        //if (consigneeres != null)
                        //    consigneeres.TextResponse = consigneeres.TextResponse.Count(s => s == '\n') > 5 ? consigneeres.TextResponse.Replace("\r\n", "; ") : consigneeres.TextResponse;
                        //var consignorres = request.Responses.FirstOrDefault(s => s.RequirementId == consignor?.ServiceRequirementId);
                        //if (consignorres != null)
                        //    consignorres.TextResponse = consignorres.TextResponse.Count(s => s == '\n') > 5 ? consignorres.TextResponse.Replace("\r\n", "; ") : consignorres.TextResponse;
                        //var bankDetres = request.Responses.FirstOrDefault(s => s.RequirementId == bankDet?.ServiceRequirementId);
                        //if (bankDetres != null)
                        //    bankDetres.TextResponse = bankDetres.TextResponse.Count(s => s == '\n') > 3 ? bankDetres.TextResponse.Replace("\r\n", "; ") : bankDetres.TextResponse;
                        var importerres = request.Responses.FirstOrDefault(s => s.RequirementId == importer?.ServiceRequirementId);
                        if (importerres != null)
                        {
                           // importerres.TextResponse = importerres.TextResponse.Count(s => s == '\n') > 4 ? importerres.TextResponse.Replace("\r\n", "; ") : importerres.TextResponse;
                            if (!string.IsNullOrWhiteSpace(importerres.TextResponse) && importerres.TextResponse.Length > 5)
                            {
                                importerres.TextResponse = "Importer: " + importerres.TextResponse;
                            }
                            else
                            {
                                importerres.TextResponse = "";
                            }
                        }
                        stream = GetReportData<CommercialInvoice, CommercialInvoiceDetail, CommercialInvoiceReportFields>
                            (request, fileName, "CommercialInvoice.frx", "CommercialInvoice");
                        break;
                    case CertificateTypeEnum.PackingList:
                        stream = GetReportData<PackingListModel, PackingListModelDetail, PackingListReportFields>
                            (request, fileName, "PackingListCertificate.frx", "PackingListModel");
                        break;
                    case CertificateTypeEnum.SupportLetter:
                        stream = GetReportData<SupportLetterReportModel, object, SupportLetterReportFields>
                            (request, fileName, "SupportLetter.frx", "SupportLetter");
                        //try
                        //{
                        //    return await GenerateSupportLetter(serviceId);

                        //}
                        //catch (Exception ex)
                        //{
                        //    return BadRequest(ex.Message);
                        //}
                        break;
                    case CertificateTypeEnum.HealthCertificate:
                        stream = GetReportData<HealthCertificateReportModel, object, HealthCertificateReportFields>
                            (request, fileName, "HealthCertificate.frx", "HealthCertificateReportModel");
                        break;
                    case CertificateTypeEnum.FreeSale:
                        stream = GetReportData<FreeSaleModel, FreeSaleModelDetail, FreeSaleReportFields>
                            (request, fileName, "FreeSale.frx", "FreeSale");
                        break;
                    default:
                        return NotFound();
                }
                Response.Headers.Add("content-disposition", "inline;filename=" + $"{certificateName}.pdf");
                return File(stream, "application/octet-stream", fileName);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return BadRequest("Unable to print certificate.");
            }
        }

        private FileStream GetReportData<T, D, E>(ServiceRequest request, string fileName, string templateName, string dataName)
            where T : IPrintReportModelWithDetail<D>, IPrintReportModel, new()
            where D : new()
            where E : Enum
        {
            var values = Enum.GetValues(typeof(E));
            var data = new List<T>();
            //data.Add(PackingListModel.GenerateFack());
            var reportValues = request.ServiceType.ServiceTypeReports;
            var model = new T(); ;
            foreach (var report in reportValues)
            {
                var prop = model.GetType().GetProperty(Enum.GetName(typeof(E), report.EnumValue));
                var response = request.Responses.FirstOrDefault(r => r.RequirementId == report.ServiceRequirementId);
                var requirement = request.ServiceType.ServiceRequirements.FirstOrDefault(r => r.Id == report.ServiceRequirementId);
                if (requirement == null || response == null || prop == null) continue;
                string reportValue = null;
                if (requirement.InputMethod == ServiceInputTypeEnum.TradeAgreement && report.FieldName != null &&
                    response.LookupValueId != null)
                {
                    var aggrement = _exportAgreementRepository.UserExportAgreementByCommodityId(request.RequestingUserId, response.LookupValueId.Value);
                    if (aggrement != null)
                    {
                        var aggProp = aggrement.GetType().GetProperty(report.FieldName);
                        if (aggProp != null && aggProp.GetValue(aggrement) != null)
                        {
                            reportValue = aggProp.GetValue(aggrement).ToString();
                        }
                    }
                }
                else if (requirement.InputMethod == ServiceInputTypeEnum.Tabular &&
                    typeof(IPrintReportModelWithDetail<D>).IsAssignableFrom(typeof(T)))
                {
                    var tabularSource = request.ServiceType.TabularRequirements.FirstOrDefault(r => r.Id == requirement.LookupTypeId);
                    if (tabularSource != null && response.TabularResponse != null)
                    {
                        model.Details = new List<D>();
                        var groupedResponse = response.TabularResponse.GroupBy(s => s.RowId);
                        foreach (var g in groupedResponse)
                        {
                            var reportDetail = new D();
                            foreach (var item in g)
                            {
                                var tabularS = tabularSource.Details.FirstOrDefault(d => d.Id == item.Text);
                                if (tabularS == null) continue;
                                var Detailprop = reportDetail.GetType().GetProperty(tabularS.Name);
                                if (Detailprop.PropertyType == typeof(System.Decimal))
                                {
                                    if (decimal.TryParse(item.Value, out var dv))
                                    {
                                        Detailprop.SetValue(reportDetail, dv);
                                    }
                                }
                                else if (Detailprop.PropertyType == typeof(System.Int32))
                                {
                                    if (int.TryParse(item.Value, out var dv))
                                    {
                                        Detailprop.SetValue(reportDetail, dv);
                                    }
                                }
                                else
                                {
                                    Detailprop.SetValue(reportDetail, item.Value.Length > 270 ? item.Value.Substring(0, 270) : item.Value);
                                }
                            }
                            model.Details.Add(reportDetail);
                        }
                    }
                }
                else if (reportValue == null)
                {
                    reportValue = response.Response;
                }
                if (requirement.InputMethod != ServiceInputTypeEnum.Tabular)
                {
                    prop.SetValue(model, reportValue);
                }
            }
            model.CompanyName = TheUser.CompanyName;
            model.CompanyAddress = TheUser.Email + "\r\n" + TheUser.PhoneNumber + ", " + TheUser.MobileNo + "\r\n" + TheUser.Address;
            model.AuthorizedBy = request.OperatorName;
            model.AuthorizationDate = request.AuthorizedDate?.ToString("dd/MM/yyyy");
            model.AuthorizerTitle = request.OperatorTitle;
            model.ApproverName = request.ApproverName;
            model.ApproveredDate = request.ApprovedDate?.ToString("dd/MM/yyyy");
            model.RequestDate = request.RequestDate.ToString("dd o\\f MMM, yyyy");
            model.ApproverTitle = request.ApproverTitle;
            model.Signature = Guid2String(request.Id);
            model.CertificateNo = request.Signature;
            model.InvoiceNo = request.InvoiceNo;
            model.OptionalEntries = new List<OptionalEntryForReport>();

            if (request.OptionalEntries != null && request.OptionalEntries.Count > 0)
            {
                int i = 0;
                for (int j = 0; j < request.OptionalEntries.Count; j++)
                {
                    if (j % 2 == 0)
                    {
                        model.OptionalEntries.Add(new OptionalEntryForReport
                        {
                            Label = request.OptionalEntries[j].Label,
                            Value = request.OptionalEntries[j].Value,
                        });
                        i++;
                    }
                    else if (model.OptionalEntries.Count >= i)
                    {
                        model.OptionalEntries[i - 1].Label1 = request.OptionalEntries[j].Label + ":";
                        model.OptionalEntries[i - 1].Value1 = request.OptionalEntries[j].Value;
                    }
                }
            }
            string logoUrl = !string.IsNullOrWhiteSpace(TheUser.LogoExtension) ?
            Path.Combine("..", UserLogoDocumentsAbsolutePath, Guid2String(TheUser.Id) + TheUser.LogoExtension) : "";
            if (string.IsNullOrWhiteSpace(TheUser.LogoExtension) ||
            !System.IO.File.Exists(Path.Combine(UserLogoDocumentsAbsolutePath, Guid2String(TheUser.Id) + TheUser.LogoExtension)))
            {
                logoUrl = string.Empty;
            }
            model.Logo = logoUrl;
            data.Add(model);
            return GetReportStream(templateName, data, dataName, fileName, Path.Combine("wwwroot", "certificates"));
        }

        public async Task<FileStreamResult> GenerateSupportLetter(Guid serviceId)
        {
            var gd = Guid2String(serviceId);
            var fileName = $"Certificate_{gd}.docx";
            var request = await _repository.ServiceRequestForPreview(serviceId);
            Dictionary<string, string> replaceParameters = new Dictionary<string, string>();
            foreach (var report in request.ServiceType.ServiceTypeReports)
            {
                var resp = request.Responses.FirstOrDefault(r => r.RequirementId == report.ServiceRequirementId);
                if (resp == null) continue;

                replaceParameters.Add(((SupportLetterReportFields)report.EnumValue).ToString(), resp.Response);
            }
            replaceParameters.Add("AuthorizedBy", request.ApproverName);
            replaceParameters.Add("AuthTitle", request.ApproverTitle);
            var templateFile = Path.Combine(_hostingEnvironment.WebRootPath, "ReportTemplates", "EmbassySupportLetter.docx");
            var fullFileName = Path.Combine(Path.Combine("wwwroot", "certificates"), fileName);

            if (OpenXmlWordEditor.LoadDocument(templateFile, replaceParameters, fullFileName))
            {
                FileStream stream = new FileStream(fullFileName, FileMode.Open);
                Response.Headers.Add("content-disposition", "inline;filename=" + $"{fileName}");
                return File(stream, "application/octet-stream", fileName);
            }
            return null;
        }
        [HttpGet]
        public async Task<IActionResult> DownloadResource(Guid requestId)
        {
            try
            {
                var request = await _repository.UserServiceRequest(TheUser.Id, requestId);
                if (request == null || request.Resource == null || request.RequestStatus != WorkflowStateEnum.Accepted
                    || request.RequestStatus == WorkflowStateEnum.Closed)
                {
                    return NotFound();
                }
                var resource = await _resourceRepository.ById(request.Resource.Id);

                var fileName = $"{resource.Title}.{resource.Extension}";
                var fname = Guid2String(resource.Id) + resource.Extension;
                var path = Path.Combine(AbsolutePath(resource.OwnerModule), fname);
                return await base.DownLoadFile(path, fileName);
            }
            catch (Exception ex)
            {
                return BadRequest("Unable to download resource");
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetResourceUrl(Guid requestId)
        {
            try
            {
                var request = await _repository.UserServiceRequest(TheUser.Id, requestId);
                if (request == null || request.Resource == null || request.RequestStatus != WorkflowStateEnum.Accepted
                    || request.RequestStatus == WorkflowStateEnum.Closed)
                {
                    return NotFound();
                }
                var resource = await _resourceRepository.ById(request.Resource.Id);

                return Ok(resource.Url);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
