using DocumentFormat.OpenXml.Wordprocessing;
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
using ECCSA.BackOffice.Server.WebApp.Services;
using FastReport.Export.PdfSimple;
using Microsoft.AspNetCore.Hosting;
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
    public class ServiceRequestController : BaseController
    {
        private readonly ILogger<ServiceRequestController> _logger;
        private readonly ServiceRequestRepository _repository;
        private readonly ServiceTypeRepository _serviceTypeRepository;
        private readonly InvoiceRepository _invoiceRepository;
        private readonly ExportAgreementRepository _exportAgreementRepository;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ServiceRequestController(ILogger<ServiceRequestController> logger,
            UserManager<User> userManager,
            ServiceRequestRepository repository,
            ServiceTypeRepository serviceTypeRepository,
             InvoiceRepository invoiceRepository,
             IWebHostEnvironment hostingEnvironment,
            PublicUserRepository userRepository,
            ExportAgreementRepository exportAgreementRepository,
            NotificationRepository notificationRepository
            ) : base(userManager, userRepository, notificationRepository)
        {

            _logger = logger;
            _repository = repository;
            _serviceTypeRepository = serviceTypeRepository;
            _exportAgreementRepository = exportAgreementRepository;
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
            if (TheUser.IsArbitrator)
            {
                return RedirectToAction("Index", "Arbitrator", new { area = "Service", module });
            }
            if (roleLevel <= RoleLevelEnum.None)
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
                LogError(_logger, ex, "ServiceRequest", "Index", null);
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

        public async Task<IActionResult> ByServiceType(ECCSAModuleFlag module, Guid serviceTypeId, string title)
        {
            LogStart(_logger, controllerName, "ByServiceType");
            var roleLevel = await GetRoleLevel(module);
            if (roleLevel <= RoleLevelEnum.None)
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
                CurrentOperation = title,
                TheList = await _repository.ServicesByServiceType(module, serviceTypeId),
                TheModel = new ServiceRequest()
                {
                    Id = Guid.Empty,
                }

            };

            model.CurrentModule = module;
            model.RoleLevel = roleLevel;
            model.ChangeActive(MenuItemEnum.Events);
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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GenerateInvoice(Guid requestId, decimal rate)
        {
            try
            {
                YesNoEnum asMember = YesNoEnum.NotSet;
                var request = await _repository.ServiceRequestById(requestId, TheUser.Id);
                if (request.ServiceType.RateBasis == RateBasisEnum.Membership)
                {
                    asMember = rate == request.ServiceType.MemberServiceCharge ? YesNoEnum.Yes : YesNoEnum.No;
                }

                var simpleInvoice = new Invoice()
                {
                    SubSystem = request.ServiceType.ServiceOwner,
                    ReferenceId = requestId,
                    BasePrice = decimal.Round(rate / 1.15m, 2, MidpointRounding.ToEven),
                    Discount = 0,
                    TotalAmount = rate,
                    CompanyName = request.FullName,
                    TIN = request.TIN,
                    Address = request.Address,
                    PaymentReason = request.ServiceType.NameEng
                };
                var invoice = await _invoiceRepository.GetOrCreate(simpleInvoice, TheUser.Id);
                await _repository.RecordInvoice(requestId, invoice.InvoiceNo, rate, asMember, TheUser.Id);

                CommandStatus(CommandStatusEnum.InvoiceGenerated);
            }
            catch (Exception ex)
            {
                CommandStatus(CommandStatusEnum.UnkownError);
                TempData["CommandStatus"] = ex.Message;
            }

            return RedirectToAction("Detail", new { id = requestId });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GenerateRequirementInvoice(Guid requestId, Guid id, decimal rate)
        {
            try
            {
                YesNoEnum asMember = YesNoEnum.NotSet;
                LogStart(_logger, controllerName, "GenerateRequirementInvoice");
                var request = await CheckSecurity(requestId, RoleLevelEnum.User, "GenerateRequirementInvoice");
                if (request == null)
                {
                    return RedirectToAction("Detail", new { id = requestId });
                }
                if (rate < 0.01m)
                {
                    TempData["CommandStatus"] = "Rate is required";
                    return RedirectToAction("Detail", new { id = requestId });
                }
                var response = request.Responses.FirstOrDefault(r => r.Id == id);
                var requirement = request.ServiceType.ServiceRequirements.FirstOrDefault(x => x.Id == response.RequirementId);
                if (requirement == null
                    || requirement.InputMethod != ServiceInputTypeEnum.Payment
                    || requirement.RequirementType != RequirementTypeEnum.Invoice)
                {
                    return RedirectToAction("Detail", new { id = requestId });
                }


                var simpleInvoice = new Invoice()
                {
                    SubSystem = request.ServiceType.ServiceOwner,
                    ReferenceId = id,
                    BasePrice = decimal.Round(rate / 1.15m, 2, MidpointRounding.ToEven),
                    Discount = 0,
                    TotalAmount = rate,
                    CompanyName = request.FullName,
                    TIN = request.TIN,
                    Address = request.Address,
                    PaymentReason = request.ServiceType.NameEng
                };
                var invoice = await _invoiceRepository.GetOrCreate(simpleInvoice, TheUser.Id);
                await _repository.RecordRequirementInvoice(requestId, id, invoice.InvoiceNo, simpleInvoice.TotalAmount, asMember, TheUser.Id);

                CommandStatus(CommandStatusEnum.InvoiceGenerated);
            }
            catch (Exception ex)
            {
                CommandStatus(CommandStatusEnum.UnkownError);
                TempData["CommandStatus"] = ex.Message;
            }

            return RedirectToAction("Detail", new { id = requestId });

        }


        [HttpGet]
        public async Task<IActionResult> PrintInvoice(Guid referenceId, string invoiceNo)
        {
            Invoice invoice = await _invoiceRepository.InvoiceByNo(invoiceNo, referenceId);
            var pi = new PrintInvoice(invoice);
            var fileName = $"Invoice_{pi.InvoiceNo}.pdf";
            var stream = GetCurrentFile(fileName);
            if (stream != null)
            {
                Response.Headers.Add("content-disposition", "inline;filename=" + fileName);
                return File(stream, "application/pdf");
            }
            try
            {
                var request = await _repository.ServiceRequestById(invoice.ReferenceId, TheUser.Id);
                pi.PaymentReason = request.ServiceType.NameEng;
                var data = new List<PrintInvoice>
                {
                    pi
                };
                stream = GetReportStream<PrintInvoice>("Invoice.frx", data, "PrintInvoice", fileName);
                return File(stream, "application/pdf");
            }
            catch (Exception ex)
            {
                TempData["CommandStatus"] = ex.Message;
            }
            return RedirectToAction("Detail", new { id = invoice.ReferenceId });
        }

        [HttpGet]
        public async Task<IActionResult> PrintReceipt(Guid referenceId, string invoiceNo, int no = 0)
        {
            Invoice invoice = await _invoiceRepository.InvoiceByNo(invoiceNo, referenceId);
            var pi = new PrintInvoice(invoice);

            if (invoice.Settlments == null || invoice.Settlments.Count < no + 1)
            {
                TempData["CommandStatus"] = "Receipt Not found";
                return RedirectToAction("Detail", new { id = invoice.ReferenceId });
            }
            var fileName = $"Receipt_{pi.InvoiceNo}_{pi.Settlements[no].ReceiptNo}.pdf";
            var stream = GetCurrentFile(fileName);
            if (stream != null)
            {
                Response.Headers.Add("content-disposition", "inline;filename=" + fileName);
                return File(stream, "application/pdf");
            }
            try
            {

                var request = await _repository.ServiceRequestById(invoice.ReferenceId, TheUser.Id);
                pi.PaymentReason = request.ServiceType.NameEng;
                var data = new List<PrintInvoice>
                {
                    pi
                };
                stream = GetReportStream<PrintInvoice>("Receipt.frx", data, "PrintInvoice", fileName);

                return File(stream, "application/pdf");
            }
            catch (Exception ex)
            {
                TempData["CommandStatus"] = ex.Message;
            }

            return RedirectToAction("Detail", new { id = invoice.ReferenceId });
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
                var request = await _repository.ServiceRequestById(id, TheUser.Id);
                if (request == null)
                {
                    return RedirectToAction("Detail", new { id = id });
                }
                await _repository.StartProcessing(id, TheUser.Id);
                // CommandStatus(CommandStatusEnum.Saved);
                var subject = $"Service Request {request.TrackingNo} is now under review.";
                var body = $"Dear, This is just to inform you that the your request {request.TrackingNo} is now under review.";
                await SaveNotification(request.ServiceType.ServiceOwner, subject, body,
                        new List<INotificationAddress> { request },
                        null, SendThroughFlag.Email | SendThroughFlag.SMS);
            }
            catch (Exception ex)
            {
                LogError(_logger, ex, controllerName, "StartProcessing", id);
                TempData["CommandStatus"] = ex.Message;
            }

            return RedirectToAction("Detail", new { id });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignUser(Guid id, Guid userId)
        {
            try
            {
                await _repository.AssignUser(id, userId, TheUser.Id);
                // CommandStatus(CommandStatusEnum.Saved);
            }
            catch (Exception ex)
            {
                LogError(_logger, ex, controllerName, "AssignUser", id);
                TempData["CommandStatus"] = ex.Message;
            }

            return RedirectToAction("Detail", new { id });

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
                if (model.Status == DocumentReviewStatusEnum.Rejected)
                {
                    //
                    var subject = $"Service Request {request.TrackingNo} response for req. {requirement.Label} is Rejected.";
                    var body = $"Dear, This is just to inform you that the your response for service request " +
                        $"{request.TrackingNo} for requirement {requirement.Label} is rejected with remark {model.Remark}.";
                    var recepients = new List<INotificationAddress> { request };

                    await SaveNotification(request.ServiceType.ServiceOwner, subject, body,
                        recepients,
                        null, SendThroughFlag.Email | SendThroughFlag.SMS);
                    //
                }
            }
            catch (Exception ex)
            {
                LogError(_logger, ex, controllerName, "ReviewResponse", model);
                TempData["CommandStatus"] = ex.Message;
            }

            return RedirectToAction("Detail", new { id = model.ResponseId });

        }

        public async Task<IActionResult> Edit(Guid id, Guid serviceTypeId)
        {
            var userName = TheUser.FullName;
            ServiceRequestViewModel model;
            var serviceType = await _serviceTypeRepository.ServiceTypeById(serviceTypeId);
            model = new ServiceRequestViewModel
            {
                FullName = userName,
                CurrentPage = GetCurrentPage(serviceType.ServiceOwner),
                CurrentOperation = "Detail",
                TheList = new List<ServiceRequest>(),


            };
            if (Guid.Empty == id)
            {
                model.TheModel = new ServiceRequest()
                {
                    Id = Guid.Empty,
                    ServiceTypeId = serviceTypeId,
                    ServiceType = serviceType,

                };
            }
            else
            {
                model.TheModel = await _repository.ServiceRequestById(id, TheUser.Id);
                model.TheModel.ServiceType = serviceType;
            }



            model.ChangeActive(MenuItemEnum.Services);
            model.CurrentModule = serviceType.ServiceOwner;
            TempData["title"] = GetCurrentPage(serviceType.ServiceOwner);
            return View(model);
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
        public async Task<IActionResult> ConfirmPayment(ConfirmPaymentModel model)
        {
            LogStart(_logger, controllerName, "ConfirmPayment");
            var request = await CheckSecurity(model.Id, RoleLevelEnum.User, "ConfirmPayment");
            if (request == null)
            {
                return RedirectToAction("Detail", new { id = model.Id });
            }

            try
            {
                var settlement = await _invoiceRepository.ConfirmPayment(request.Id, model.InvoiceNo, model.ReceiptNo, model.ConfirmedAmount, TheUser.Id);
                await _repository.ApproveReceiptPrint(model.Id, settlement.Invoice.TotalPaid, settlement.Status, TheUser.Id);
                if (settlement.AmountPaid < model.ConfirmedAmount)
                {
                    CommandStatus("Confirmed Amount is greater than amount required.");
                }
                if (settlement.AmountPaid > model.ConfirmedAmount)
                {
                    CommandStatus("Insufficient Balance");
                }
                else
                {
                    CommandStatus(CommandStatusEnum.Approved);
                }

                //
                var partial = settlement.AmountPaid > model.ConfirmedAmount ? "Partially" : "";
                var subject = $"Service Request {request.TrackingNo} Payment for Invoice/Receipt {model.InvoiceNo}/{model.ReceiptNo} confirmed {partial} with amount {model.ConfirmedAmount}.";
                var body = $"Dear, This is just to inform you that the your payment for service request " +
                    $"{request.TrackingNo} for Invoice {model.InvoiceNo} and for receipt {model.ReceiptNo} is confirmed with amount {model.ConfirmedAmount}.";
                var recepients = new List<INotificationAddress> { request };

                await SaveNotification(request.ServiceType.ServiceOwner, subject, body,
                    recepients,
                    null, SendThroughFlag.Email | SendThroughFlag.SMS);
                //

            }
            catch (Exception ex)
            {
                LogError(_logger, ex, controllerName, "ConfirmPayment", model);

                if (ex.InnerException != null)
                {
                    TempData["CommandStatus"] = ex.InnerException.Message;
                }
                else
                {
                    TempData["CommandStatus"] = ex.Message;
                }
            }
            LogEnd(_logger, controllerName, "ConfirmPayment");
            return RedirectToAction("Detail", new { id = model.Id });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectReceipt(ConfirmPaymentModel model)
        {
            LogStart(_logger, controllerName, "RejectReceipt");
            var request = await CheckSecurity(model.Id, RoleLevelEnum.User, "RejectReceipt");
            if (request == null)
            {
                return RedirectToAction("Detail", new { id = model.Id });
            }

            try
            {
                var settlement = await _invoiceRepository.RejectReceipt(request.Id, model.InvoiceNo, model.ReceiptNo, TheUser.Id);
                await _repository.ApproveReceiptPrint(model.Id, settlement.Invoice.TotalPaid, settlement.Status, TheUser.Id);

                //
                var subject = $"Payment for Service Request {request.TrackingNo} Rejected.";
                var body = $"Dear, This is just to inform you that the your payment for service request " +
                    $"{request.TrackingNo} for Invoice {model.InvoiceNo} and for receipt {model.ReceiptNo} is rejected.";
                var recepients = new List<INotificationAddress> { request };

                await SaveNotification(request.ServiceType.ServiceOwner, subject, body,
                    recepients,
                    null, SendThroughFlag.Email | SendThroughFlag.SMS);
                //
            }
            catch (Exception ex)
            {
                LogError(_logger, ex, controllerName, "RejectReceipt", model);

                if (ex.InnerException != null)
                {
                    TempData["CommandStatus"] = ex.InnerException.Message;
                }
                else
                {
                    TempData["CommandStatus"] = ex.Message;
                }
            }
            LogEnd(_logger, controllerName, "RejectReceipt");
            return RedirectToAction("Detail", new { id = model.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmRequirementPayment(ConfirmPaymentModel model)
        {
            LogStart(_logger, controllerName, "ConfirmPayment");
            var request = await CheckSecurity(model.RequestId, RoleLevelEnum.User, "ConfirmPayment");
            if (request == null)
            {
                return RedirectToAction("Detail", new { id = model.RequestId });
            }
            var requrement = _repository.Response(request.RequestingUserId, model.Id);
            if (requrement == null && request.OpponentUserId != null)
            {
                requrement = _repository.Response(request.OpponentUserId.Value, model.Id);
            }

            try
            {
                var settlement = await _invoiceRepository.ConfirmPayment(model.Id, model.InvoiceNo, model.ReceiptNo, model.ConfirmedAmount, TheUser.Id);
                await _repository.ApproveRequirementReceiptPrint(model.RequestId, model.Id, settlement.Invoice.TotalPaid, settlement.Status, TheUser.Id);

                if (settlement.AmountPaid < model.ConfirmedAmount)
                {
                    CommandStatus("Confirmed Amount is greater than amount required.");
                }
                if (settlement.AmountPaid > model.ConfirmedAmount)
                {
                    CommandStatus("Insufficient Balance");
                }
                else
                {
                    CommandStatus(CommandStatusEnum.Approved);
                }
                if (requrement != null && requrement.Requirement != null)
                {
                    //
                    var subject = $"Payment for Service Request {request.TrackingNo} confirmed.";
                    var body = $"Dear, This is just to inform you that the your payment for service request " +
                        $"{request.TrackingNo} requirement for Invoice {model.InvoiceNo} and for receipt {model.ReceiptNo} is confirmed with amount {model.ConfirmedAmount}.";
                    var recepients = new List<INotificationAddress> { };
                    if (requrement.Requirement.IsForOpponent)
                    {
                        if (request.ServiceType.IsArbitration && request.OpponentUserId != null)
                        {
                            var opR = new ServiceRequest { Email = request.OpponentEmail, MobileNo = request.OpponentMobileNo, FullName = request.OpponentFullName };
                            recepients.Add(opR);
                        }
                    }
                    else
                    {
                        recepients.Add(request);
                    }
                    await SaveNotification(request.ServiceType.ServiceOwner, subject, body,
                        recepients,
                        null, SendThroughFlag.Email | SendThroughFlag.SMS);
                    //
                }
            }
            catch (Exception ex)
            {
                LogError(_logger, ex, controllerName, "ConfirmPayment", model);

                if (ex.InnerException != null)
                {
                    TempData["CommandStatus"] = ex.InnerException.Message;
                }
                else
                {
                    TempData["CommandStatus"] = ex.Message;
                }
            }
            LogEnd(_logger, controllerName, "ConfirmPayment");
            return RedirectToAction("Detail", new { id = model.RequestId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectRequirementReceipt(ConfirmPaymentModel model)
        {
            LogStart(_logger, controllerName, "ConfirmPayment");
            var request = await CheckSecurity(model.RequestId, RoleLevelEnum.User, "ConfirmPayment");
            if (request == null)
            {
                return RedirectToAction("Detail", new { id = model.RequestId });
            }

            var requrement = _repository.Response(request.RequestingUserId, model.Id);
            if (requrement == null && request.OpponentUserId != null)
            {
                requrement = _repository.Response(request.OpponentUserId.Value, model.Id);
            }

            try
            {
                var settlement = await _invoiceRepository.RejectReceipt(model.Id, model.InvoiceNo, model.ReceiptNo, TheUser.Id);
                await _repository.ApproveRequirementReceiptPrint(model.RequestId, model.Id, settlement.Invoice.TotalPaid, settlement.Status, TheUser.Id);

                if (requrement != null && requrement.Requirement != null)
                {
                    //
                    var subject = $"Payment for Service Request {request.TrackingNo} rejected.";
                    var body = $"Dear, This is just to inform you that the your payment for service request " +
                        $"{request.TrackingNo} requirement for Invoice {model.InvoiceNo} and for receipt {model.ReceiptNo} is rejected.";
                    var recepients = new List<INotificationAddress> { };
                    if (requrement.Requirement.IsForOpponent)
                    {
                        if (request.ServiceType.IsArbitration && request.OpponentUserId != null)
                        {
                            var opR = new ServiceRequest { Email = request.OpponentEmail, MobileNo = request.OpponentMobileNo, FullName = request.OpponentFullName };
                            recepients.Add(opR);
                        }
                    }
                    else
                    {
                        recepients.Add(request);
                    }
                    await SaveNotification(request.ServiceType.ServiceOwner, subject, body,
                        recepients,
                        null, SendThroughFlag.Email | SendThroughFlag.SMS);
                    //
                }
            }
            catch (Exception ex)
            {
                LogError(_logger, ex, controllerName, "ConfirmPayment", model);

                if (ex.InnerException != null)
                {
                    TempData["CommandStatus"] = ex.InnerException.Message;
                }
                else
                {
                    TempData["ConfirmPayment"] = ex.Message;
                }
            }
            LogEnd(_logger, controllerName, "RecommendApprove");
            return RedirectToAction("Detail", new { id = model.RequestId });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AcceptRejection(ChangeStatusModel model)
        {
            LogStart(_logger, controllerName, "AcceptRejection");
            var request = await CheckSecurity(model.Id, RoleLevelEnum.Admin, "AcceptRejection");
            if (request == null)
            {
                return RedirectToAction("Detail", new { id = model.Id });
            }


            try
            {
                await _repository.ChangeRequestStatus(model.Id, model.Status, model.Code, model.Extension, model.Remark, TheUser.Id);

                //
                if (model.Status == WorkflowStateEnum.Rejected)
                {
                    var subject = $"Service request {request.TrackingNo} Rejected";
                    var body = $"Dear, we regrate to inform you that your request {request.TrackingNo}. Reason for rejection  {model.Remark} you can contact ";
                    var recepients = new List<INotificationAddress> { request };
                    if (request.ServiceType.IsArbitration && request.OpponentUserId != null)
                    {
                        var opR = new ServiceRequest { Email = request.OpponentEmail, MobileNo = request.OpponentMobileNo, FullName = request.OpponentFullName };
                        recepients.Add(opR);
                    }
                    await SaveNotification(request.ServiceType.ServiceOwner, subject, body,
                        recepients,
                        null, SendThroughFlag.Email | SendThroughFlag.SMS);
                }
                //
                //CommandStatus(CommandStatusEnum.Saved);
            }
            catch (Exception ex)
            {
                LogError(_logger, ex, controllerName, "AcceptRejection", model);
                if (ex.InnerException != null)
                {
                    TempData["CommandStatus"] = ex.InnerException.Message;
                }
                else
                {
                    TempData["CommandStatus"] = ex.Message;
                }
            }
            _logger.LogInformation("ServiceRequest", "AcceptRejection", "End", DateTime.UtcNow);
            return RedirectToAction("Detail", new { id = model.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeclineRejection(ChangeStatusModel model)
        {
            LogStart(_logger, controllerName, "DeclineRejection");
            var request = await CheckSecurity(model.Id, RoleLevelEnum.Admin, "DeclineRejection");
            if (request == null)
            {
                return RedirectToAction("Detail", new { id = model.Id });
            }
            if (string.IsNullOrEmpty(model.Remark))
            {
                CommandStatus("Please provide reject reason");
                return RedirectToAction("Detail", new { id = model.Id });
            }
            try
            {
                await _repository.DeclineRejectionRequest(model.Id, TheUser.Id, model.Remark);
                CommandStatus(CommandStatusEnum.Rejected);
            }
            catch (Exception ex)
            {
                LogError(_logger, ex, controllerName, "DeclineRejection", model);
                if (ex.InnerException != null)
                {
                    TempData["CommandStatus"] = ex.InnerException.Message;
                }
                else
                {
                    TempData["CommandStatus"] = ex.Message;
                }
            }
            _logger.LogInformation("ServiceRequest", "DeclineRejection", "End", DateTime.UtcNow);
            return RedirectToAction("Detail", new { id = model.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeclineApprove(ChangeStatusModel model)
        {
            LogStart(_logger, controllerName, "DeclineApprove");
            var request = await CheckSecurity(model.Id, RoleLevelEnum.Admin, "DeclineApprove");
            if (request == null)
            {
                return RedirectToAction("Detail", new { id = model.Id });
            }
            var statusEnum = CommandStatusEnum.Rejected;
            try
            {
                await _repository.DeclineAcceptRequest(model.Id, TheUser.Id, model.Remark);

                CommandStatus(statusEnum);
            }
            catch (Exception ex)
            {
                LogError(_logger, ex, controllerName, "DeclineApprove", model);
                if (ex.InnerException != null)
                {
                    TempData["CommandStatus"] = ex.InnerException.Message;
                }
                else
                {
                    TempData["CommandStatus"] = ex.Message;
                }
            }
            _logger.LogInformation("ServiceRequest", "DeclineApprove", "End", DateTime.UtcNow);
            return RedirectToAction("Detail", new { id = model.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AcceptApprove(ChangeStatusModel model)
        {
            LogStart(_logger, controllerName, "AcceptApprove");
            var request = await CheckSecurity(model.Id, RoleLevelEnum.Admin, "AcceptApprove");
            if (request == null)
            {
                return RedirectToAction("Detail", new { id = model.Id });
            }

            try
            {
                await _repository.ApproveAcceptRequest(model.Id, TheUser.Id);
                //
                var subject = $"Service request {request.TrackingNo} Approved ";
                var body = $"Dear {request.FullName}, This is just to inform you that your request {request.TrackingNo} is approved";
                var recepients = new List<INotificationAddress> { request };
                if (request.ServiceType.IsArbitration && request.OpponentUserId != null)
                {
                    var opR = new ServiceRequest { Email = request.OpponentEmail, MobileNo = request.OpponentMobileNo, FullName = request.OpponentFullName };
                    recepients.Add(opR);
                }
                await SaveNotification(request.ServiceType.ServiceOwner, subject, body,
                        recepients,
                        null, SendThroughFlag.Email | SendThroughFlag.SMS);
                //
                CommandStatus(CommandStatusEnum.Approved);
            }
            catch (Exception ex)
            {
                LogError(_logger, ex, controllerName, "AcceptApprove", model);
                if (ex.InnerException != null)
                {
                    TempData["CommandStatus"] = ex.InnerException.Message;
                }
                else
                {
                    TempData["CommandStatus"] = ex.Message;
                }
            }
            _logger.LogInformation("ServiceRequest", "AcceptApprove", "End", DateTime.UtcNow);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveOpponent(Guid id, Guid opponentUserId)
        {
            LogStart(_logger, controllerName, "SaveOpponent");
            var request = await CheckSecurity(id, RoleLevelEnum.User, "ConfirmPayment");
            if (request == null)
            {
                return RedirectToAction("Detail", new { id = id });
            }

            try
            {
                await _repository.SetOpponentMember(id, opponentUserId, TheUser.Id);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (Exception ex)
            {
                LogError(_logger, ex, controllerName, "SaveOpponent", id);

                if (ex.InnerException != null)
                {
                    TempData["CommandStatus"] = ex.InnerException.Message;
                }
                else
                {
                    TempData["CommandStatus"] = ex.Message;
                }
            }
            LogEnd(_logger, controllerName, "CommandStatus");
            return RedirectToAction("Detail", new { id });
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
            var fullFileName = Path.Combine(AbsolutePath(request.ServiceType.ServiceOwner), fileName);
            if (OpenXmlWordEditor.LoadDocument(templateFile, replaceParameters, fullFileName))
            {
                FileStream stream = new FileStream(fullFileName, FileMode.Open);
                Response.Headers.Add("content-disposition", "inline;filename=" + $"{fileName}");
                return File(stream, "application/octet-stream", fileName);
            }
            return null;
        }
        [HttpGet]
        public async Task<IActionResult> PreviewCertificate(Guid serviceId)
        {

            try
            {
                var request = await CheckSecurity(serviceId, RoleLevelEnum.User, "PreviewCertificate");
                if (request == null || request.ServiceType == null)
                {
                    return RedirectToAction("Detail", new { id = serviceId });
                }
                var gd = Guid2String(serviceId);

                var fileName = $"Certificate_{gd}.pdf";
                FileStream stream = null;
                var certificateName = request.ServiceType.CertificateType.GetDisplayName();
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
                            //importerres.TextResponse = importerres.TextResponse.Count(s => s == '\n') > 4 ? importerres.TextResponse.Replace("\r\n", "; ") : importerres.TextResponse;
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
                    case CertificateTypeEnum.HealthCertificate:
                        stream = GetReportData<HealthCertificateReportModel, object, HealthCertificateReportFields>
                            (request, fileName, "HealthCertificate.frx", "HealthCertificateReportModel");
                        break;

                    case CertificateTypeEnum.SupportLetter:
                        //if (serviceId == new Guid("a4de7e53-2de6-4b01-8645-8b64fc936084"))
                        //{
                        return await GenerateSupportLetter(serviceId);
                        //}
                        //else
                        //{
                        //    stream = GetReportData<SupportLetterReportModel, object, SupportLetterReportFields>
                        //        (request, fileName, "SupportLetter.frx", "SupportLetter");
                        //}
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
                return BadRequest(ex.Message);
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
            var requester = _repository.User(request.RequestingUserId);
            model.CompanyName = requester.CompanyName;
            model.CompanyAddress = requester.Email + "\r\n" + requester.PhoneNumber + ", " + requester.MobileNo + "\r\n" + requester.Address;
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
                var orderedList = request.OptionalEntries;//.OrderBy(s => s.Label).ToList();
                for (int j = 0; j < orderedList.Count; j++)
                {
                    if (j % 2 == 0)
                    {
                        model.OptionalEntries.Add(new OptionalEntryForReport
                        {
                            Label = orderedList[j].Label,
                            Value = orderedList[j].Value,
                        });
                        i++;
                    }
                    else if (model.OptionalEntries.Count >= i)
                    {
                        model.OptionalEntries[i - 1].Label1 = orderedList[j].Label + ":";
                        model.OptionalEntries[i - 1].Value1 = orderedList[j].Value;
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
            //model.Watermark = "Back Office Copy";
            data.Add(model);
            return GetReportStream(templateName, data, dataName, fileName, AbsolutePath(request.ServiceType.ServiceOwner));
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
    }
}
