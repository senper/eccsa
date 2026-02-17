using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Common.Notification;
using ECCSA.BackOffice.Server.Model.Membership;
using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Notification;
using ECCSA.BackOffice.Server.Persistence.Repositories.Membership;
using ECCSA.BackOffice.Server.Persistence.Repositories.Security;
using ECCSA.BackOffice.Server.WebApp.Areas.Membership.Model;
using ECCSA.BackOffice.Server.WebApp.Controllers;
using ECCSA.BackOffice.Server.WebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using FastReport.Export.PdfSimple;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Membership.Controllers
{
    [Area("Membership")]
    public class MeetingController : BaseController
    {
        private ILogger<MeetingController> _logger;
        MeetingRepository _repository;
        NotificationRepository _notificationrepository;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public MeetingController(ILogger<MeetingController> logger, UserManager<User> userManager, IWebHostEnvironment hostingEnvironment, PublicUserRepository userRepository, NotificationRepository notificationrepository, MeetingRepository repository, NotificationRepository notificationRepository) : base(userManager, userRepository, notificationRepository)
        {
            _logger = logger;
            _repository = repository;
            _notificationrepository = notificationrepository;
            _hostingEnvironment = hostingEnvironment;
            controllerName = "Meeting";
        }

        public async Task<IActionResult> Index()
        {
            var model = new MeetingViewModel();
            
            _logger.LogInformation("Meeting Index Started");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Membership);
            if (roleLevel <= RoleLevelEnum.None && !TheUser.IsHeadOfficeUser)
            {
                LogSecurityError(_logger, controllerName, "Index");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            try
            {
                model.TheList = await _repository.Meetings();
            }
            catch (Exception ex)
            {
                model.TheList = new List<Meeting>();
                _logger.LogError(ex, "Meeting Index Exception");
                return RedirectToAction("Error", "Home", new { area = "" });
            }
            model.CurrentPage = "Meeting";
            model.CurrentOperation = "Index";
            model.ChangeActive(MenuItemEnum.Meeting);
            model.RoleLevel = roleLevel;
            return View(model);
        }

        public async Task<IActionResult> Detail(Guid id, int activeTabIndex = 0)
        {
            _logger.LogInformation("Meeting Detail Started");
            var model = new MeetingViewModel();
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Membership);
            
            if (roleLevel <= RoleLevelEnum.None && !TheUser.IsHeadOfficeUser)
            {
                LogSecurityError(_logger, controllerName, "Index");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            try
            {
                model.TheModel = await _repository.Meeting(id);
                if (model.TheModel != null)
                {
                    foreach (var af in model.TheModel.Artifacts)
                    {
                        var fileName = Guid2String(af.Id);
                        var filePath = AbsolutePath(ECCSAModuleFlag.Membership);
                        var extension = GetExtension(filePath, fileName);
                        af.RelativePath = string.Empty;
                        if (!string.IsNullOrEmpty(extension))
                            af.RelativePath = Path.Combine(RelativePath(ECCSAModuleFlag.Membership), $"{fileName}{extension}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Meeting Detail Exception {id}");
            }
            if (model.TheModel == null)
            {
                return RedirectToAction("Error", "Home", new { area = "" });
            }
            // model.TheModel.MediaType2Bool();
            model.CurrentPage = "Meeting";
            model.CurrentOperation = "Detail";
            model.RoleLevel = roleLevel;
            model.ActiveTabIndex = activeTabIndex;
            model.ChangeActive(MenuItemEnum.Meeting);
            return View(model);
        }

        public async Task<IActionResult> PublicIndex()
        {
            var model = new MeetingViewModel();
            _logger.LogInformation("Meeting Index Started");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Membership);
            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Index");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            try
            {
                model.TheList = await _repository.Meetings();
            }
            catch (Exception ex)
            {
                model.TheList = new List<Meeting>();
                _logger.LogError(ex, "Meeting Index Exception");
            }
            model.CurrentPage = "General Assembly";
            model.CurrentOperation = "Index";
            model.RoleLevel = roleLevel;
            model.ChangeActive(MenuItemEnum.Meeting);
            return View(model);
        }

        public async Task<IActionResult> PublicDetail(Guid id)
        {
            _logger.LogInformation("General Assembly Detail Started");
            var model = new MeetingViewModel();
            var associationId =  TheUser.AssociationId;
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Membership);
            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Index");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            try
            {
                model.TheModel = await _repository.Meeting(id);
                model.TheParticipant = model.TheModel.Participants.FirstOrDefault(p => p.AssociationId == associationId);
                if (model.TheModel != null)
                {
                    foreach (var af in model.TheModel.Artifacts)
                    {
                        var fileName = Guid2String(af.Id);
                        var filePath = AbsolutePath(ECCSAModuleFlag.Membership);
                        var extension = GetExtension(filePath, fileName);
                        af.RelativePath = string.Empty;
                        if (!string.IsNullOrEmpty(extension))
                            af.RelativePath = Path.Combine(RelativePath(ECCSAModuleFlag.Membership), $"{fileName}{extension}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Meeting Detail Exception {id}");
            }

            // model.TheModel.MediaType2Bool();
            model.CurrentPage = "General Assembly";
            model.CurrentOperation = "Detail";
            model.RoleLevel = roleLevel;
            model.ChangeActive(MenuItemEnum.Meeting);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Save(Meeting model)
        {
            try
            {
                if (model.Id == Guid.Empty)
                {
                    model.Id = Guid.NewGuid();
                    model.Code = DateTime.Today.Year.ToString();
                }
                await _repository.Save(model, TheUser.Id);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("Save", ex, model);
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Save", ex, model);
                CommandStatus(CommandStatusEnum.UnkownError);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ChangeStatus(Guid id, MeetingStatusEnum status)
        {
            var model = await _repository.Meeting(id);
            if(model == null)
            {
                CommandStatus("Cannot change status");
                return RedirectToAction("Index");
            }
            if (model.Status == MeetingStatusEnum.Closed)
            {
                CommandStatus("Cannot change status closed ");
                return RedirectToAction("Index");
            }
            model.Status = status;
            try
            {
                await _repository.Save(model, TheUser.Id);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("Save", ex, model);
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Save", ex, model);
                CommandStatus(CommandStatusEnum.UnkownError);
            }

            return RedirectToAction("Index");
        }

        private async Task<String> SaveDocument(Guid id, IFormFile file)
        {
            string fileName = file.FileName;
            string newShortName = Guid2String(id);
            string extension = Path.GetExtension(Path.GetFileName(fileName));
            var newFileName = String.Concat(newShortName, extension);
            return await SaveFile(AbsolutePath(ECCSAModuleFlag.Membership), newFileName, file, newShortName);
        }
        [HttpPost]
        public async Task<IActionResult> SaveDistingueshedGuest(Guest data, Guid meetingId)
        {

            try
            {
                await _repository.Save(meetingId, data, TheUser.Id);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("Save Distingueshed Guest", ex, new { meetingId, data });
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Save Artifact", ex, data);
                CommandStatus(CommandStatusEnum.UnkownError);
            }

            return RedirectToAction("Detail", new { id = meetingId, activeTabIndex = 5 });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteDistingueshedGuest(Guest data, Guid meetingId)
        {

            try
            {

                await _repository.Delete(meetingId, data, TheUser.Id);
                CommandStatus(CommandStatusEnum.Deleted);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("Delete Distingueshed Guest", ex, new { meetingId, data });
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Save Artifact", ex, data);
                CommandStatus(CommandStatusEnum.UnkownError);
            }

            return RedirectToAction("Detail", new { id = meetingId, activeTabIndex = 5 });
        }
        private void DeleteBadge(Guid id, Guid meetingId)
        {
            var gd = Guid2String(id);

            var fileName = $"Passbadge_{gd}.pdf";
            var outFolder = Path.GetTempPath();
            if (!Directory.Exists(outFolder))
                Directory.CreateDirectory(outFolder);

            var tmpFileName = Path.Combine(outFolder, fileName);
            if (System.IO.File.Exists(tmpFileName))
            {
                System.IO.File.Delete(tmpFileName);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteOtherParticipant(Guest data, Guid meetingId, Guid participantId)
        {

            try
            {

                await _repository.Delete(meetingId, data, TheUser.Id);
                DeleteBadge(participantId, meetingId);
                CommandStatus(CommandStatusEnum.Deleted);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("Delete Distingueshed Guest", ex, new { meetingId, data });
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Save Artifact", ex, data);
                CommandStatus(CommandStatusEnum.UnkownError);
            }

            return RedirectToAction("PublicDetail", new { id = meetingId });
        }

        [HttpPost]
        public async Task<IActionResult> SaveOtherParticipant(Guest data, Guid meetingId, Guid participantId)
        {

            try
            {

                await _repository.Save(meetingId, participantId, data, TheUser.Id);
                 DeleteBadge(participantId, meetingId);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("Save Artifact", ex, new { meetingId, participantId, data });
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Save Artifact", ex, new { meetingId, participantId, data });
                CommandStatus(CommandStatusEnum.UnkownError);
            }

            return RedirectToAction("PublicDetail", new { id = meetingId });
        }
        [HttpPost]
        public async Task<IActionResult> SaveArtifact(MeetingViewModel vmodel)
        {
            var model = new Artifact()
            {
                Id = vmodel.Id == Guid.Empty ? Guid.NewGuid() : vmodel.Id,
                Title = vmodel.Title,
                MeetingId = vmodel.MeetingId,
                Description = vmodel.Description,
            };
            try
            {
                if (vmodel.Attachment != null && vmodel.Attachment.Length > 0)
                {
                    await SaveDocument(model.Id, vmodel.Attachment);
                }
                await _repository.Save(model, TheUser.Id);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("Save Artifact", ex, model);
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Save Artifact", ex, model);
                CommandStatus(CommandStatusEnum.UnkownError);
            }

            return RedirectToAction("Detail", new { id = model.MeetingId, activeTabIndex = 4 });
        }

        [HttpPost]
        public async Task<IActionResult> SaveParticipant(Participant model)
        {
            try
            {
                var data = await _repository.Participant(model.Id);
                data.FullName = model.FullName;
                data.Email = model.Email;
                data.Mobile = model.Mobile;
                await _repository.Save(data, TheUser.Id);
                 DeleteBadge(model.Id, model.MeetingId);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("Save Participant", ex, model);
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Save Participant", ex, model);
                CommandStatus(CommandStatusEnum.UnkownError);
            }

            return RedirectToAction("PublicDetail", new { id = model.MeetingId });
        }

        [HttpPost]
        public async Task<IActionResult> SaveAttendance(MeetingAttendance model)
        {
            try
            {
                await _repository.Save(model, TheUser.Id);
               
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("Save Participant", ex, model);
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Save Participant", ex, model);
                CommandStatus(CommandStatusEnum.UnkownError);
            }

            return RedirectToAction("Detail", new { id = model.MeetingId, activeTabIndex = 2 });
        }
        [HttpPost]
        public async Task<IActionResult> SaveMinuteStatus(Participant model)
        {
            try
            {
                var data = await _repository.Participant(model.Id);
                data.MinuteStatus = model.MinuteStatus;
                data.Remark = String.IsNullOrEmpty(model.Remark) ? "" : model.Remark;
                await _repository.Save(data, TheUser.Id);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("Save Participant", ex, model);
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Save Participant", ex, model);
                CommandStatus(CommandStatusEnum.UnkownError);
            }

            return RedirectToAction("PublicDetail", new { id = model.MeetingId });
        }
        [HttpPost]
        public async Task<IActionResult> DeleteArtifact(Guid id, Guid meetingId)
        {
            try
            {
                var fileName = Guid2String(id);
                var filePath = AbsolutePath(ECCSAModuleFlag.Membership);
                var extension = GetExtension(filePath, fileName);
                if (!string.IsNullOrEmpty(extension))
                {
                    var path = Path.Combine(filePath, $"{fileName}{extension}");
                    System.IO.File.Delete(path);
                }
                await _repository.DeleteArtifact(TheUser.Id, id);
                CommandStatus(CommandStatusEnum.Deleted);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Artifact Deleted");
                CommandStatus(CommandStatusEnum.CannotDelete);
            }

            return RedirectToAction("Detail", new { id = meetingId, activeTabIndex = 4 });
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmail(Guid id)
        {
            var meeting = await _repository.Meeting(id);
            var participants = meeting.Participants.ToList();

            try
            {
                var email = new EmailMessage()
                {
                    Id = Guid.NewGuid(),
                    Owner = ECCSAModuleFlag.Membership,
                    Subject = $"{meeting.Title}",
                    Body = @$"
                                Venue: {meeting.Venue}
                                Date: {meeting.Date.ToString("yyyy-MM-dd")}
                                Agenda: {meeting.Agenda}
                             ",
                    Recipients = new List<MessageDelivery>(),
                    Status = MessageStatus.Draft,
                    Attachment = "",
                    ReferenceId = id
                };
                await _notificationrepository.Save(email, TheUser.Id);
                CommandStatus("Email Created Successfully");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("Email Create", ex, meeting);
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Email Create", ex, meeting);
                CommandStatus(CommandStatusEnum.UnkownError);
            }
            return RedirectToAction("Detail", new { id, activeTabIndex = 3 });
        }

        [HttpPost]
        public async Task<IActionResult> SaveNominee(Nominee model)
        {
            try
            {
                await _repository.Save(model, TheUser.Id);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("Save Participant", ex, model);
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Save Participant", ex, model);
                CommandStatus(CommandStatusEnum.UnkownError);
            }

            return RedirectToAction("Detail", new { id = model.MeetingId, activeTabIndex = 1 });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteNominee(Guid id, Guid meetingId)
        {
            try
            {
                await _repository.DeleteNominee(id, meetingId, TheUser.Id);
                CommandStatus(CommandStatusEnum.Deleted);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Artifact Deleted");
                CommandStatus(CommandStatusEnum.CannotDelete);
            }

            return RedirectToAction("Detail", new { id = meetingId, activeTabIndex = 1 });
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
        [HttpGet]
        public async Task<IActionResult> PrintMeetingCard(Guid id, Guid meetingId)
        {
            var data = new List<Badge>();
            var gd = Guid2String(id) ;

            var fileName = $"Passbadge_{gd}.pdf";
            var stream = GetCurrentFile(fileName);
            if (stream != null)
            {
                Response.Headers.Add("content-disposition", "inline;filename=" + fileName);
                return File(stream, "application/pdf");
            }
            try
            {
                var meeting = await _repository.Meeting(meetingId);
                var participant = meeting.Participants.FirstOrDefault(m => m.Id == id);
                participant.OtherParticipants.Insert(0, new Guest()
                {
                    Code = participant.Code,
                    FullName = participant.FullName,
                    Title = "Main Participant"
                });
                var count = participant.OtherParticipants.Count;
                var pi = new Badge();
                var i = 0;
                var q = i / 4;
                while (i < count)
                {
                    var d = participant.OtherParticipants[i];
                    switch (i % 4)
                    {
                        case 0:
                            
                            pi = new Badge
                            {
                                AssociationName1 = participant.Association,
                                FullName1 = d.FullName,
                                Title1 = d.Title,
                                Code1 = d.Code,
                                No = $"{meeting.Date.Year}-0{i / 4}"
                            };
                           
                            data.Add(pi);
                            break;
                        case 1:
                            pi.AssociationName2 = participant.Association;
                            pi.FullName2 = d.FullName;
                            pi.Title2 = d.Title;
                            pi.Code2 = d.Code;
                            break;
                        case 2:
                            pi.AssociationName3 = participant.Association;
                            pi.FullName3 = d.FullName;
                            pi.Title3 = d.Title;
                            pi.Code3 = d.Code;
                            break;
                        case 3:
                            pi.AssociationName4 = participant.Association;
                            pi.FullName4 = d.FullName;
                            pi.Title4 = d.Title;
                            pi.Code4 = d.Code;
                            break;
                    }
                    i++;
                }

                stream = GetReportStream<Badge>("MeetingBadge.frx", data, "Badge", fileName);
                return File(stream, "application/octet-stream", fileName);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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
