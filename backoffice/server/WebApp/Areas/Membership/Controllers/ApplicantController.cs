
using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Membership;
using ECCSA.BackOffice.Server.Model.Payment;
using ECCSA.BackOffice.Server.Model.Security;
using FastReport.Export.PdfSimple;
using ECCSA.BackOffice.Server.Persistence.Repositories.Membership;
using ECCSA.BackOffice.Server.Persistence.Repositories.Payment;
using ECCSA.BackOffice.Server.Persistence.Repositories.Security;
using ECCSA.BackOffice.Server.WebApp.Areas.Membership.Model;
using ECCSA.BackOffice.Server.WebApp.Controllers;
using ECCSA.BackOffice.Server.WebApp.Models;
using MailService.WebApi.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ECCSA.BackOffice.Server.WebApp.Areas.Service.Model;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Notification;
using ECCSA.BackOffice.Server.Model.Common.Notification;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Wordprocessing;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Membership.Controllers
{
    [Area("Membership")]
    public class ApplicantController : BaseController
    {
        private readonly ILogger<ApplicantController> _logger;
        readonly AssociationRepository _associationRepository;
        private readonly InvoiceRepository _invoiceRepository;
        private readonly LegalDocumentTypeRepository _legalDocumentTypeRepository;
        private readonly LocationRepository _locationRepository;
        private readonly ApplicantRepository _repository;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ApplicantController(ILogger<ApplicantController> logger,
            UserManager<User> userManager,
            AssociationRepository associationRepository,
            LegalDocumentTypeRepository legalDocumentTypeRepository,
            LocationRepository locationRepository,
            InvoiceRepository invoiceRepository,
            ApplicantRepository applicantRepository,
             IWebHostEnvironment hostingEnvironment,
            PublicUserRepository userRepository,
            NotificationRepository notificationRepository
            ) : base(userManager, userRepository, notificationRepository)
        {

            _logger = logger;
            _associationRepository = associationRepository;
            _locationRepository = locationRepository;
            _legalDocumentTypeRepository = legalDocumentTypeRepository;
            _invoiceRepository = invoiceRepository;
            _repository = applicantRepository;
            _hostingEnvironment = hostingEnvironment;
            controllerName = "Applicant";
        }

        [HttpGet]
        public async Task<JsonResult> GetTree()
        {
            var associations = await _associationRepository.List(TheUser.Id);
            var tview = TreeView.BuildTree(associations, TheUser.AssociationId);
            return Json(tview);
        }

        [HttpGet]
        public JsonResult GetLocation(Guid? parentId)
        {
            var locations = _locationRepository.Locations(TheUser.Id).ToList();
            Guid pId;
            if (parentId.HasValue && parentId.Value != Guid.Empty)
            {
                pId = parentId.Value;
            }
            else
            {
                pId = Constants.EthiopiaId;
            }
            var tmp = locations.Where(l => l.ParentId == pId
                  || l.Id == pId).ToList();

            tmp.Insert(0, new Location
            {
                Id = Guid.Empty,
                NameEng = string.Empty,
                NameLocal = string.Empty,
                Code = string.Empty
            });
            var lst = new List<LookupView>();
            foreach (var t in tmp)
            {
                lst.Add(new LookupView
                {
                    Id = t.Id,
                    Name = t.Name,
                    Code = t.Code,
                    ParentId = t.ParentId
                });
            }
            return Json(lst);
        }

        public async Task<IActionResult> Index(Guid? associationId)
        {
            var userName = TheUser.FullName;
            var currentAssociationId = associationId.HasValue && associationId.Value != Guid.Empty ? associationId.Value : TheUser.AssociationId;
            //var associations = await _associationRepository.List(TheUser.Id);
            //var tview = AssociationTreeView.BuildTree(associations, TheUser.AssociationId);
            //var json = tview.ToString();
            LogStart(_logger, controllerName, "Index");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Membership);
            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Index");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            var model = new ApplicantViewModel
            {
                FullName = userName,
                CurrentPage = "Applicants",
                CurrentOperation = "List",
                TheList = await _repository.List(associationId ?? TheUser.AssociationId, TheUser.Id),
                Associations = await _associationRepository.ChildList(currentAssociationId, TheUser.Id),
                AssociationId = currentAssociationId

            };
            model.Associations.Insert(0, new Association
            {
                Id = Guid.Empty,
                NameEng = "",
                NameLocal = "",
                ShortName = "",
                ShortName_Local = "",
            });
            model.TheModel = new Applicant()
            {
                AssociationId = currentAssociationId,

            };
            model.ChangeActive(MenuItemEnum.Applicant);
            model.RoleLevel = roleLevel;
            LogEnd(_logger, controllerName, "Index");
            TempData["title"] = "Applicants";
            return View(model);
        }

        public async Task<IActionResult> Detail(Guid id)
        {
            var userName = TheUser.FullName;
            LogStart(_logger, controllerName, "Detail");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Membership);
            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Detail");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            var model = new ApplicantViewModel
            {
                FullName = userName,
                CurrentPage = "Applicant",
                CurrentOperation = "Detail",

                TheModel = await _repository.ApplicantById(id, TheUser.Id),

            };
            if (model.TheModel == null)
            {
                return RedirectToAction("Error", "Home", new { area = "" });
            }
            model.DocumentTypes = _legalDocumentTypeRepository.LegalDocumentTypes(Guid.Empty).ToList();
            model.Locations = _locationRepository.Locations(Guid.Empty).ToList();

            if (string.IsNullOrEmpty(model.TheModel.InvoiceNo))
            {
                model.Invoice = new Server.Model.Payment.Invoice();
                model.Payment = new Server.Model.Payment.Settlment();
            }
            else
            {
                model.Invoice = await _invoiceRepository.InvoiceByNo(model.TheModel.InvoiceNo, model.TheModel.Id);
                if (model.Invoice == null)
                {
                    model.Invoice = new Server.Model.Payment.Invoice();
                    model.Payment = new Server.Model.Payment.Settlment();
                }
                else
                {
                    if (model.Invoice.DueRemaining >= 1)
                    {
                        model.Payment = new Server.Model.Payment.Settlment
                        {
                            Id = Guid.Empty,
                            InvoiceId = model.Invoice.Id,

                        };
                    }
                    else
                    {
                        model.Payment = model.Invoice.Settlments.ToList()[model.Invoice.Settlments.Count - 1];
                    }
                }
                if (model.TheModel.Invoice?.Settlments.Count > 0)
                {
                    foreach (var ss in model.TheModel.Invoice.Settlments)
                    {
                        var fileName = Guid2String(ss.Id);
                        ss.RelativePath = Path.Combine(RelativePath(ECCSAModuleFlag.Membership), "receipts", $"{fileName}{ss.Extension}");
                    }
                }
            }
            if (model.RoleLevel == RoleLevelEnum.Admin)
            {
                model.Users = await _repository.Users();
            }
            if (model.TheModel.UnderReview)
            {
                model.IsCurrentOperator = !string.IsNullOrEmpty(model.TheModel.OperatorName)
                    && model.TheModel.OperatorName.ToLowerInvariant() == TheUser.FullName.ToLowerInvariant();
            }

            foreach (var res in model.TheModel.Responses)
            {
                if (res.Requirement.InputMethod == ServiceInputTypeEnum.Upload)
                {
                    var fileName = Guid2String(res.Id);
                    var filePath = AbsolutePath(ECCSAModuleFlag.Membership);
                    var extension = GetExtension(filePath, fileName);
                    res.RelativePath = Path.Combine(RelativePath(ECCSAModuleFlag.Membership), $"{fileName}{extension}");
                }
                else if (res.Requirement.InputMethod == ServiceInputTypeEnum.LegalDocument)
                {
                    var userPath = Guid2String(model.TheModel.AssociatedUserId);
                    var filePath = Path.Combine(UserLegalProfileDocumentsAbsolutePath, userPath);
                    var fileName = Guid2String(res.LookupValueId.Value);
                    var extension = GetExtension(filePath, fileName);
                    res.RelativePath = Path.Combine(UserLegalProfileDocumentsRelativePath, userPath, $"{fileName}{extension}");
                }
            }
            model.ChangeActive(MenuItemEnum.Applicant);
            model.RoleLevel = roleLevel;
            LogEnd(_logger, controllerName, "Detail");
            return View(model);
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
                var request = await _repository.ApplicantById(invoice.ReferenceId, TheUser.Id);
                pi.PaymentReason = request.Reason;
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

                var request = await _repository.ApplicantById(invoice.ReferenceId, TheUser.Id);
                pi.PaymentReason = request.Reason;
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
                await _repository.StartProcessing(id, TheUser.Id);
                CommandStatus(CommandStatusEnum.Saved);
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
                CommandStatus(CommandStatusEnum.Saved);
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
                var requirement = request.ServiceType.MembershipRequirements.FirstOrDefault(x => x.Id == response.RequirementId);
                if (requirement == null)
                {
                    CommandStatus("Requirement Not found");
                    return RedirectToAction("Detail", new { id = model.ResponseId });
                }

                await _repository.ReviewRequirement(model.Id, model.ResponseId, model.Status, model.Remark, TheUser.Id);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (Exception ex)
            {
                LogError(_logger, ex, controllerName, "ReviewResponse", model);
                TempData["CommandStatus"] = ex.Message;
            }

            return RedirectToAction("Detail", new { id = model.ResponseId });

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatus(ChangeStatusModel model)
        {
            _logger.LogInformation("ServiceRequest", "ChangeStatus", "Started", DateTime.UtcNow);
            var request = await _repository.ApplicantById(model.Id, TheUser.Id);
            // var serviceOwner = request.ServiceType.ServiceOwner;
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
                await SaveDocument(model.Id, ECCSAModuleFlag.Membership, model.File);
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
                    await SaveDocument(model.Id, ECCSAModuleFlag.Membership, model.File);
                    var fileName = model.File.FileName;
                    model.Extension = Path.GetExtension(Path.GetFileName(fileName));
                }

            }
            try
            {
                await _repository.ChangeRequestStatus(model.Id, model.Status, model.Code, model.Extension, model.Remark, TheUser.Id);
                if (model.Status == WorkflowStateEnum.Accepted || model.Status == WorkflowStateEnum.Rejected)
                {
                    //
                    var subject = $"Service Status Changed to {model.Status}";
                    var body = string.IsNullOrEmpty(model.Remark) ? "Dear, This is just to inform you that the status of your request has changed" :
                                $"Dear, The following reason is provided. {model.Remark}";
                    await SaveNotification(ECCSAModuleFlag.Membership, subject, body,
                        new List<INotificationAddress> { request },
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

        private async Task<Applicant> CheckSecurity(Guid requestId, RoleLevelEnum minLevel, string operationName)
        {
            var request = await _repository.ApplicantById(requestId, TheUser.Id);

            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Membership);
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
                var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Membership);

                if (roleLevel == RoleLevelEnum.Admin)
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

            model.Extension = string.Empty;
            MembershipEnrollment enrollment = null;
            if (request.PendingDocumentsNo > 0)
            {
                TempData["CommandStatus"] = $"There are {request.PendingDocumentsNo} pending documents";
                return RedirectToAction("Detail", new { id = model.Id });
            }

            else if ((model.File != null && model.File.Length > 0))
            {
                await SaveDocument(model.Id, ECCSAModuleFlag.Membership, model.File);
                var fileName = model.File.FileName;
                model.Extension = Path.GetExtension(Path.GetFileName(fileName));
                model.Code = await GetHashCode(model.File);

            }


            try
            {
                await _repository.RecommendApprove(model.Id, model.Extension, model.Code, TheUser.Id);
                var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Membership);

                if (roleLevel == RoleLevelEnum.Admin)
                {
                    enrollment = await _repository.ApproveAcceptRequest(model.Id, TheUser.Id);
                }
                
                    //
                    var subject = $"Welcome to becoming a member {request.TrackingNo} is approved.";
                    var body = $"Dear, This is just to inform you that the your request {request.TrackingNo} for membership is approved.";
                await SaveNotification(ECCSAModuleFlag.Membership, subject, body,
                    new List<INotificationAddress> { request },
                    model.File, SendThroughFlag.Email | SendThroughFlag.SMS);
                //

                CommandStatus(CommandStatusEnum.Approved);
                LogEnd(_logger, controllerName, "RecommendApprove");

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
            if (enrollment == null)
                return RedirectToAction("Detail", new { id = model.Id });
            return RedirectToAction("Detail", "Individual", new { id = enrollment.Id });
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
                var subject = $"Service request {request.TrackingNo} Rejected";
                var body = $"Dear, we regrate to inform you that your request {request.TrackingNo}. Reason for rejection  {model.Remark} you can contact ";
                await SaveNotification(ECCSAModuleFlag.Membership, subject, body,
                        new List<INotificationAddress> { request },
                        null, SendThroughFlag.Email | SendThroughFlag.SMS);
                //

                CommandStatus(CommandStatusEnum.Saved);
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
                await SaveNotification(ECCSAModuleFlag.Membership, subject, body,
                        new List<INotificationAddress> { request },
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


    }
}
