using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Common.Notification;
using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Model.Trade;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Notification;
using ECCSA.BackOffice.Server.Persistence.Repositories.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories.UserAdministration;
using ECCSA.BackOffice.Server.Persistence.Repositories.Website;
using ECCSA.BackOffice.Server.WebApp.Areas.Communication.Model;
using ECCSA.BackOffice.Server.WebApp.Areas.Service.Model;
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

namespace ECCSA.BackOffice.Server.WebApp.Areas.Communication.Controllers
{
    [Area("Communication")]
    public class NewsletterController : BaseController
    {
        private readonly ILogger<NewsletterController> _logger;
        private readonly NewsRepository _repository;
        private readonly UserAdministrationRepository _userAdminRepository;
        public NewsletterController(
            UserManager<User> userManager
            , ILogger<NewsletterController> logger
            , NewsRepository repository
            , PublicUserRepository userRepository,
            UserAdministrationRepository userAdminRepository,
            NotificationRepository notificationRepository) : base(userManager, userRepository, notificationRepository)
        {
            _logger = logger;
            _repository = repository;
            _userAdminRepository = userAdminRepository;
            controllerName = "Newsletter";
        }
        [HttpGet]
        public async Task<IActionResult> Index(Guid topicId)
        {
            var model = new NewsletterViewModel();
            LogStart(_logger, controllerName, "Index");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Communication);
            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Index");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            try
            {
                model.TheList = await _repository.GetNewsletterIssues(topicId);
                model.Topics = await _repository.Topics();
            }
            catch (Exception ex)
            {
                model.TheList = new List<NewsletterIssue>();
                model.Topics = new List<LookupValue>();
                _logger.LogError(ex, "Index");
                return RedirectToAction("Error", "Home", new { area = "" });
            }
            model.CurrentPage = "Newsletter";
            model.CurrentOperation = "Index";
            model.ChangeActive(MenuItemEnum.Newsletter);
            model.TopicId = topicId;
            model.RoleLevel = roleLevel;
            LogEnd(_logger, controllerName, "Index");

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(Guid id)
        {
            LogStart(_logger, controllerName, "Detail");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Communication);
            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Detail");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            var model = new NewsletterViewModel();
            try
            {
                model.TheModel = await _repository.GetNewsletterIssue(id);
                model.Topics = await _repository.Topics();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $" Detail Exception {id}");
                CommandStatus(ex.Message);
            }
            if (model.TheModel == null)
            {
                return RedirectToAction("Error", "Home", new { area = "" });
            }
            model.CurrentPage = $"Newsletter";
            model.CurrentOperation = "Detail";
            model.ChangeActive(MenuItemEnum.Newsletter);
            model.RoleLevel = roleLevel;
            LogEnd(_logger, controllerName, "Detail");
            return View(model);
        }
        private List<string> GetProfileAttachedImages(Guid associationId)
        {
            var id = Guid2String(associationId);
            string filePath = Path.Combine(NewsProfileImagePath, id);
            if (Directory.Exists(filePath))
            {
                var lst = Directory.GetFiles(filePath).Select(s => $"{NewsProfileRelativePath}{id}/" +
                (s.Split($"{id}\\").Length > 1 ? s.Split($"{id}\\")[1] :
                (s.Split($"{id}/").Length > 1 ? s.Split($"{id}/")[1] : "")
                )).ToList();

                return lst;
            }
            return new List<string>();
        }
        private async Task<String> SaveDocument(Guid id, IFormFile file, ECCSAModuleFlag module, string subfolder = "")
        {
            var absolutePath = AbsolutePath(module);
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(NewsletterIssue model, IFormFile photo)
        {
            if (model.Id == Guid.Empty)
            {
                model.Id = Guid.NewGuid();
            }
            LogStart(_logger, controllerName, "Save");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Communication);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "Save");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("Index");
            }
            try
            {
                if (photo != null && photo.Length > 0)
                {
                    model.Extension = await SaveDocument(model.Id, photo, ECCSAModuleFlag.Communication);
                }

                await _repository.Save(model, TheUser.Id);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("Save", ex, model);
                CommandStatus(ex.Message);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError("Save", ex, model);
                CommandStatus(CommandStatusEnum.UnkownError);
                return RedirectToAction("Index");
            }
            return RedirectToAction("Detail", new { id = model.Id });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Send(Guid id)
        {
            if (id == Guid.Empty)
            {
                return RedirectToAction("Index");
            }
            LogStart(_logger, controllerName, "Send");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Communication);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "Send");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("Index");
            }
            try
            {

                await SendEmail(id);
                await _repository.UpdateSendStatus(id);
                
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("Send", ex, id);
                CommandStatus(ex.Message);
                return RedirectToAction("Detail", new { id });
            }
            catch (Exception ex)
            {
                _logger.LogError("Send", ex, id);
                CommandStatus(CommandStatusEnum.UnkownError);
                return RedirectToAction("Detail", new { id });
            }
            return RedirectToAction("Detail", new { id });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Close(Guid id)
        {
            if (id == Guid.Empty)
            {
                return RedirectToAction("Index");
            }
            LogStart(_logger, controllerName, "Send");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Communication);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "Send");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("Index");
            }
            try
            {
                await _repository.Close(id, TheUser.Id);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("Send", ex, id);
                CommandStatus(ex.Message);
                return RedirectToAction("Detail", new { id });
            }
            catch (Exception ex)
            {
                _logger.LogError("Send", ex, id);
                CommandStatus(CommandStatusEnum.UnkownError);
                return RedirectToAction("Detail", new { id });
            }
            return RedirectToAction("Detail", new { id });
        }

        private async Task SendEmail(Guid issueId)
        {
            var issue = await _repository.GetNewsletterIssue(issueId);

            IFormFile file = null;
            if (string.IsNullOrEmpty(issue.Extension))
            {
                throw new InvalidOperationException("Attachment Not found");
            }
            var id = issue.Id;
            var fname = Guid2String(id) + issue.Extension;
            string fileName = "Newsletter" + issue.Extension;
            var fullName = Path.Combine(AbsolutePath(ECCSAModuleFlag.Communication), fname);
            if (!System.IO.File.Exists(fullName))
            {
                throw new InvalidOperationException("Attachment Not found");
            }
            MemoryStream memoryStream = new();

            using var stream = new FileStream(fullName, FileMode.Open, FileAccess.Read);
            await stream.CopyToAsync(memoryStream);
            file = new FormFile(memoryStream, 0, memoryStream.Length, fileName, fileName);

            //
            var subject = issue.Title + issue.TitleLocal == null?"": issue.TitleLocal;
            var body =  issue.Brief;
            await SaveNotification(ECCSAModuleFlag.Communication, subject, body,
                       issue.Subscribers.ToList<INotificationAddress>(),
                        file, SendThroughFlag.Email);
            //
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            LogStart(_logger, controllerName, "Delete");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Communication);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "Delete");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("Index");
            }
            try
            {
                var res = await _repository.GetNewsletterIssue(id);
                await _repository.Delete(res);
                if (!string.IsNullOrEmpty(res.Extension))
                {
                    var fname = Guid2String(id) + res.Extension;
                    var path = Path.Combine(AbsolutePath(ECCSAModuleFlag.Communication), fname);
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                }

                CommandStatus(CommandStatusEnum.Deleted);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("Delete", ex, id);
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Delete", ex, id);
                CommandStatus(CommandStatusEnum.UnkownError);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<FileResult> DownLoadFile(Guid id, string type, string fileName)
        {
            var filePath = AbsolutePath(ECCSAModuleFlag.Communication);
            string extension = Path.GetExtension(Path.GetFileName(fileName));
            return await DownLoadFile(Path.Combine(filePath, $"{Guid2String(id)}{extension}"), fileName);
        }


        [HttpPost]
        public async Task<IActionResult> Subscribe(Guid userId, Guid topicId)
        {
            await _repository.Subscribe(userId, topicId);
            return RedirectToAction("Subscribers", new { topicId });
        }
        [HttpPost]
        public async Task<IActionResult> UnSubscribe(Guid userId, Guid topicId)
        {
            await _repository.UnSubscribe(userId, topicId);
            return RedirectToAction("Subscribers", new { topicId });
        }
        [HttpGet]
        public async Task<IActionResult> Subscribers(Guid topicId)
        {
            var model = new SubscribersViewModel();
            LogStart(_logger, controllerName, "Subscribers");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Communication);
            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Index");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            try
            {
                model.TheList = await _repository.GetSubscriptions(topicId);
                model.Users = await _userAdminRepository.BackOfficeUsers();
                
                model.Topics = await _repository.Topics();
            }
            catch (Exception ex)
            {
                model.TheList = new List<Subscription>();
                model.Topics = new List<LookupValue>();
                _logger.LogError(ex, "Index");
                return RedirectToAction("Error", "Home", new { area = "" });
            }
            model.CurrentPage = "Newsletter";
            model.CurrentOperation = "Subscribers";
            model.ChangeActive(MenuItemEnum.Newsletter);
            model.TopicId = topicId;
            model.RoleLevel = roleLevel;
            LogEnd(_logger, controllerName, "Subscribers");

            return View(model);
        }
    }
}
