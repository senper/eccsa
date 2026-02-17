using ECCSA.BackOffice.Server.Model.Advocacy;
using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Advocacy;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Notification;
using ECCSA.BackOffice.Server.Persistence.Repositories.Security;
using ECCSA.BackOffice.Server.WebApp.Areas.Advocacy.Model;
using ECCSA.BackOffice.Server.WebApp.Controllers;
using ECCSA.BackOffice.Server.WebApp.Models;
using ECCSA.BackOffice.Server.WebApp.ReportServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Advocacy.Controllers
{
    [Area("Advocacy")]
    public class DialogController : BaseController
    {
        private ILogger<DialogController> _logger;
        private readonly IReportService _pdfService;
        
        AdvocacyRepository _repository;
        public DialogController(ILogger<DialogController> logger,
            UserManager<User> userManager,
            IReportService reportService,
            PublicUserRepository userRepository,
            AdvocacyRepository repository, NotificationRepository notificationRepository) : base(userManager, userRepository, notificationRepository)
        {

            _logger = logger;
            _repository = repository;

            _pdfService = reportService;
            controllerName = "Dialog";
        }

        public async Task<IActionResult> Index()
        {
            var model = new DialogViewModel();
           LogStart(_logger, controllerName, "Index");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Research);
            if(roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Index");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            try
            {
                model.TheList = await _repository.Dialogs();
            }
            catch (Exception ex)
            {
                model.TheList = new List<Dialog>();
                _logger.LogError(ex, "Dialog Index Exception");
            }
            model.CurrentOperation = "Index";
            model.RoleLevel = await GetRoleLevel(Server.Model.Common.ECCSAModuleFlag.Research);
            model.ChangeActive(MenuItemEnum.AdvocacyDialog);
            model.RoleLevel = roleLevel;
            LogEnd(_logger, controllerName, "Index");
           
            return View(model);
        }
        public async Task<IActionResult> Pdf()
        {
            var model = new DialogViewModel();
           

            try
            {
                model.TheList = await _repository.Dialogs();
            }
            catch (Exception ex)
            {
                model.TheList = new List<Dialog>();
                _logger.LogError(ex, "Dialog Index Exception");
            }
            model.CurrentPage = "Dialog";
            model.CurrentOperation = "Index";
            model.ChangeActive(MenuItemEnum.AdvocacyDialog);

            var pdfFile = _pdfService.GeneratePdfReport();
            return File(pdfFile,
            "application/octet-stream", "SimplePdf.pdf");
           
        }
        public async Task<IActionResult> Detail(Guid id, int activeTabIndex = 0)
        {
            LogStart(_logger, controllerName, "Detail");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Research);
            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Detail");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            var model = new DialogViewModel();
            try
            {
                model.TheModel = await _repository.DialogById(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Dialog Detail Exception {id}");
            }
            // model.TheModel.MediaType2Bool();
            model.CurrentPage = "Dialog";
            model.RoleLevel = await GetRoleLevel(ECCSAModuleFlag.Research);
            model.CurrentOperation = "Detail";
            model.RoleLevel = roleLevel;
            model.ActiveTabIndex = activeTabIndex;
            LogEnd(_logger, controllerName, "Detail");
            model.ChangeActive(MenuItemEnum.AdvocacyDialog);
            if (model.TheModel == null)
            {
                return RedirectToAction("Error", "Home", new { area = "" });
            }
            return View(model);
        }
        public async Task<IActionResult> Meeting(Guid id, int activeTabIndex = 0)
        {
            LogStart(_logger, controllerName, "Meeting");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Research);
            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Detail");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            var model = new DialogMeetingViewModel();
            try
            {
                model.TheModel = await _repository.MeetingById(id);
                model.TheDialog = model.TheModel.Dialog;
                model.Issues = model.TheDialog.Recommendations.Where(i => i.Status != PPDRecommendationEnum.Accepted 
                && i.Status != PPDRecommendationEnum.Rejected
                && !model.TheModel.Points.Any(p=> p.RecommendationId == i.Id)).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Dialog Detail Exception {id}");
            }
            // model.TheModel.MediaType2Bool();
            model.CurrentPage = "Meeting";
            model.RoleLevel = await GetRoleLevel(ECCSAModuleFlag.Research);
            model.CurrentOperation = "Meeting";
            model.RoleLevel = roleLevel;
            model.ActiveTabIndex = activeTabIndex;
            LogEnd(_logger, controllerName, "Meeting");
            model.ChangeActive(MenuItemEnum.AdvocacyDialog);
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Save(Dialog model)
        {
            LogStart(_logger, controllerName, "Save");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Research);
            if(roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "Save");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("Index");
            }
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
        [HttpPost]
        public async Task<IActionResult> CreateRecommendation(Recommendation model)
        {
            LogStart(_logger, controllerName, "Save");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Research);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "Save");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("Index");
            }
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

            return RedirectToAction("Detail", new {id=model.DialogId, activeTabIndex = 0 });
        }

        [HttpPost]
        public async Task<IActionResult> SaveMeeting(PPDMeeting model)
        {
            LogStart(_logger, controllerName, "Save");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Research);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "Save");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("Index");
            }
            try
            {
                if (model.Id == Guid.Empty)
                    model.Id = Guid.NewGuid();
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

            return RedirectToAction("Meeting", new { id = model.Id, activeTabIndex = 0 });
        }


        [HttpPost]
        public async Task<IActionResult> SaveMeetingPoint(MeetingPoint model)
        {
            LogStart(_logger, controllerName, "Save");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Research);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "Save");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("Meeting", new { id = model.MeetingId, activeTabIndex = 0 });
            }
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

            return RedirectToAction("Meeting", new { id = model.MeetingId, activeTabIndex = 0 });
        }


        [HttpPost]
        public async Task<IActionResult> SaveAttachment(PPDAttachment model, IFormFile file)
        {
            model.Id = model.Id == Guid.Empty ? Guid.NewGuid() : model.Id;
            LogStart(_logger, controllerName, "Save");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Research);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "Save");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("Index");
            }

            if (file == null || file.Length == 0)
            {
                CommandStatus("upload file missing");
                return RedirectToAction("Index");
            }
            try
            {
                if (file != null && file.Length > 0)
                {
                    model.Extension = await SaveDocumentExtension(model.Id, ECCSAModuleFlag.Research, file);
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

            return RedirectToAction("Meeting", new { id = model.MeetingId, activeTabIndex = 1 });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMeetingPoint(MeetingPoint model)
        {
            LogStart(_logger, controllerName, "DeleteMeetingPoint");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Research);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "DeleteMeetingPoint");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("Meeting", new { id = model.MeetingId, activeTabIndex = 0 });
            }
            try
            {

                await _repository.DeleteMeetingPoint(model.Id, TheUser.Id);
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

            return RedirectToAction("Meeting", new { id = model.MeetingId, activeTabIndex = 0 });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAttachment(PPDAttachment model)
        {
            LogStart(_logger, controllerName, "DeleteAttachment");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Research);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "DeleteAttachment");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("Meeting", new { id = model.MeetingId, activeTabIndex = 0 });
            }
            try
            {

                await _repository.DeleteAttachment(model.Id);
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

            return RedirectToAction("Meeting", new { id = model.MeetingId, activeTabIndex = 0 });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteIssue(Recommendation model)
        {
            LogStart(_logger, controllerName, "DeleteIssue");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Research);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "DeleteIssue");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("Detail", new { id = model.DialogId, activeTabIndex = 0 });
            }
            try
            {

                await _repository.DeleteRecommendation(model.Id);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("DeleteIssue", ex, model);
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("DeleteIssue", ex, model);
                CommandStatus(CommandStatusEnum.UnkownError);
            }

            return RedirectToAction("Detail", new { id = model.DialogId, activeTabIndex = 0 });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMeeting(PPDMeeting model)
        {
            LogStart(_logger, controllerName, "DeleteMeeting");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Research);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "DeleteMeeting");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("Detail", new { id = model.DialogId, activeTabIndex = 0 });
            }
            try
            {

                await _repository.DeleteRecommendation(model.Id);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("DeleteMeeting", ex, model);
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("DeleteMeeting", ex, model);
                CommandStatus(CommandStatusEnum.UnkownError);
            }

            return RedirectToAction("Detail", new { id = model.DialogId, activeTabIndex = 2 });
        }

        [HttpPost]
        public async Task<IActionResult> Activate(Guid id)
        {
            LogStart(_logger, controllerName, "Activate");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Research);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "Activate");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("Detail", new { id , activeTabIndex = 0 });
            }
            var model = await _repository.DialogById(id);
            try
            {
               
                model.Status = StatusEnum.Active;
                await _repository.Save(model,TheUser.Id);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("Activate", ex, model);
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Activate", ex, model);
                CommandStatus(CommandStatusEnum.UnkownError);
            }

            return RedirectToAction("Detail", new { id , activeTabIndex = 0 });
        }
        [HttpPost]
        public async Task<IActionResult> Deactivate(Guid id)
        {
            LogStart(_logger, controllerName, "Deactivate");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Research);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "Deactivate");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("Detail", new { id, activeTabIndex = 0 });
            }
            var model = await _repository.DialogById(id);
            try
            {

                model.Status = StatusEnum.Inactive;
                await _repository.Save(model, TheUser.Id);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("Deactivate", ex, model);
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Deactivate", ex, model);
                CommandStatus(CommandStatusEnum.UnkownError);
            }

            return RedirectToAction("Detail", new { id, activeTabIndex = 0 });
        }
    }

}
