using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Membership;
using ECCSA.BackOffice.Server.Model.Payment;
using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Servicing;
using ECCSA.BackOffice.Server.Persistence.Repositories.Membership;
using ECCSA.BackOffice.Server.Persistence.Repositories.Payment;
using ECCSA.BackOffice.Server.Persistence.Repositories.Security;
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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Notification;

namespace ECCSA.BackOffice.Server.WebApp.Api.Contollers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //[AllowAnonymous]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ApiMembershipController : BaseController
    {
        private readonly ILogger<ApiMembershipController> _logger;
        private readonly IndividualRepository _individualRepository;
        private readonly AssociationRepository _associationRepository;
        private readonly StaffRepository _staffRepository;
        private readonly ApplicantRepository _applicantRepository;
        private readonly InvoiceRepository _invoiceRepository;
        private readonly LegalProfileRepository _legalProfileRepository;
        private readonly ExportAgreementRepository _exportAgreementRepository;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public ApiMembershipController(ILogger<ApiMembershipController> logger,
            UserManager<User> userManager,
            IndividualRepository individualRepository,
            AssociationRepository associationRepository,
            ApplicantRepository applicantRepository,
             InvoiceRepository invoiceRepository,
            LegalProfileRepository legalProfileRepository,
            ExportAgreementRepository exportAgreementRepository,
             IWebHostEnvironment hostingEnvironment, StaffRepository staffRepository,
            PublicUserRepository userRepository, NotificationRepository notificationRepository) : base(userManager, userRepository, notificationRepository)
        {
            _logger = logger;
            _individualRepository = individualRepository;
            _hostingEnvironment = hostingEnvironment;
            _associationRepository = associationRepository;
            _staffRepository = staffRepository;
            _applicantRepository = applicantRepository;
            _invoiceRepository = invoiceRepository;
            _legalProfileRepository = legalProfileRepository;
            _exportAgreementRepository = exportAgreementRepository;
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetECCSAAssociations()
        {
            try
            {
                var eccsaMembers = await _associationRepository.GetAssociationsForPublic(Persistence.Repositories.Constants.ECCSA);
                return Ok(eccsaMembers.Select(s => new
                {
                    Id = s.Id,
                    NameShort = s.NameShort,
                    NameEng = s.NameEng,
                    NameLocal = s.NameLocal,
                    ShortName = s.ShortName,
                    ShortName_Local = s.ShortName_Local
                }).OrderBy(s => s.NameEng));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetAssociation(Guid id)
        {
            try
            {
                var association = await _associationRepository.AssociationByIdForPublic(id, Guid.Empty);
                if (association == null)
                {
                    return NotFound();
                }
                if (association.ParentId != Guid.Empty)
                {
                    var parent = await _associationRepository.AssociationByIdForPublic(association.ParentId, Guid.Empty);
                    association.Parent = new Association
                    {
                        NameEng = parent.NameEng,
                        NameLocal = parent.NameLocal,
                        ShortName = parent.ShortName,
                        ShortName_Local = parent.ShortName_Local
                    };
                }
                else
                {
                    association.Parent = Association.NULL;
                }
                var employees = _staffRepository.Employees(id, Guid.Empty).ToList();
                var boardChairman = employees.FirstOrDefault(l =>
                l.PositionTypeId == Persistence.Repositories.Constants.MembershipBoardChairmanId &&
                l.Status == StatusEnum.Active &&
                l.EndDate >= DateTime.Today);

                var ceo = employees.FirstOrDefault(l =>
               l.PositionTypeId == Persistence.Repositories.Constants.MembershipCEOId &&
               l.Status == StatusEnum.Active &&
               l.EndDate >= DateTime.Today);
                if (boardChairman == null)
                {
                    boardChairman = new Staff()
                    {
                        Id = Guid.Empty,
                        NameEng = "",
                        NameLocal = "",
                        Mobile = "",
                        Email = "",
                        PositionType = new PositionType()
                        {
                            NameEng = "",
                            NameLocal = ""
                        }
                    };
                }
                if (ceo == null)
                {
                    ceo = new Staff()
                    {
                        Id = Guid.Empty,
                        NameEng = "",
                        NameLocal = "",
                        Mobile = "",
                        Email = "",
                        PositionType = new PositionType()
                        {
                            NameEng = "",
                            NameLocal = ""
                        }
                    };
                }
                var boardDirectorPath = ProcessImage(
                    boardChairman.Id,
                    boardChairman.Extension,
                    MembershipPhotoRelativePath,
                    MembershipPhotoAbsolutePath,
                   MembershipNoPersonUrl);
                var secretaryGeneralPath = ProcessImage(
                    ceo.Id,
                    ceo.Extension,
                    MembershipPhotoRelativePath,
                    MembershipPhotoAbsolutePath,
                    MembershipNoPersonUrl);

                var logoPath = ProcessImage(
                    association.Id,
                    association.Extension,
                    MembershipPhotoRelativePath,
                    MembershipPhotoAbsolutePath,
                   MembershipNoLogoUrl);
                return Ok(new
                {
                    association,
                    logo = logoPath,
                    boardChairman = new { boardChairman.Id, PositionName = boardChairman.PositionType?.Name ?? "", logo = boardDirectorPath },
                    ceo = new { ceo.Id, PositionName = ceo.PositionType?.Name ?? "", logo = secretaryGeneralPath }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetMembershipInfo()
        {
            try
            {
                var memberInfo = await _individualRepository.GetUserMembershipInfo(TheUser.Id);
                return Ok(memberInfo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("{type}")]
        public async Task<IActionResult> GetAssociationsForApply(AssociationTypeEnum type)
        {
            try
            {
                var list = await _associationRepository.GetAssociationsForApply(type, TheUser.Id);
                return Ok(list.Select(s => new
                {
                    Id = s.Id,
                    NameShort = s.NameShort,
                    NameEng = s.NameEng,
                    NameLocal = s.NameLocal,
                    ShortName = s.ShortName,
                    ShortName_Local = s.ShortName_Local
                }).OrderBy(s => s.NameEng));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetApplications(AssociationTypeEnum type)
        {
            try
            {
                var list = await _applicantRepository.UserApplicants(TheUser.Id);
                return Ok(list.OrderBy(s => s.ApplicationDate));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetAssociationForApply(Guid id)
        {
            try
            {
                var association = await _associationRepository.AssociationByIdForPublicApply(id, Guid.Empty);
                return Ok(association);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Save(IFormCollection formCollection)
        {
            var message = string.Empty;
            var isValid = true;
            Guid id = Guid.Parse(formCollection["Id"]);
            Guid associationId = Guid.Parse(formCollection["associationId"]);
            var applicant = new Applicant()
            {
                Id = id,
                AssociationId = associationId,
                AssociatedUserId = TheUser.Id,
                IndividualId = TheUser.AssociationId,
                Responses = new List<ApplicantDocument>(),
                NameEng = TheUser.CompanyName,
                NameLocal = TheUser.CompanyName,
                ApplicationType = ApplicationTypeEnum.New,
                Remark = " "
                //Attachments = new List<RequestAttachment>()
            };

            var association = await _associationRepository.AssociationByIdForPublicApply(associationId, TheUser.Id);
            var memberInfo = await _individualRepository.GetEnrollmentById(associationId, TheUser.AssociationId);
            if (memberInfo != null)
            {
                applicant.ApplicationType = ApplicationTypeEnum.Renewal;
                applicant.Signature = memberInfo.Code;
            }

            var userApplications = await _applicantRepository.UserApplicants(TheUser.Id);
            if (userApplications.
                Any(p => p.AssociatedUserId == TheUser.Id && p.AssociationId == associationId &&
                p.ApplicationStatus != WorkflowStateEnum.Rejected && p.Id != applicant.Id))
            {
                return Ok(new
                {
                    saved = false,
                    erorMsg = "Application for Association/camber already exits!!"
                });
            }
            var serviceOwner = ECCSAModuleFlag.Membership;
            var legalProfiles = _legalProfileRepository.UserLegalProfileLookup(TheUser.Id, 1);
            var agreements = _exportAgreementRepository.UserExportAgreementLookup(TheUser.Id, 1);



            List<ApplicantDocument> responses = null;
            if (applicant.Id != Guid.Empty)
            {
                responses = await _applicantRepository.ApplicantDocuments(id, TheUser.Id);
            }
            else
            {
                //await PopulateAttachment(formCollection, association, applicant);
                //
            }
            await PopulateResponses(formCollection, association, serviceOwner, legalProfiles, agreements, applicant, responses);
            if (isValid)
            {
                if (applicant.Id == Guid.Empty)
                {
                    await _applicantRepository.Save(applicant, TheUser.Id);
                }
                else
                {
                    await _applicantRepository.UpdateResponse(applicant, TheUser.Id);
                }
                return Ok(new
                {
                    saved = true,
                    erorMsg = string.Empty
                });
            }
            return Ok(new
            {
                saved = false,
                erorMsg = message
            });
        }
        private async Task PopulateResponses(IFormCollection formCollection, Association association,
           ECCSAModuleFlag serviceOwner, List<LookupValue> legalProfiles,
           List<LookupValue> agreements, Applicant request,
           List<ApplicantDocument> responses)
        {
            foreach (var req in association.MembershipRequirements.Where(
                r => (request.ApplicationType == ApplicationTypeEnum.New && 
                r.ApplicationRequirementLevel != RequirementLevelEnum.NotRequired) ||
                (request.ApplicationType == ApplicationTypeEnum.Renewal &&
                r.RenewalRequirementLevel != RequirementLevelEnum.NotRequired)))
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
                var resp = new ApplicantDocument
                {
                    Id = reqResponseId,
                    ApplicantId = Guid.Empty,
                    RequirementId = req.Id,
                };
                switch (req.InputMethod)
                {
                    case ServiceInputTypeEnum.Upload:
                        var file = formCollection.Files.FirstOrDefault(f => f.Name == key);
                        if (file != null)
                        {
                            var fileName = file.FileName;
                            await SaveDocument(resp.Id, serviceOwner, file);
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
                    case ServiceInputTypeEnum.SimpleLookup:
                        resp.LookupValueId = Guid.Parse(val);
                        resp.TextResponse = association.RequirementLookups
                            .FirstOrDefault(l => l.Id == req.LookupTypeId)?.
                            LookupValues.FirstOrDefault(v => v.Id == resp.LookupValueId)?.Name;
                        break;
                    case ServiceInputTypeEnum.TradeAgreement:
                        resp.LookupValueId = Guid.Parse(val);
                        resp.TextResponse = agreements.FirstOrDefault(v => v.Id == resp.LookupValueId)?.Name;
                        break;
                    case ServiceInputTypeEnum.LegalDocument:
                        resp.LookupValueId = Guid.Parse(val);
                        resp.TextResponse = legalProfiles.FirstOrDefault(v => v.Id == resp.LookupValueId)?.Name;
                        break;
                    default:
                        resp.TextResponse = val;
                        break;
                }

                request.Responses.Add(resp);
            }
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetApplication(Guid id)
        {
            //HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);

            var serviceRequest = await _applicantRepository.UserApplication(TheUser.Id, id);

            return Ok(serviceRequest);
        }

        [HttpPost]
        [Route("{id}")]
        public async Task<IActionResult> SubmitApplication(Guid id)
        {
            await _applicantRepository.SubmitUserApplication(id, TheUser.Id);
            return Ok(new
            {
                submitted = true
            });
        }
        [HttpPost]
        [Route("{id}")]
        public async Task<IActionResult> SubmitApplicationDocument(Guid id)
        {
            await _applicantRepository.SubmitUserApplicationDocument(id, TheUser.Id);
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
        public async Task<IActionResult> SaveApplicationPayment(IFormCollection formCollection)
        {
            Guid applicationId = Guid.Parse(formCollection["applicationId"]);
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
            PaymentMethodEnum paymentMethod = PaymentMethodEnum.Manual; //(PaymentMethodEnum)int.Parse(formCollection["paymentMethod"]);
            DateTime paymentDate = DateTime.Now;
            string remark = string.Empty;
            try
            {
                var application = await _applicantRepository.UserApplication(TheUser.Id, applicationId);
                if (application == null)
                    return BadRequest("Invalid Request");
                var invoice = await _invoiceRepository.InvoiceByNo(application.InvoiceNo, application.Id);
                if (invoice.InvoiceDate > paymentDate || paymentDate > DateTime.Now)
                {
                    CommandStatus(CommandStatusEnum.ValidationError);

                    return BadRequest("Payment Date out of range");
                }
                var settlement = await _invoiceRepository.
                    MakeUserPayment(applicationId, invoiceNo, amount, paymentDate, receiptNo, paymentMethod, remark, receiptExtention, TheUser.Id);
                if (file != null && settlement != null)
                {
                    var fileName = file.FileName;
                    await SaveDocument(settlement.Id, ECCSAModuleFlag.Membership, file, "receipts");
                }
                if (settlement != null)
                {
                    await _applicantRepository.RecordUserPayment(applicationId, invoiceNo, settlement.ReceiptNo, paymentMethod, remark, TheUser.Id);
                }
                return Ok("Payment Processed");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public async Task<IActionResult> PrintApplicationInvoice(Guid referenceId, string invoiceNo)
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
                var application = await _applicantRepository.UserApplication(TheUser.Id, invoice.ReferenceId);
                if (application == null)
                {

                }
                else
                {
                    pi.PaymentReason = "Membership Application for " + application.AssociationName;
                }
                var data = new List<PrintInvoice>
                {
                    pi
                };
                stream = GetReportStream<PrintInvoice>("Invoice.frx", data, "PrintInvoice", fileName);
                return File(stream, "application/octet-stream", fileName);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        public async Task<IActionResult> PrintApplicationReceipt(Guid referenceId, string invoiceNo, int no = 0)
        {
            Invoice invoice = await _invoiceRepository.InvoiceByNo(invoiceNo, referenceId);
            var pi = new PrintInvoice(invoice);

            if (invoice.Settlments == null || invoice.Settlments.Count < no + 1)
            {
                return BadRequest("Receipt Not found");
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

                var application = await _applicantRepository.UserApplication(TheUser.Id, invoice.ReferenceId);
                if (application == null)
                {

                }
                else
                {
                    pi.PaymentReason = "Membership Application for " + application.AssociationName;
                }
                var data = new List<PrintInvoice>
                {
                    pi
                };
                stream = GetReportStream<PrintInvoice>("Receipt.frx", data, "PrintInvoice", fileName);

                return File(stream, "application/pdf");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        private FileStream GetCurrentFile(string fileName)
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
            var pdfExport = new PDFSimpleExport();
            pdfExport.Export(report, tmpFileName);
            report.Dispose();

            Response.Headers.Add("content-disposition", "inline;filename=" + fileName);
            return new FileStream(tmpFileName, FileMode.Open);
        }


    }
}
