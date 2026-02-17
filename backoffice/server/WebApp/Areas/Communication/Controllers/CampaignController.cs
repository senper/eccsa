using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Common.Notification;
using ECCSA.BackOffice.Server.Model.Communication;
using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Notification;
using ECCSA.BackOffice.Server.Persistence.Repositories.Communication;
using ECCSA.BackOffice.Server.Persistence.Repositories.Security;
using ECCSA.BackOffice.Server.WebApp.Areas.Communication.Model;
using ECCSA.BackOffice.Server.WebApp.Controllers;
using ECCSA.BackOffice.Server.WebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Communication.Controllers
{
    [Area("Communication")]
    public class CampaignController : BaseController
    {
        private readonly ILogger<CampaignController> _logger;
        readonly MediaAgencyRepository _repository;
        public CampaignController(ILogger<CampaignController> logger,
            UserManager<User> userManager,
            MediaAgencyRepository repository,
            PublicUserRepository userRepository,
            NotificationRepository notificationRepository
            ) : base(userManager, userRepository, notificationRepository)
        {
            _logger = logger;
            _repository = repository;
            controllerName = "Campaign";
        }
        public async Task<IActionResult> Index()
        {
            var model = new CampaignViewModel();
            LogStart(_logger, controllerName, "Index");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Communication);
            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Index");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            try
            {
                model.TheList = await _repository.Campaigns() ?? new List<Campaign>();
            }
            catch (Exception ex)
            {
                model.TheList = new List<Campaign>();
                _logger.LogError(ex, "Campaign Index Exception");
                return RedirectToAction("Error", "Home", new { area = "" });
            }
            model.CurrentPage = "Campaign";
            model.CurrentOperation = "Index";
            model.ChangeActive(MenuItemEnum.CommunicationCampaign);
            model.RoleLevel = roleLevel;
            LogEnd(_logger, controllerName, "Index");
            return View(model);
        }

        public async Task<IActionResult> Detail(Guid id)
        {
            LogStart(_logger, controllerName, "Detail");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Communication);
            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Detail");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            var model = new CampaignViewModel();
            try
            {
                model.TheModel = await _repository.CampaignById(id) ?? new Campaign();
                model.MediaAgencies = await _repository.MediaAgencies();
                model.AttachmentPath = GetAttachment(id, model.TheModel.Extension);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Campaign Detail Exception {id}");
            }

            if (model.TheModel == null)
            {
                return RedirectToAction("Error", "Home", new { area = "" });
            }
            model.CurrentPage = "Campaign";
            model.CurrentOperation = "Detail";
            model.ChangeActive(MenuItemEnum.CommunicationCampaign);
            model.RoleLevel = roleLevel;
            LogEnd(_logger, controllerName, "Detail");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Save(Campaign model, IFormFile material)
        {
            LogStart(_logger, controllerName, "Save");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Communication);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "Save");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("Index");
            }
            var oldId = model.Id;
            model.Id = model.Id == Guid.Empty ? Guid.NewGuid() : model.Id;

            try
            {
                if (material is { Length: > 0 })
                {
                    model.Extension = await SaveDocument(model.Id, material);
                }
                await _repository.Save(model, TheUser.Id);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (Exception ex)
            {
                _logger.LogError("Save", ex, model);
                CommandStatus(CommandStatusEnum.UnkownError);
            }
            return oldId == Guid.Empty ? RedirectToAction("Index") : RedirectToAction("Detail", new { id = model.Id });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveParticipants(Guid id, List<Guid> agencies)
        {
            LogStart(_logger, controllerName, "SaveParticipants");
            try
            {
                await _repository.ChangeParticipants(id, agencies);
                CommandStatus("Participants saved Successfully");
            }
            catch (Exception ex)
            {
                LogError(_logger, ex, controllerName, "SaveParticipants", new { id, agencies });
                TempData["CommandStatus"] = ex.Message;
            }
            LogEnd(_logger, controllerName, "SaveParticipants");
            return RedirectToAction("Detail", new { id });
        }

        private string GetAttachment(Guid mediaId, string extension)
        {

            var shortName = Guid2String(mediaId);
            var absolutePath = AbsolutePath(ECCSAModuleFlag.Communication);
            var newFileName = String.Concat(shortName, extension);
            string filePath = Path.Combine(absolutePath, newFileName);
            if (System.IO.File.Exists(filePath))
                return Path.Combine(RelativePath(ECCSAModuleFlag.Communication), newFileName);
            return string.Empty;
        }

        [HttpGet]
        public async Task<FileResult> DownLoadFile(Guid id)
        {
            var fileName = Guid2String(id);
            var filePath = AbsolutePath(ECCSAModuleFlag.Communication);
            var model = await _repository.CampaignById(id);

            fileName += model.Extension;
            var title = $"{model.Title} {model.StartDate.ToString("yyyyMMddhh")}-{model.EndDate.ToString("yyyyMMddhh")}on{DateTime.Now.ToString("yyyyMMdd")}-{DateTime.Now.ToString("hhmmss")}{model.Extension}";
            return await DownLoadFile(filePath, fileName, title);
        }
        private async Task<String> SaveDocument(Guid id, IFormFile file, string subfolder = "")
        {
            var absolutePath = AbsolutePath(ECCSAModuleFlag.Communication);
            if (!string.IsNullOrWhiteSpace(subfolder))
                absolutePath = Path.Combine(absolutePath, subfolder);
            var shortName = Guid2String(id);
            string fileName = file.FileName;
            string extension = Path.GetExtension(Path.GetFileName(fileName));
            var newFileName = String.Concat(shortName, extension);
            await SaveFile(absolutePath, newFileName, file, shortName);

            return extension;
        }
        [HttpPost]
        public async Task<IActionResult> SendEmail(Guid id)
        {
            var msg = await _repository.CampaignById(id);
            IFormFile file = null;
            if (msg == null)
            {
                CommandStatus("Message Not found");
                return RedirectToAction("Detail", new { id });
            }

            if (!string.IsNullOrEmpty(msg.Extension))
            {

                var fileName = Guid2String(id);
                var filePath = AbsolutePath(ECCSAModuleFlag.Communication);
                var model = await _repository.MediaCoverageById(id);

                fileName += model.Extension;
                MemoryStream memoryStream = new();
                await using var stream =
                    new FileStream(Path.Combine(filePath, fileName), FileMode.Open, FileAccess.Read);
                await stream.CopyToAsync(memoryStream);
                file = new FormFile(memoryStream, 0, memoryStream.Length, fileName, fileName);
            }

            try
            {
                var subject = $"{msg.Title} {msg.StartDate.ToString("yyyy-MM-dd")}";
                var body = $"<p>{msg.Description}</p><p>{msg.Purpose}</p>";
                await SaveNotification(ECCSAModuleFlag.Communication, subject, body,
                       msg.CampaignPartnerAgencies.Where(s => s.MediaAgency != null)
                       .Select(c => c.MediaAgency).ToList<INotificationAddress>(),
                        file, SendThroughFlag.Email);
                //
                await _repository.Save(msg, TheUser.Id);
                CommandStatus("Successfully Email Sent");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("Send Email", ex, msg);
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Send Email", ex, msg);
                CommandStatus(CommandStatusEnum.UnkownError);
            }
            return RedirectToAction("Detail", new { id });
        }
    }
}
