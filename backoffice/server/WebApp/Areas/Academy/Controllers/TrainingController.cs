using DocumentFormat.OpenXml.Office2010.Excel;
using ECCSA.BackOffice.Server.Model.Academy;
using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Common.Notification;
using ECCSA.BackOffice.Server.Model.Payment;
using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Persistence.DTO;
using ECCSA.BackOffice.Server.Persistence.DTO.Academy;
using ECCSA.BackOffice.Server.Persistence.Repositories.Academy;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Notification;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Servicing;
using ECCSA.BackOffice.Server.Persistence.Repositories.Payment;
using ECCSA.BackOffice.Server.Persistence.Repositories.Security;
using ECCSA.BackOffice.Server.WebApp.Areas.Academy.Model;
using ECCSA.BackOffice.Server.WebApp.Controllers;
using ECCSA.BackOffice.Server.WebApp.Models;
using FastReport.Export.PdfSimple;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Academy.Controllers
{
    [Area("Academy")]
    public class TrainingController : BaseController
    {
        private readonly ILogger<TrainingController> _logger;
        private readonly string SponsorAttachmentsFolder = Path.Combine(
                                        Directory.GetCurrentDirectory(),
                                        "wwwroot", "Academy",
                                        "SponsorAttachments");
        private readonly string CertificateAttachmentsFolder = Path.Combine(
                                        Directory.GetCurrentDirectory(),
                                        "wwwroot", "Academy",
                                        "TrainingCertificate");

        private readonly TrainingRepository _trainingRepository;
        private readonly TopicRepository _topicRepository;
        private readonly TrainingSponsorRepository _trainingSponsorRepository;
        private readonly SponsorRepository _sponsorRepository;
        private readonly RegistrationRepository _registrationRepository;
        private readonly AssessmentRepository _assessmentRepository;
        private readonly ResourceDeliveryRepository _resourceDeliveryRepository;
        private readonly LearningResourceRepository _learningResourceRepository;
        private readonly AttendanceRepository _attendanceRepository;
        private readonly InvoiceRepository _invoiceRepository;
        private readonly ServiceTypeRepository _settingRepository;
        private readonly ServiceRequestRepository _requestRepository;
        private readonly SurveyRepository _surveyRepository;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public TrainingController(ILogger<TrainingController> logger,
            UserManager<User> userManager,
            SurveyRepository surveyRepository,
            TrainingRepository trainingRepository,
            TopicRepository topicRepository,
            TrainingSponsorRepository trainingSponsorRepository,
        RegistrationRepository registrationRepository,
        AssessmentRepository assessmentRepository,
        ResourceDeliveryRepository resourceDeliveryRepository,
        LearningResourceRepository learningResourceRepository,
        AttendanceRepository attendanceRepository,
            InvoiceRepository invoiceRepository,
        // IndividualRepository individualRepository,
        SponsorRepository sponsorRepository,
        PublicUserRepository userRepository,
            ServiceTypeRepository settingRepository,
            ServiceRequestRepository requestRepository,
            NotificationRepository notificationRepository,
        IWebHostEnvironment hostingEnvironment
            ) : base(userManager, userRepository, notificationRepository)
        {
            _logger = logger;
            _trainingRepository = trainingRepository;
            _topicRepository = topicRepository;
            _trainingSponsorRepository = trainingSponsorRepository;
            _sponsorRepository = sponsorRepository;
            _registrationRepository = registrationRepository;
            _assessmentRepository = assessmentRepository;
            _resourceDeliveryRepository = resourceDeliveryRepository;
            _learningResourceRepository = learningResourceRepository;
            _attendanceRepository = attendanceRepository;
            _invoiceRepository = invoiceRepository;
            _settingRepository = settingRepository;
            _requestRepository = requestRepository;
            _surveyRepository = surveyRepository;
            _hostingEnvironment = hostingEnvironment;
            controllerName = "Training";
        }
        [HttpGet]
        public async Task<IActionResult> Index(TrainingStatus status = TrainingStatus.New)
        {
            var userName = TheUser.FullName;
            LogStart(_logger, controllerName, "Index");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Academy);
            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Index");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            var model = new TrainingViewModel
            {
                FullName = userName,
                CurrentPage = "Training",
                CurrentOperation = "List",
                TheList = _trainingRepository.Trainings(TheUser.Id, status).ToList(),

            };
            model.ChangeActive(MenuItemEnum.AcademyTraining);
            model.RoleLevel = roleLevel;
            LogEnd(_logger, controllerName, "Index");
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Detail(Guid id, Guid? serviceTypeId = null, int activeTabIndex = 0)
        {
            var userName = TheUser.FullName;
            LogStart(_logger, controllerName, "Detail");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Academy);
            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Detail");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            var training = id != Guid.Empty || serviceTypeId == null ?
                await _trainingRepository.TrainingById(id, TheUser.Id) :
                await _trainingRepository.TrainingByServiceTypeId(serviceTypeId.Value, TheUser.Id);
            if(training == null)
            {
                return RedirectToAction("Index");
            }
            var model = new TrainingViewModel
            {
                FullName = userName,
                CurrentPage = "Training",
                CurrentOperation = "Detail",
                TheList = new List<Training>(),
                TheModel = training,
                UserId = TheUser.Id,
                Requests = training.ServiceTypeId.HasValue ?
                await _requestRepository.ServicesByServiceType(ECCSAModuleFlag.Academy, training.ServiceTypeId.Value) :
                new List<Server.Model.Common.Servicing.ServiceRequest>()
            };
            if (model.TheModel == null)
            {
                return RedirectToAction("Error", "Home", new { area = "" });
            }
            if (model.TheModel.Status >= TrainingStatus.Completed)
            {
                model.SurveySettings = await _trainingRepository.GetPrivateSurveySettings();
            }
            else
            {
                model.SurveySettings = new List<LookupValue>();
            }
            model.ChangeActive(MenuItemEnum.AcademyTraining);
            model.RoleLevel = roleLevel;
            model.ActiveTabIndex = activeTabIndex;
            LogEnd(_logger, controllerName, "Detail");
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id, Guid topicId)
        {
            var userName = TheUser.FullName;
            var model = new TrainingViewModel
            {
                FullName = userName,
                CurrentPage = "Trainings",
                TheList = new List<Training>(),
            };
            model.ChangeActive(MenuItemEnum.AcademyTraining);
            try
            {
                model.TheModel = (await _trainingRepository.TrainingById(id, TheUser.Id)) ?? new Training
                {
                    TopicId = topicId,
                    Topic = await _topicRepository.TopicById(topicId, TheUser.Id),
                    StartDate = DateTime.Today,
                    EndDate = DateTime.Today,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unknown Error");
                TempData["CommandStatus"] = ex.Message;
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Save(TrainingSurvey model)
        {
            try
            {
                var training = await _trainingRepository.TrainingById(model.TrainingId, TheUser.Id);
                var survey = new Survey()
                {
                    Id = training.Id,
                    SettingId = model.SettingId,
                    StartDate = DateTime.Today.Date,
                    EndDate = model.EndDate,
                    Title = $"{training.Topic.Code} {training.Topic.NameEng} {training.StartDate.ToString("yyyyMMdd")}",
                    Description = training.Topic.Decription,
                    AnalysisDescription = String.Empty,


                };
                foreach (var student in training.Registrations)
                {
                    if (student.RegistrationStatus != RegistrationStatus.Accepted) continue;
                    survey.Audiences.Add(new SurveyAudience
                    {
                        Id = Guid.NewGuid(),
                        SurveyId = survey.Id,
                        Email = student.EmailAddress,
                        MobileNo = student.MobileNo,
                        UserId = student.LoginId
                    });
                }
                training.SurveyId = survey.Id;
                await _surveyRepository.Save(survey, TheUser.Id);
                await _trainingRepository.Save(training, TheUser.Id);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (Exception ex)
            {
                LogError(_logger, ex, controllerName, "Save", model);
                CommandStatus(CommandStatusEnum.UnkownError);
            }
            return RedirectToAction("Detail", new { id = model.TrainingId });
        }

        [HttpPost]
        public async Task<IActionResult> CreateTraining(CreateTrainingInput theModel)
        {
            LogStart(_logger, controllerName, "CreateTraining");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Academy);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "CreateTraining");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("Detail", "Topic", new { Id = theModel.TopicId });
            }
            try
            {
                var topic = await _topicRepository.TopicById(theModel.TopicId, TheUser.Id);
                var settingValues = string.Empty;
                foreach (var setting in topic.TopicSettings)
                {
                    settingValues += $"{setting.SettingVariable.NameEng}: {setting.SettingValue}. \r\n";
                }
                var sessions = string.Empty;
                if (theModel.HasMorning)
                    sessions = "Morning ";
                if (theModel.HasAfternoon)
                    sessions = "Afternoon ";
                if (theModel.HasEvening)
                    sessions = "Evening ";

                if (!string.IsNullOrEmpty(sessions))
                    sessions = $"Session(s): {sessions} ";
                var desc = topic.Decription == null ||
                    topic.Decription.Trim() == string.Empty ? topic.NameEng : topic.Decription;
                var serviceType = await _settingRepository.Save(new Server.Model.Common.Servicing.ServiceType
                {
                    NameEng = $"{topic.NameEng} Training",
                    DescriptionEng = $"Training on {topic.NameEng} from {theModel.StartDate.ToString("yyyy-MM-dd")} to {theModel.EndDate.ToString("yyyy-MM-dd")}. \r\n" +
                    $"Category: {topic.Category.NameEng}. \r\n" +
                     $"Venue: {theModel.Schedule}, {sessions} \r\nInstructor: {theModel.Instructor}, \r\nInstruction Media: {theModel.InstructionMedia.GetDisplayName()}. \r\n" +
                     $"Topic description: {desc}. \r\n" +
                     $"{settingValues}",
                    RateBasis = theModel.RateBasis,
                    MemberServiceCharge = theModel.MemberFee,
                    NonMemberServiceCharge = theModel.NonMemberFee,
                    StartDate = theModel.EndDate,
                    EndDate = theModel.EndDate,
                    ServiceOwner = ECCSAModuleFlag.Academy,
                    ApprovalRequired = false,
                    Status = StatusEnum.Inactive,
                    ApprovalAttachmentRequired = false,
                }, TheUser.Id);

                var payload = await _trainingRepository.Create(new GenericInput<CreateTrainingInput>(theModel, TheUser.Id), serviceType.Id);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (Exception ex)
            {
                TempData["CommandStatus"] = ex.Message;
            }
            return RedirectToAction("Detail", "Topic", new { Id = theModel.TopicId, activeTabIndex = 2 });
        }
        [HttpPost]
        public async Task<IActionResult> UpdateTraining(UpdateTrainingInput theModel)
        {
            LogStart(_logger, controllerName, "UpdateTraining");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Academy);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "UpdateTraining");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("Detail", new { id = theModel.Id });
            }
            try
            {
                //
                var needNotification = false;
                var training = await _trainingRepository.TrainingByIdForCompare(theModel.Id, TheUser.Id);
                needNotification = training != null && training.Registrations?.Count > 0 &&
                    (training.StartDate != theModel.StartDate ||
                     training.EndDate != theModel.EndDate ||
                     training.Schedule != theModel.Schedule ||
                     training.Instructor != theModel.Instructor ||
                     training.InstructionMedia != theModel.InstructionMedia ||
                     training.HasAfternoon != theModel.HasAfternoon ||
                     training.HasMorning != theModel.HasMorning ||
                     training.HasEvening != theModel.HasEvening);
                //
                var topic = await _topicRepository.TopicById(theModel.TopicId, TheUser.Id);
                var settingValues = string.Empty;
                foreach (var setting in topic.TopicSettings)
                {
                    settingValues += $"{setting.SettingVariable.NameEng}: {setting.SettingValue}. \r\n";
                }
                var desc = topic.Decription == null ||
                    topic.Decription.Trim() == string.Empty ? topic.NameEng : topic.Decription;
                var serviceType = await _settingRepository.Save(new Server.Model.Common.Servicing.ServiceType
                {
                    Id = training.ServiceTypeId ?? Guid.Empty,
                    NameEng = $"{topic.NameEng} Training",
                    DescriptionEng = $"Training on {topic.NameEng} from {theModel.StartDate:yyyy-MM-dd} to {theModel.EndDate:yyyy-MM-dd}. \r\n" +
                    $"Category: {topic.Category.NameEng}. \r\n" +
                     $"Schedule: {theModel.Schedule}, \r\nInstructor: {theModel.Instructor}, \r\nInstruction Media: {theModel.InstructionMedia.GetDisplayName()}. \r\n" +
                     $"Topic description: {desc}. \r\n" +
                     $"{settingValues}",
                    RateBasis = theModel.RateBasis,
                    MemberServiceCharge = theModel.MemberFee,
                    NonMemberServiceCharge = theModel.NonMemberFee,
                    EndDate = theModel.EndDate,
                    ServiceOwner = ECCSAModuleFlag.Academy,
                    ApprovalRequired = false,
                    ApprovalAttachmentRequired = false,
                }, TheUser.Id);
                var payload = await _trainingRepository.Update(new GenericInput<UpdateTrainingInput>(theModel, TheUser.Id), serviceType.Id);
                //
                if (needNotification)
                {
                    //
                    var subject = $"Change on Training '{topic.NameEng}'";
                    var body = $"Dear, New Value for the Training" + Environment.NewLine +
                        $"Date: {theModel.StartDate.ToString("dd/MM/yyyy")} - {theModel.EndDate.ToString("dd/MM/yyyy")}" + Environment.NewLine +
                        $"Session(s): {(theModel.HasMorning ? "Morning" : "")} {(theModel.HasAfternoon ? "Afternoon" : "")} {(theModel.HasEvening ? "Evening" : "")}" + Environment.NewLine +
                        $"Address: {theModel.Schedule}" + Environment.NewLine +
                        $"Instructor: {theModel.Instructor}" + Environment.NewLine +
                        $"Instruction Media: {theModel.InstructionMedia}" + Environment.NewLine;
                    await SaveNotification(ECCSAModuleFlag.Academy, subject, body,
                        training.Registrations.ToList<INotificationAddress>(),
                        null, SendThroughFlag.Email);
                    //
                }
                //
                CommandStatus(CommandStatusEnum.Updated);
            }
            catch (Exception ex)
            {
                TempData["CommandStatus"] = ex.Message;
            }
            return RedirectToAction("Detail", new { id = theModel.Id });
        }
        [HttpPost]
        public async Task<IActionResult> DeleteTraining(Guid id)
        {
            LogStart(_logger, controllerName, "DeleteTraining");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Academy);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "DeleteTraining");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("Index");
            }
            var training = (await _trainingRepository.TrainingById(id, TheUser.Id));
            if (training != null)
            {
                await _trainingRepository.Delete(training);
                CommandStatus(CommandStatusEnum.Deleted);
            }
            else
            {
                TempData["CommandStatus"] = "Invalid Training";
            }
            return RedirectToAction("Index", new { status = training.Status });
        }
        [HttpPost]
        public async Task<IActionResult> ChangeTrainingStatus(Guid id, TrainingStatus status)
        {
            try
            {
                var payload = await _trainingRepository.UpdateStatus(new
                    GenericInput<UpdateTrainingStatusInput>(new UpdateTrainingStatusInput(id, status), TheUser.Id));
                CommandStatus(CommandStatusEnum.Updated);
            }
            catch (Exception ex)
            {
                TempData["CommandStatus"] = ex.Message;
            }
            return RedirectToAction("Detail", new { id });
        }
        ///
        [HttpGet]
        public async Task<FileResult> DownLoadFile(Guid id, string type, string fileName)
        {
            var filePath = type == "CertificateAttachment" ? CertificateAttachmentsFolder : SponsorAttachmentsFolder;
            return await DownLoadFile(Path.Combine(filePath , Guid2String(id) + Path.GetExtension(fileName)), fileName);
        }

        ///
        [HttpGet]
        public async Task<IActionResult> EditSponsor(Guid id, Guid trainingId)
        {
            var userName = TheUser.FullName;

            var model = new TrainingSponsorViewModel
            {
                FullName = userName,
                CurrentPage = "Trainings",
                TheList = new List<TrainingSponsor>(),
                SponsorsLookup = _sponsorRepository.Sponsors(TheUser.Id).Select(s => new Lookup2
                {
                    Id = s.Id,
                    Name = $"{s.NameEng} ( {s.NameLocal} )"
                }).ToList(),
                TheModel = (await _trainingSponsorRepository.TrainingSponsorById(id, TheUser.Id)) ?? new TrainingSponsor { TrainingId = trainingId },
            };
            model.ChangeActive(MenuItemEnum.AcademyTraining);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CreateSponsor(CreateTrainingSponsorInput theModel, IFormFile attachmentFile)
        {
            try
            {
                if (attachmentFile != null)
                {
                    CheckFileExtension(attachmentFile.FileName ?? "");
                }
                var payload = await _trainingSponsorRepository.Create(new GenericInput<CreateTrainingSponsorInput>(theModel, TheUser.Id));
                string extension = Path.GetExtension(Path.GetFileName(attachmentFile.FileName));
                var shortName = Guid2String(payload.PayLoad.Id);
                var newFileName = String.Concat(shortName, extension);
                await SaveFile(SponsorAttachmentsFolder, newFileName, attachmentFile);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (Exception ex)
            {
                TempData["CommandStatus"] = ex.Message;
            }
            return RedirectToAction("Detail", new { id = theModel.TrainingId, activeTabIndex = 2 });
        }
        [HttpPost]
        public async Task<IActionResult> UpdateSponsor(UpdateTrainingSponsorInput theModel, IFormFile attachmentFile)
        {
            try
            {
                if (attachmentFile != null)
                {
                    await SaveFile(SponsorAttachmentsFolder, theModel.Id.ToString(), attachmentFile);
                }
                var payload = await _trainingSponsorRepository.Update(new GenericInput<UpdateTrainingSponsorInput>(theModel, TheUser.Id));
                CommandStatus(CommandStatusEnum.Updated);
            }
            catch (Exception ex)
            {
                TempData["CommandStatus"] = ex.Message;
            }
            return RedirectToAction("Detail", new { id = theModel.TrainingId, activeTabIndex = 2 });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSponsor(Guid id)
        {
            var sponsor = (await _trainingSponsorRepository.TrainingSponsorById(id, TheUser.Id));
            if (sponsor != null)
            {
                await _trainingSponsorRepository.Delete(sponsor);
                System.IO.File.Delete(SponsorAttachmentsFolder + "\\" + id.ToString());
                CommandStatus(CommandStatusEnum.Deleted);
            }
            else
            {
                TempData["CommandStatus"] = "Invalid Training Sponsor";
            }
            return RedirectToAction("Detail", new { id = sponsor.TrainingId, activeTabIndex = 2 });
        }
        ///

        ///
        [HttpGet]
        public async Task<IActionResult> RegistrationDetail(Guid id, int activeTabIndex = 0)
        {
            var userName = TheUser.FullName;

            var model = new RegistrationViewModel
            {
                FullName = userName,
                CurrentPage = "Training",
                CurrentOperation = "Registration Detail",
                TheList = new List<Registration>(),
                TheModel = await _registrationRepository.RegistrationById(id, TheUser.Id),
                ActiveTabIndex = activeTabIndex
            };
            if (!string.IsNullOrEmpty(model.TheModel.InvoiceNumber))
            {
                if (model.TheModel.InvoiceNumber.Trim().Length > 3)
                {
                    var invoice = await _invoiceRepository.InvoiceByNo(model.TheModel.InvoiceNumber, model.TheModel.Id);
                    if (invoice != null)
                    {
                        model.TheModel.InvoiceDate = invoice.InvoiceDate.Date;
                    }
                    model.TheModel.Today = DateTime.Today;
                }
            }
            model.ChangeActive(MenuItemEnum.AcademyTraining);
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> EditRegistration(Guid id, Guid trainingId)
        {
            var userName = TheUser.FullName;

            var model = new RegistrationViewModel
            {
                FullName = userName,
                CurrentPage = "Trainings",
                TheList = new List<Registration>(),
                SponsorsLookup = _trainingSponsorRepository.TrainingSponsors(TheUser.Id, trainingId).
                Where(s => s.Status == 1).Select(s => new Lookup2
                {
                    Id = s.Id,
                    Name = $"{s.Sponsor.NameEng} ( {s.Sponsor.NameLocal} )"
                }).ToList(),
                DiscountCategory = new List<Lookup> { new Lookup { Id = 1, Name = "None" } },
                TheModel = (await _registrationRepository.RegistrationById(id, TheUser.Id)) ??
                new Registration { TrainingId = trainingId, RegistrationDate = DateTime.Today },
            };
            model.ChangeActive(MenuItemEnum.AcademyTraining);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRegistration(CreateRegistrationInput theModel)
        {
            try
            {
                var payload = await _registrationRepository.Create(new GenericInput<CreateRegistrationInput>(theModel, TheUser.Id));
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (Exception ex)
            {
                TempData["CommandStatus"] = ex.Message;
            }
            return RedirectToAction("Detail", new { id = theModel.TrainingId, activeTabIndex = 1 });
        }
        [HttpPost]
        public async Task<IActionResult> UpdateRegistration(UpdateRegistrationInput theModel)
        {
            try
            {
                var payload = await _registrationRepository.Update(new GenericInput<UpdateRegistrationInput>(theModel, TheUser.Id));
                CommandStatus(CommandStatusEnum.Updated);
            }
            catch (Exception ex)
            {
                TempData["CommandStatus"] = ex.Message;
            }
            return RedirectToAction("Detail", new { id = theModel.TrainingId, activeTabIndex = 1 });
        }

        [HttpGet]
        public async Task<IActionResult> EditRegistrationFeedback(Guid id, Guid trainingId)
        {
            var userName = TheUser.FullName;

            var model = new RegistrationViewModel
            {
                FullName = userName,
                CurrentPage = "Trainings",
                TheList = new List<Registration>(),
                TheModel = (await _registrationRepository.RegistrationById(id, TheUser.Id)) ??
                new Registration { TrainingId = trainingId, RegistrationDate = DateTime.Today },
            };
            model.ChangeActive(MenuItemEnum.AcademyTraining);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateRegistrationFeedback(UpdateRegistrationFeedbackInput theModel, IFormFile attachmentFile)
        {
            try
            {
                if (attachmentFile != null)
                {
                    string extension = Path.GetExtension(Path.GetFileName(attachmentFile.FileName));
                    var shortName = Guid2String(theModel.Id);
                    var newFileName = String.Concat(shortName, extension);
                    await SaveFile(CertificateAttachmentsFolder, newFileName, attachmentFile);
                }
                var payload = await _registrationRepository.Update(new GenericInput<UpdateRegistrationFeedbackInput>(theModel, TheUser.Id));
                CommandStatus(CommandStatusEnum.Updated);
            }
            catch (Exception ex)
            {
                TempData["CommandStatus"] = ex.Message;
            }
            return RedirectToAction("RegistrationDetail", new { id = theModel.Id });
        }
        [HttpPost]
        public async Task<IActionResult> DeleteRegistration(Guid id)
        {
            var registration = (await _registrationRepository.RegistrationById(id, TheUser.Id));
            if (registration != null)
            {
                await _registrationRepository.Delete(registration);
                CommandStatus(CommandStatusEnum.Deleted);
            }
            else
            {
                TempData["CommandStatus"] = "Invalid Training registration";
            }
            return RedirectToAction("Detail", new { id = registration.TrainingId, activeTabIndex = 1 });
        }
        [HttpPost]
        public async Task<IActionResult> GenerateInvoice(Guid id, decimal rate)
        {
            try
            {
                var registration = await _registrationRepository.RegistrationById(id, TheUser.Id);
                var basis = registration.Training.RateBasis;

                var simpleInvoice = new Invoice()
                {
                    SubSystem = ECCSAModuleFlag.Academy,
                    PaymentReason = "Training Registration",
                    ReferenceId = id,
                    BasePrice = decimal.Round(rate / 1.15m, 2, MidpointRounding.ToEven),
                    Discount = 0m,
                    TotalAmount = rate,
                    CompanyName = "",
                    TIN = "",
                    Address = ""
                };
                YesNoEnum asMember = YesNoEnum.NotSet;
                if (basis == RateBasisEnum.Membership)
                {
                    asMember = rate == registration.Training.MemberFee ? YesNoEnum.Yes : YesNoEnum.No;
                }
                var invoice = await _invoiceRepository.GetOrCreate(simpleInvoice, TheUser.Id);

                var registrationPayload = (await _registrationRepository.GenerateInvoice(id, invoice.InvoiceNo, asMember, invoice.TotalAmount, TheUser.Id));
                CommandStatus(CommandStatusEnum.InvoiceGenerated);
            }
            catch (Exception ex)
            {
                TempData["CommandStatus"] = ex.Message;
            }
            return RedirectToAction("RegistrationDetail", new { id });
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> MakePayment(Guid id, string invoiceNo, string receiptNo, DateTime paymentDate, decimal amount, string remark, PaymentMethodEnum paymentMethod)
        //{
        //    remark = string.IsNullOrEmpty(remark) ? string.Empty : remark;
        //    try
        //    {
        //        var registration = await _registrationRepository.RegistrationById(id, TheUser.Id);
        //        var invoice = await _invoiceRepository.InvoiceByNo(registration.InvoiceNumber, registration.Id);
        //        if (invoice.InvoiceDate.Date > paymentDate.Date || paymentDate > DateTime.Today)
        //        {
        //            CommandStatus(CommandStatusEnum.ValidationError);
        //            TempData["CommandStatus"] = "Payment Date out of range";
        //            return RedirectToAction("RegistrationDetail", new { id });
        //        }
        //        var settlement = await _invoiceRepository.MakePayment(id, invoiceNo, amount, paymentDate, receiptNo, paymentMethod, remark, TheUser.Id);
        //        await _registrationRepository.RecordPayment(id, invoiceNo, settlement.ReceiptNo, paymentMethod, remark, TheUser.Id);
        //        CommandStatus(CommandStatusEnum.PaymentMade);
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["CommandStatus"] = ex.Message;
        //    }

        //    return RedirectToAction("RegistrationDetail", new { id });

        //}
        [HttpPost]
        public async Task<IActionResult> CancelRegistration(Guid id)
        {
            try
            {
                await _registrationRepository.ChangeStatus(id, TheUser.Id, RegistrationStatus.Canceled);
                CommandStatus(CommandStatusEnum.Updated);
            }
            catch (Exception ex)
            {
                TempData["CommandStatus"] = ex.Message;
            }
            return RedirectToAction("RegistrationDetail", new { id });
        }
        [HttpPost]
        public async Task<IActionResult> AcceptRegistration(Guid id)
        {
            try
            {
                await _registrationRepository.ChangeStatus(id, TheUser.Id, RegistrationStatus.Accepted);
                CommandStatus(CommandStatusEnum.Updated);
            }
            catch (Exception ex)
            {
                TempData["CommandStatus"] = ex.Message;
            }
            return RedirectToAction("RegistrationDetail", new { id });
        }
        [HttpPost]
        public async Task<IActionResult> RejectRegistration(Guid id)
        {
            try
            {
                await _registrationRepository.ChangeStatus(id, TheUser.Id, RegistrationStatus.Rejected);
                CommandStatus(CommandStatusEnum.Updated);
            }
            catch (Exception ex)
            {
                TempData["CommandStatus"] = ex.Message;
            }
            return RedirectToAction("RegistrationDetail", new { id });
        }

        [HttpGet]
        public async Task<IActionResult> EditAssessment(Guid id, Guid registrationId, Guid trainingId)
        {
            var userName = TheUser.FullName;

            var model = new AssessmentViewModel
            {
                FullName = userName,
                CurrentPage = "Trainings",
                TheList = new List<Assessment>(),
                TheModel = (await _assessmentRepository.AssessmentById(id, TheUser.Id)) ??
                new Assessment { RegistrationId = registrationId, TrainingId = trainingId, Date = DateTime.Today },
            };
            model.ChangeActive(MenuItemEnum.AcademyTraining);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CreateAssessment(CreateAssessmentInput theModel)
        {
            try
            {
                var payload = await _assessmentRepository.Create(new GenericInput<CreateAssessmentInput>(theModel, TheUser.Id));
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (Exception ex)
            {
                TempData["CommandStatus"] = ex.Message;
            }
            return RedirectToAction("RegistrationDetail", new { id = theModel.RegistrationId, activeTabIndex = 1 });
        }
        [HttpPost]
        public async Task<IActionResult> UpdateAssessment(UpdateAssessmentInput theModel)
        {
            try
            {
                var payload = await _assessmentRepository.Update(new GenericInput<UpdateAssessmentInput>(theModel, TheUser.Id));
                CommandStatus(CommandStatusEnum.Updated);
            }
            catch (Exception ex)
            {
                TempData["CommandStatus"] = ex.Message;
            }
            return RedirectToAction("RegistrationDetail", new { id = theModel.RegistrationId, activeTabIndex = 1 });
        }
        [HttpPost]
        public async Task<IActionResult> DeleteAssessment(Guid id)
        {
            var assessment = (await _assessmentRepository.AssessmentById(id, TheUser.Id));
            if (assessment != null)
            {
                await _assessmentRepository.Delete(assessment);
                CommandStatus(CommandStatusEnum.Deleted);
            }
            else
            {
                TempData["CommandStatus"] = "Invalid Assessment";
            }
            return RedirectToAction("RegistrationDetail", new { id = assessment.RegistrationId, activeTabIndex = 1 });
        }

        [HttpGet]
        public async Task<IActionResult> EditDelivery(Guid id, Guid registrationId, Guid topicId)
        {
            var userName = TheUser.FullName;

            var model = new DeliveryViewModel
            {
                FullName = userName,
                CurrentPage = "Trainings",
                TheList = new List<ResourceDelivery>(),
                ResourcesLookup = _learningResourceRepository.LearningResources(TheUser.Id, topicId)
                .Select(s => new Lookup2 { Id = s.Id, Name = s.Name }).ToList(),
                TheModel = (await _resourceDeliveryRepository.ResourceDeliveryById(id, TheUser.Id)) ??
                new ResourceDelivery { RegistrationId = registrationId, DeliveryDate = DateTime.Today },
            };
            model.ChangeActive(MenuItemEnum.AcademyTraining);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CreateDelivery(CreateResourceDeliveryInput theModel)
        {
            try
            {
                var payload = await _resourceDeliveryRepository.Create(new GenericInput<CreateResourceDeliveryInput>(theModel, TheUser.Id));
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (Exception ex)
            {
                TempData["CommandStatus"] = ex.Message;
            }
            return RedirectToAction("RegistrationDetail", new { id = theModel.RegistrationId, activeTabIndex = 2 });
        }
        [HttpPost]
        public async Task<IActionResult> UpdateDelivery(UpdateResourceDeliveryInput theModel)
        {
            try
            {
                var payload = await _resourceDeliveryRepository.Update(new GenericInput<UpdateResourceDeliveryInput>(theModel, TheUser.Id));
                CommandStatus(CommandStatusEnum.Updated);
            }
            catch (Exception ex)
            {
                TempData["CommandStatus"] = ex.Message;
            }
            return RedirectToAction("RegistrationDetail", new { id = theModel.RegistrationId, activeTabIndex = 2 });
        }
        [HttpPost]
        public async Task<IActionResult> DeleteDelivery(Guid id)
        {
            var delivery = (await _resourceDeliveryRepository.ResourceDeliveryById(id, TheUser.Id));
            if (delivery != null)
            {
                await _resourceDeliveryRepository.Delete(delivery);
                CommandStatus(CommandStatusEnum.Deleted);
            }
            else
            {
                TempData["CommandStatus"] = "Invalid Delivery";
            }
            return RedirectToAction("RegistrationDetail", new { id = delivery.RegistrationId, activeTabIndex = 2 });
        }


        [HttpGet]
        public async Task<IActionResult> EditAttendance(Guid id, Guid registrationId, Guid trainingId)
        {
            var userName = TheUser.FullName;

            var model = new AttendanceViewModel
            {
                FullName = userName,
                CurrentPage = "Trainings",
                TheList = new List<Attendance>(),
                TheModel = (await _attendanceRepository.AttendanceById(id, TheUser.Id)) ??
                new Attendance
                {
                    RegistrationId = registrationId,
                    TrainingId = trainingId,
                    AttendanceDate = DateTime.Today,
                    Training = await _trainingRepository.TrainingById(trainingId, TheUser.Id)
                },
            };
            model.ChangeActive(MenuItemEnum.AcademyTraining);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CreateAttendance(CreateAttendanceInput theModel)
        {
            try
            {
                var payload = await _attendanceRepository.Create(new GenericInput<CreateAttendanceInput>(theModel, TheUser.Id));
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (Exception ex)
            {
                TempData["CommandStatus"] = ex.Message;
            }
            return RedirectToAction("RegistrationDetail", new { id = theModel.RegistrationId, activeTabIndex = 0 });
        }
        [HttpPost]
        public async Task<IActionResult> UpdateAttendance(UpdateAttendanceInput theModel)
        {
            try
            {
                var payload = await _attendanceRepository.Update(new GenericInput<UpdateAttendanceInput>(theModel, TheUser.Id));
                CommandStatus(CommandStatusEnum.Updated);
            }
            catch (Exception ex)
            {
                TempData["CommandStatus"] = ex.Message;
            }
            return RedirectToAction("RegistrationDetail", new { id = theModel.RegistrationId, activeTabIndex = 0 });
        }
        [HttpPost]
        public async Task<IActionResult> DeleteAttendance(Guid id)
        {
            var delivery = (await _attendanceRepository.AttendanceById(id, TheUser.Id));
            if (delivery != null)
            {
                await _attendanceRepository.Delete(delivery);
                CommandStatus(CommandStatusEnum.Deleted);
            }
            else
            {
                TempData["CommandStatus"] = "Invalid Attendance";
            }
            return RedirectToAction("RegistrationDetail", new { id = delivery.RegistrationId, activeTabIndex = 0 });
        }

        private FileStream GetCurrentFile(string fileName, string outFolder)
        {
            //var outFolder = Path.GetTempPath();
            if (!Directory.Exists(outFolder))
                Directory.CreateDirectory(outFolder);

            var tmpFileName = Path.Combine(outFolder, fileName);
            if (System.IO.File.Exists(tmpFileName))
            {
                try
                {
                    var stream = new FileStream(tmpFileName, FileMode.Open, FileAccess.Read);
                    return stream;
                }
                catch (Exception ex)
                {
                    throw new Exception("File in progress. Try later.");
                }
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
        [HttpGet]
        public async Task<IActionResult> PrintCertificate(Guid id)
        {
            var fileName = $"Certificate_{Guid2String(id)}.pdf";
            var path = Path.Combine("wwwroot", "academy", "certificates");
            try
            {
                var stream = GetCurrentFile(fileName, path);
                var reg = await _registrationRepository.RegistrationById(id, TheUser.Id);
                var downloadFileName = $"Certificate_{reg.FullName}.pdf";
                if (stream != null)
                {
                    Response.Headers.Add("content-disposition", "inline;filename=" + downloadFileName);
                    return File(stream, "application/octet-stream", downloadFileName);
                }
                var data = new List<TrainingCertificatePrintModel>
                {
                    new TrainingCertificatePrintModel
                    {
                        FullName =reg.FullName,
                        Instrctor = reg.Training.Instructor,
                        PreparedBy = TheUser.FullName,
                        PreparedByTitle = "Manager, Ethiopian Chamber Academy",
                        Topic = reg.Training.Topic.NameEng,
                        Period = reg.Training.StartDate.ToString("MMMM dd, yyyy") + " to " + reg.Training.EndDate.ToString("MMMM dd, yyyy")
                    }
                };
                stream = GetReportStream<TrainingCertificatePrintModel>("TrainingCertificate.frx", data, "PrintCertificate", fileName, path);

                Response.Headers.Add("content-disposition", "inline;filename=" + downloadFileName);
                //
                if (reg.RequestId != null)
                {
                    var request = await _requestRepository.ServiceRequestById(reg.RequestId.Value, TheUser.Id);

                    MemoryStream memoryStream = new();
                    await stream.CopyToAsync(memoryStream);
                    IFormFile file = new FormFile(memoryStream, 0, memoryStream.Length, fileName, fileName);
                    await SaveDocument(request.Id, request.ServiceType.ServiceOwner, file);
                    request.Extension = Path.GetExtension(Path.GetFileName(fileName));
                    request.Signature = await GetHashCode(file);
                    await _requestRepository.UpdateAttachment(request.Id, request.Extension, request.Signature, TheUser.Id);
                    //
                    var subject = $"Certificate Generated for Your Training {reg.Training.Topic.NameEng}";
                    var body = $"Dear, Please see the link below to get Certificate.";
                    await SaveNotification(ECCSAModuleFlag.Academy, subject, body,
                        new List<INotificationAddress> { reg },
                        file, SendThroughFlag.Email | SendThroughFlag.SMS);
                    //
                }
                //
                return File(stream, "application/octet-stream", downloadFileName);
            }
            catch (Exception ex)
            {
                TempData["CommandStatus"] = ex.Message;
            }
            return RedirectToAction("RegistrationDetail", new { id = id });
        }
    }
}
