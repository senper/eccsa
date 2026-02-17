using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Common.Notification;
using ECCSA.BackOffice.Server.Model.Common.Servicing;
using ECCSA.BackOffice.Server.Model.Membership;
using ECCSA.BackOffice.Server.Model.Payment;
using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Notification;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Servicing;
using ECCSA.BackOffice.Server.Persistence.Repositories.Payment;
using ECCSA.BackOffice.Server.Persistence.Repositories.Security;
using ECCSA.BackOffice.Server.WebApp.Areas.Service.Model;
using ECCSA.BackOffice.Server.WebApp.Controllers;
using ECCSA.BackOffice.Server.WebApp.Models;
using FastReport.Export.PdfSimple;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace ECCSA.BackOffice.Server.WebApp.Areas.Service.Controllers
{
    [Area("Service")]
    public class ArbitratorController : BaseController
    {
        private readonly ILogger<ArbitratorController> _logger;
        private readonly ServiceRequestRepository _repository;
        private readonly ServiceTypeRepository _serviceTypeRepository;
        private readonly InvoiceRepository _invoiceRepository;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ArbitratorController(ILogger<ArbitratorController> logger,
            UserManager<User> userManager,
            ServiceRequestRepository repository,
            ServiceTypeRepository serviceTypeRepository,
             InvoiceRepository invoiceRepository,
             IWebHostEnvironment hostingEnvironment,
            PublicUserRepository userRepository,
            NotificationRepository notificationRepository
            ) : base(userManager, userRepository, notificationRepository)
        {

            _logger = logger;
            _repository = repository;
            _serviceTypeRepository = serviceTypeRepository;
            _invoiceRepository = invoiceRepository;
            _hostingEnvironment = hostingEnvironment;
            controllerName = "ServiceRequest";
        }

        private static string GetCurrentPage(ECCSAModuleFlag module)
        {
            return module switch
            {
                ECCSAModuleFlag.Academy => "Chamber Academy Services",
                ECCSAModuleFlag.Arbitration => "Arbitration Services",
                ECCSAModuleFlag.Communication => "Communication Services",
                ECCSAModuleFlag.Investment => "Trade Promotion Services",
                ECCSAModuleFlag.Membership => "Membership Services",
                ECCSAModuleFlag.Reporting => "Reporting Services",
                ECCSAModuleFlag.Research => "Research & Advocacy Services",
                ECCSAModuleFlag.TradePromotion => "Trade Facilitation Services",
                _ => string.Empty,
            };
        }

        public async Task<IActionResult> Index(ECCSAModuleFlag module, ServiceRequestViewModel searchModel)
        {
            LogStart(_logger, controllerName, "Index");
            var roleLevel = await GetRoleLevel(module);
            if (roleLevel < RoleLevelEnum.LimitedUser)
            {
                LogSecurityError(_logger, controllerName, "Index");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            var userName = TheUser.FullName;
            ServiceRequestViewModel model;
            model = new ServiceRequestViewModel
            {
                FullName = userName,
                CurrentPage = GetCurrentPage(module),
                CurrentOperation = "List",
                TheModel = new ServiceRequest()
                {
                    Id = Guid.Empty,
                }

            };
            searchModel.SearchStatus = searchModel.SearchStatus ?? 1;
            try
            {
                model.ServiceTypes = await _serviceTypeRepository.ServiceTypes(module);
            }
            catch (Exception ex)
            {
                LogError(_logger, ex, "Arbitrator", "Index", null);
            }

            model.SearchStatus = searchModel.SearchStatus;
            if (searchModel.SearchKey == null && searchModel.SearchStatus == -1)
            {
                model.TheList = await _repository.Services(module);
            }
            else
            {
                if (searchModel.SearchKey != null)
                {
                    var tmp = await _repository.Services(module, searchModel.SearchKey);
                    model.TheList = tmp;
                    if (searchModel.SearchStatus > 0)
                    {
                        model.TheList = tmp.Where(s => s.RequestStatus == (WorkflowStateEnum)searchModel.SearchStatus).ToList();
                    }
                }
                else
                {
                    model.TheList = await _repository.Services(searchModel.Module, (WorkflowStateEnum)searchModel.SearchStatus);
                }
            }
            model.CurrentModule = module;
            model.RoleLevel = roleLevel;
            model.ChangeActive(MenuItemEnum.Services);
            TempData["title"] = GetCurrentPage(module);
            _logger.LogInformation("ServiceRequest Index", "End", DateTime.UtcNow);
            return View(model);
        }

     
        public async Task<IActionResult> Detail(Guid id, Guid serviceTypeId, int activeTabIndex = 0)
        {
            var userName = TheUser.FullName;
            ServiceRequestViewModel model;
            LogStart(_logger, controllerName, "Index");

            if (Guid.Empty == id)
            {
                return NotFound();
            }
            model = new ServiceRequestViewModel
            {
                FullName = userName,
                CurrentOperation = "Detail",
                TheList = new List<ServiceRequest>(),
                TheModel = new ServiceRequest()

            };
            try
            {
                model.TheModel = await _repository.ServiceRequestById(id, TheUser.Id);
            }
            catch (Exception ex)
            {
                LogError(_logger, ex, "ServiceRequest", "Detail", new { Id = id, UserId = TheUser.Id });
                return RedirectToAction("Error", "Home", new { area = "" });
            }
            if (model.TheModel == null)
            {
                return RedirectToAction("Error", "Home", new { area = "" });
            }
            var serviceOwner = model.TheModel.ServiceType.ServiceOwner;
            var roleLevel = await GetRoleLevel(serviceOwner);
            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Index");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            if (model.TheModel.ServiceType.IsArbitration &&
                (model.TheModel.RequestStatus == WorkflowStateEnum.Submitted
                || model.TheModel.RequestStatus == WorkflowStateEnum.UnderReview))
            {
                model.OpponentUsers = await GetPublicUsers();
                model.OpponentUsers = model.OpponentUsers.Where(s => s.Id != model.TheModel.RequestingUserId).ToList();
            }
            if (!string.IsNullOrEmpty(model.TheModel.InvoiceNo))
            {
                if (model.TheModel.InvoiceNo.Trim().Length > 3)
                {
                    var invoice = await _invoiceRepository.InvoiceByNo(model.TheModel.InvoiceNo, model.TheModel.Id);
                    model.TheModel.InvoiceDate = invoice.InvoiceDate.Date;
                    model.TheModel.Today = DateTime.Today;
                }
            }
            if (model.TheModel.ServiceType.AllowAdditionalAttachments)
            {
                foreach (var att in model.TheModel.Attachments)
                {
                    var fileName = Guid2String(att.Id);
                    att.RelativePath = Path.Combine(RelativePath(serviceOwner), $"{fileName}{att.Extension}");
                }
            }
            if (model.TheModel.Invoice?.Settlments.Count > 0)
            {
                foreach (var ss in model.TheModel.Invoice.Settlments)
                {
                    var fileName = Guid2String(ss.Id);
                    ss.RelativePath = Path.Combine(RelativePath(serviceOwner), "receipts", $"{fileName}{ss.Extension}");
                }
            }
            foreach (var res in model.TheModel.Responses)
            {
                if (res.Requirement.InputMethod == ServiceInputTypeEnum.Upload)
                {
                    var fileName = Guid2String(res.Id);
                    var filePath = AbsolutePath(serviceOwner);
                    var extension = GetExtension(filePath, fileName);
                    res.RelativePath = Path.Combine(RelativePath(serviceOwner), $"{fileName}{extension}");
                }
                else if (res.Requirement.InputMethod == ServiceInputTypeEnum.LegalDocument)
                {
                    var userPath = Guid2String(model.TheModel.RequestingUserId);
                    var filePath = Path.Combine(UserLegalProfileDocumentsAbsolutePath, userPath);
                    var fileName = Guid2String(res.LookupValueId.Value);
                    var extension = GetExtension(filePath, fileName);
                    res.RelativePath = Path.Combine(UserLegalProfileDocumentsRelativePath, userPath, $"{fileName}{extension}");
                }
                else if (res.Requirement.InputMethod == ServiceInputTypeEnum.Payment)
                {
                    if (res.Invoice?.Settlments.Count > 0)
                    {
                        foreach (var ss in res.Invoice.Settlments)
                        {
                            var fileName = Guid2String(ss.Id);
                            ss.RelativePath = Path.Combine(RelativePath(serviceOwner), "receipts", $"{fileName}{ss.Extension}");
                        }
                    }
                }
            }
            model.CurrentModule = serviceOwner;


            model.RoleLevel = roleLevel;
            model.CurrentPage = GetCurrentPage(model.CurrentModule);
            model.IsCurrentOperator = false;
            model.ServiceTypeId = serviceTypeId;
            model.ActiveTabIndex = activeTabIndex;
            if (model.TheModel.ServiceTypeId == serviceTypeId)
            {
                model.ChangeActive(MenuItemEnum.Events);
            }
            else
            {
                model.ChangeActive(MenuItemEnum.Services);
            }
            if (model.RoleLevel == RoleLevelEnum.Admin)
            {
                model.Users = await _repository.Users(model.CurrentModule);
            }
            if (model.TheModel.UnderReview)
            {
                model.IsCurrentOperator = !string.IsNullOrEmpty(model.TheModel.OperatorName)
                    && model.TheModel.OperatorName.ToLowerInvariant() == TheUser.FullName.ToLowerInvariant();
            }
            if (model.TheModel.RequestStatus == WorkflowStateEnum.Accepted ||
                model.TheModel.RequestStatus == WorkflowStateEnum.Closed)
            {

                var fileName = $"Certificate_{Guid2String(model.TheModel.Id)}.pdf";
                var relativePath = Path.Combine("/", "certificates", fileName);
                var absolutePath = Path.Combine(
                                        Directory.GetCurrentDirectory(),
                                        "wwwroot", "certificates",
                                        fileName);
                model.TheModel.CertificateUrl = relativePath;
                if (!System.IO.File.Exists(absolutePath))
                {
                    model.TheModel.CertificateUrl = String.Empty;
                }
            }
            TempData["title"] = model.CurrentPage;
            LogEnd(_logger, controllerName, "Detail");
            return View(model);
        }


     

        private static FileStream GetCurrentFile(string fileName)
        {
            var outFolder = Path.GetTempPath();
            if (!Directory.Exists(outFolder))
                Directory.CreateDirectory(outFolder);

            var tmpFileName = Path.Combine(outFolder, fileName);
            if (System.IO.File.Exists(tmpFileName))
            {
                return new FileStream(tmpFileName, FileMode.Open);
            }
            return null;
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReviewResponse(ReviewResponseModel model)
        {
            try
            {
                LogStart(_logger, controllerName, "GenerateRequirementInvoice");
                var request = await CheckSecurity(model.ResponseId, RoleLevelEnum.User, "ReviewResponse");
                if (request == null)
                {

                    return RedirectToAction("Detail", new { id = model.ResponseId });
                }
                var response = request.Responses.FirstOrDefault(r => r.Id == model.Id);
                var requirement = request.ServiceType.ServiceRequirements.FirstOrDefault(x => x.Id == response.RequirementId);
                if (requirement == null)
                {
                    CommandStatus("Requirement Not found");
                    return RedirectToAction("Detail", new { id = model.ResponseId });
                }
                if (requirement.InputMethod == ServiceInputTypeEnum.Payment
                    && requirement.RequirementType == RequirementTypeEnum.Invoice)
                {
                    if (model.Status == DocumentReviewStatusEnum.Accepted)
                    {
                        if (string.IsNullOrEmpty(response.InvoiceNo)
                            || string.IsNullOrEmpty(response.ReceiptNo)
                            || !response.InvoiceAmount.HasValue
                            || !response.AmountPaid.HasValue
                            || response.InvoiceAmount.Value - response.AmountPaid.Value > 0.01m)
                        {
                            return RedirectToAction("Detail", new { id = request.Id });
                        }
                    }
                }
                await _repository.ReviewRequirement(model.Id, model.ResponseId, model.Status, model.Remark, TheUser.Id);
                // CommandStatus(CommandStatusEnum.Saved);
            }
            catch (Exception ex)
            {
                LogError(_logger, ex, controllerName, "ReviewResponse", model);
                TempData["CommandStatus"] = ex.Message;
            }

            return RedirectToAction("Detail", new { id = model.ResponseId });

        }

        private FileStream GetReportStream<T>(string reportName, IEnumerable<T> data, string dataName, string fileName)
        {
            var outFolder = Path.GetTempPath();
            if (!Directory.Exists(outFolder))
                Directory.CreateDirectory(outFolder);

            var tmpFileName = Path.Combine(outFolder, fileName);
            FastReport.Report report = new();
            report.Load(Path.Combine(_hostingEnvironment.WebRootPath, "ReportTemplates", reportName));
            report.RegisterData(data, dataName);
            report.Prepare();
            PDFSimpleExport pdfExport = new();
            pdfExport.Export(report, tmpFileName);
            report.Dispose();

            Response.Headers.Add("content-disposition", "inline;filename=" + fileName);
            return new FileStream(tmpFileName, FileMode.Open);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StartProcessing(Guid id)
        {
            try
            {
                await _repository.StartProcessing(id, TheUser.Id);
                // CommandStatus(CommandStatusEnum.Saved);
            }
            catch (Exception ex)
            {
                LogError(_logger, ex, controllerName, "StartProcessing", id);
                TempData["CommandStatus"] = ex.Message;
            }

            return RedirectToAction("Detail", new { id });

        }

      public async Task<IActionResult> Save(IFormCollection formCollection)
        {
            var message = string.Empty;
            var isValid = true;
            Guid serviceTypeId = Guid.Parse(formCollection["ServiceTypeId"]);
            var serviceType = await _serviceTypeRepository.ServiceTypeById(serviceTypeId);
            var serviceOwner = serviceType.ServiceOwner;
            ServiceRequestViewModel model = new();
            ServiceRequest request = new()
            {
                Id = Guid.Empty,
                ServiceTypeId = serviceTypeId,
                Responses = new List<ServiceRequirementResponse>()
            };
            foreach (var req in serviceType.ServiceRequirements)
            {
                var key = model.Guid2String(req.Id);
                var val = formCollection[key];
                var resp = new ServiceRequirementResponse
                {
                    Id = Guid.NewGuid(),
                    ServiceRequestId = Guid.Empty,
                    RequirementId = req.Id,
                };
                switch (req.InputMethod)
                {
                    case ServiceInputTypeEnum.Upload:
                        if (formCollection.Files[key] != null)
                        {
                            var fileName = formCollection.Files[key].FileName;
                            await SaveDocument(resp.Id, serviceOwner, formCollection.Files[key]);
                            resp.TextResponse = Path.GetExtension(Path.GetFileName(fileName));
                        }
                        break;
                    case ServiceInputTypeEnum.Integer:
                        //  case ServiceInputTypeEnum.SingleSelect:
                        resp.IntResponse = int.Parse(val);
                        break;
                    case ServiceInputTypeEnum.Decimal:
                        resp.DecimalResponse = decimal.Parse(val);
                        break;
                    case ServiceInputTypeEnum.Date:
                        resp.DateResponse = DateTime.Parse(val);
                        break;
                    case ServiceInputTypeEnum.Email:

                        resp.TextResponse = val;
                        break;
                    case ServiceInputTypeEnum.Mobile:
                    case ServiceInputTypeEnum.Phone:
                        resp.TextResponse = val;
                        break;
                    case ServiceInputTypeEnum.ShortText:
                    case ServiceInputTypeEnum.LongText:

                    default:
                        resp.TextResponse = val;
                        break;

                }

                request.Responses.Add(resp);
            }
            if (isValid)
            {
                await _repository.Save(request, TheUser.Id);
            }
            else
            {
                CommandStatus(message);
            }
            return RedirectToAction("Index", new { module = serviceType.ServiceOwner });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatus(ChangeStatusModel model)
        {
            _logger.LogInformation("ServiceRequest", "ChangeStatus", "Started", DateTime.UtcNow);
            var request = await _repository.ServiceRequestById(model.Id, TheUser.Id);
            var serviceOwner = request.ServiceType.ServiceOwner;
            CommandStatusEnum statusEnum;
            model.Code = string.Empty;
            model.Extension = string.Empty;
            if (model.Status == WorkflowStateEnum.Accepted ||
                model.Status == WorkflowStateEnum.DraftPrepared)
            {
                if (model.File == null || model.File.Length == 0 || request.PendingDocumentsNo != 0)
                {
                    TempData["CommandStatus"] = "Attach the document";
                    CommandStatus("Attach the document");
                    return RedirectToAction("Detail", new { id = model.Id });
                }
                await SaveDocument(model.Id, serviceOwner, model.File);
                var fileName = model.File.FileName;
                model.Extension = Path.GetExtension(Path.GetFileName(fileName));
                model.Code = await GetHashCode(model.File);
                statusEnum = CommandStatusEnum.Approved;
            }
            else if (model.Status == WorkflowStateEnum.Rejected)
            {
                if (string.IsNullOrEmpty(model.Remark))
                {
                    CommandStatus("Please provide reject reason");
                    return RedirectToAction("Detail", new { id = model.Id });
                }
                statusEnum = CommandStatusEnum.Rejected;
            }
            else
            {
                statusEnum = CommandStatusEnum.Saved;

                if (model.File != null && model.File.Length > 0)
                {
                    await SaveDocument(model.Id, serviceOwner, model.File);
                    var fileName = model.File.FileName;
                    model.Extension = Path.GetExtension(Path.GetFileName(fileName));
                }

            }
            try
            {
                await _repository.ChangeRequestStatus(model.Id, model.Status, model.Code, model.Extension, model.Remark, TheUser.Id);
                if (model.Status == WorkflowStateEnum.Rejected || model.Status == WorkflowStateEnum.Accepted)
                {
                    //
                    var subject = $"Service Status Changed to {model.Status}";
                    var body = string.IsNullOrEmpty(model.Remark) ? "Dear, This is just to inform you that the status of your request has changed" :
                                $"Dear, The following reason is provided. {model.Remark}";
                    var recepients = new List<INotificationAddress> { request };
                    if (request.ServiceType.IsArbitration && request.OpponentUserId != null)
                    {
                        var opR = new ServiceRequest { Email = request.OpponentEmail, MobileNo = request.OpponentMobileNo, FullName = request.OpponentFullName };
                        recepients.Add(opR);
                    }
                    await SaveNotification(request.ServiceType.ServiceOwner, subject, body,
                        recepients,
                        model.File, SendThroughFlag.Email | SendThroughFlag.SMS);
                    //
                }
                CommandStatus(statusEnum);
            }
            catch (Exception ex)
            {
                LogError(_logger, ex, controllerName, "ChangeStatus", model);
                if (ex.InnerException != null)
                {
                    TempData["CommandStatus"] = ex.InnerException.Message;
                }
                else
                {
                    TempData["CommandStatus"] = ex.Message;
                }
            }
            _logger.LogInformation("ServiceRequest", "ChangeStatus", "End", DateTime.UtcNow);
            return RedirectToAction("Detail", new { id = model.Id });
        }

        private async Task<ServiceRequest> CheckSecurity(Guid requestId, RoleLevelEnum minLevel, string operationName)
        {
            var request = await _repository.ServiceRequestById(requestId, TheUser.Id);
            var serviceOwner = request.ServiceType.ServiceOwner;
            var roleLevel = await GetRoleLevel(serviceOwner);
            bool validUser;
            if (minLevel == RoleLevelEnum.User)
            {
                validUser = request.OperatorName == TheUser.FullName;
            }
            else
            {
                validUser = roleLevel == RoleLevelEnum.Admin;
                request.ApproverName = TheUser.FullName;
            }
            if (roleLevel < minLevel)
            {
                _logger.LogError(controllerName, operationName, "Insufficient Privilege", TheUser, DateTime.UtcNow);
                TempData["CommandStatus"] = "Insufficient Privilege";
                return null;
            }
            else if (!validUser)
            {
                _logger.LogError(controllerName, operationName, "Task owned by other user", TheUser, DateTime.UtcNow);
                TempData["CommandStatus"] = "Task owned by other user";
                return null;
            }

            return request;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RecommendRejection(ChangeStatusModel model)
        {
            _logger.LogInformation("ServiceRequest", "RecommendRejection", "Started", DateTime.UtcNow);
            var request = await CheckSecurity(model.Id, RoleLevelEnum.User, "RecommendRejection");

            if (request == null)
            {
                return RedirectToAction("Detail", new { id = model.Id });
            }
            model.Code = string.Empty;
            model.Extension = string.Empty;


            if (string.IsNullOrEmpty(model.Remark))
            {
                CommandStatus("Please provide reject reason");
                return RedirectToAction("Detail", new { id = model.Id });
            }
            try
            {
                await _repository.RecommendRejection(model.Id, model.Remark, TheUser.Id);
                var roleLevel = await GetRoleLevel(request.ServiceType.ServiceOwner);

                if (roleLevel == RoleLevelEnum.Admin || !request.ServiceType.ApprovalRequired)
                {
                    await _repository.ApproveRejectionRequest(model.Id, TheUser.Id, model.Remark);
                }
                CommandStatus(CommandStatusEnum.Rejected);
            }
            catch (Exception ex)
            {
                LogError(_logger, ex, controllerName, "RecommendRejection", model);
                if (ex.InnerException != null)
                {
                    TempData["CommandStatus"] = ex.InnerException.Message;
                }
                else
                {
                    TempData["CommandStatus"] = ex.Message;
                }
            }
            _logger.LogInformation("ServiceRequest", "RecommendRejection", "End", DateTime.UtcNow);
            return RedirectToAction("Detail", new { id = model.Id });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RecommendApprove(ChangeStatusModel model)
        {
            LogStart(_logger, controllerName, "RecommendApprove");
            var request = await CheckSecurity(model.Id, RoleLevelEnum.User, "RecommendApprove");
            if (request == null)
            {
                return RedirectToAction("Detail", new { id = model.Id });
            }

            model.Code = string.Empty;
            model.Extension = string.Empty;
            var template = request.ServiceType.ReportTemplate;
            if (request.PendingDocumentsNo > 0)
            {
                TempData["CommandStatus"] = $"There are {request.PendingDocumentsNo} pending documents";
                return RedirectToAction("Detail", new { id = model.Id });
            }
            else if (!string.IsNullOrEmpty(template))
            {
                //TODO: generate report
            }
            else if ((model.File != null && model.File.Length > 0))
            {
                await SaveDocument(model.Id, request.ServiceType.ServiceOwner, model.File);
                var fileName = model.File.FileName;
                model.Extension = Path.GetExtension(Path.GetFileName(fileName));
                model.Code = await GetHashCode(model.File);

            }
            else if (request.ServiceType.ApprovalAttachmentRequired)
            {
                TempData["CommandStatus"] = "Attach the document";
                CommandStatus("Attach the document");
                return RedirectToAction("Detail", new { id = model.Id });
            }

            try
            {
                await _repository.RecommendApprove(model.Id, model.Extension, model.Code, TheUser.Id);
                var roleLevel = await GetRoleLevel(request.ServiceType.ServiceOwner);

                if (roleLevel == RoleLevelEnum.Admin || !request.ServiceType.ApprovalRequired)
                {
                    await _repository.ApproveAcceptRequest(model.Id, TheUser.Id);
                }

                //
                var subject = $"Service Request {request.TrackingNo} is approved.";
                var body = $"Dear, This is just to inform you that the your request {request.TrackingNo} is approved.";
                var recepients = new List<INotificationAddress> { request };
                if (request.ServiceType.IsArbitration && request.OpponentUserId != null)
                {
                    var opR = new ServiceRequest { Email = request.OpponentEmail, MobileNo = request.OpponentMobileNo, FullName = request.OpponentFullName };
                    recepients.Add(opR);
                }
                await SaveNotification(request.ServiceType.ServiceOwner, subject, body,
                    recepients,
                    model.File, SendThroughFlag.Email | SendThroughFlag.SMS);
                //

                CommandStatus(CommandStatusEnum.Approved);
            }
            catch (Exception ex)
            {
                LogError(_logger, ex, controllerName, "RecommendApprove", model);

                if (ex.InnerException != null)
                {
                    TempData["CommandStatus"] = ex.InnerException.Message;
                }
                else
                {
                    TempData["CommandStatus"] = ex.Message;
                }
            }
            LogEnd(_logger, controllerName, "RecommendApprove");
            return RedirectToAction("Detail", new { id = model.Id });
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveFollowup(FollowupStatusModel model)
        {
            var request = await _repository.ServiceRequestById(model.ServiceId, TheUser.Id);
            var serviceOwner = request.ServiceType.ServiceOwner;
            model.Description = String.IsNullOrEmpty(model.Description) ? String.Empty : model.Description;
            var extension = string.Empty;
            model.Id = model.Id == Guid.Empty ? Guid.NewGuid() : model.Id;
            model.ServiceTypeId = request.ServiceTypeId;
            try
            {
                var booked = await _repository.IsBooked(model);
                if (booked)
                {
                    CommandStatus("Date and Time already booked by another service");
                    return RedirectToAction("Detail", new { id = model.ServiceId, model.ServiceTypeId, ActiveTabIndex = 1 });
                }
                if (model.File != null && model.File.Length > 0)
                {
                    await SaveDocument(model.Id, serviceOwner, model.File);
                    string fileName = model.File.FileName;
                    extension = Path.GetExtension(Path.GetFileName(fileName));
                    model.Extension = extension;
                }
                await _repository.Save(model, TheUser.Id);

                //
                var subject = $"Appointment for {request.TrackingNo}  ";
                var body = $"Dear {request.FullName}, This is just to inform you that you have appointment for {model.Title} on {model.StartDateStr} {model.StartTime}";
                var recepients = new List<INotificationAddress> { request };
                if (request.ServiceType.IsArbitration && request.OpponentUserId != null)
                {
                    var opR = new ServiceRequest { Email = request.OpponentEmail, MobileNo = request.OpponentMobileNo, FullName = request.OpponentFullName };
                    recepients.Add(opR);
                }
                await SaveNotification(request.ServiceType.ServiceOwner, subject, body,
                        recepients,
                        model.File, SendThroughFlag.Email | SendThroughFlag.SMS);
                //
                // CommandStatus(CommandStatusEnum.Saved);
            }
            catch (Exception ex)
            {
                LogError(_logger, ex, controllerName, "SaveFollowup", model);
                if (ex.InnerException != null)
                {
                    TempData["CommandStatus"] = ex.InnerException.Message;
                }
                else
                {
                    TempData["CommandStatus"] = ex.Message;
                }
            }
            return RedirectToAction("Detail", new { id = model.ServiceId, model.ServiceTypeId, ActiveTabIndex = 1 });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteFollowup(Guid id, Guid serviceId, Guid serviceTypeId)
        {
            var request = await _repository.ServiceRequestById(serviceId, TheUser.Id);
            var serviceOwner = request.ServiceType.ServiceOwner;
            var extension = string.Empty;
            try
            {
                var fileName = Guid2String(id);
                var filePath = AbsolutePath(serviceOwner) + "\\" + fileName;
                await _repository.DeleteFollowup(id, serviceId, TheUser.Id, filePath);
                CommandStatus(CommandStatusEnum.Deleted);
            }
            catch (Exception ex)
            {
                LogError(_logger, ex, controllerName, "DeleteFollowup", id);
                if (ex.InnerException != null)
                {
                    TempData["CommandStatus"] = ex.InnerException.Message;
                }
                else
                {
                    TempData["CommandStatus"] = ex.Message;
                }
            }
            return RedirectToAction("Detail", new { id = serviceId, serviceTypeId, activeTabIndex = 1 });
        }
        [HttpGet]
        public async Task<FileResult> DownLoadFile(Guid id, string title, string extension, ECCSAModuleFlag module)
        {
            var fileName = Guid2String(id);
            var filePath = AbsolutePath(module);
            if (string.IsNullOrEmpty(extension))
            {
                extension = GetExtension(filePath, fileName);
            }
            fileName += extension;
            title += extension;
            return await DownLoadFile(filePath + "\\" + fileName, title);
        }
     
        public async Task<IActionResult> Appointments(ECCSAModuleFlag module, AppointmentViewModel model)
        {
            LogStart(_logger, controllerName, "Appointments");
            var roleLevel = await GetRoleLevel(module);
            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Index");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            model.CurrentModule = module;
            model.CurrentPage = "Appointment";
            model.CurrentOperation = "List";
            model.RoleLevel = roleLevel;
            model.TheList = await _repository.AppointmentList(module, model.FromDate, model.ToDate);
            model.ChangeActive(MenuItemEnum.Appointments);
            TempData["title"] = GetCurrentPage(module);
            _logger.LogInformation("ServiceRequest Index", "End", DateTime.UtcNow);
            return View(model);
        }
    }
}
