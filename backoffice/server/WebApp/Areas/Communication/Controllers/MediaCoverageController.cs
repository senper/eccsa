using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Communication;
using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories.Communication;
using ECCSA.BackOffice.Server.Persistence.Repositories.Security;
using ECCSA.BackOffice.Server.WebApp.Areas.Communication.Model;
using ECCSA.BackOffice.Server.WebApp.Controllers;
using ECCSA.BackOffice.Server.WebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Notification;
using Microsoft.AspNetCore.Http;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Communication.Controllers
{
    [Area("Communication")]
    public class MediaCoverageController : BaseController
    {

        private readonly ILogger<MediaCoverageController> _logger;
        readonly MediaAgencyRepository _repository;
        public MediaCoverageController(ILogger<MediaCoverageController> logger,
            UserManager<User> userManager,
            MediaAgencyRepository repository,
            PublicUserRepository userRepository
            , NotificationRepository notificationRepository) : base(userManager, userRepository, notificationRepository)
        {

            _logger = logger;
            _repository = repository;

            controllerName = "MediaCoverage";

        }

        public async Task<IActionResult> Index()
        {
            var model = new MediaCoverageViewModel();
            LogStart(_logger, controllerName, "Index");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Communication);
            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Index");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            try
            {
                model.Agencies = await _repository.MediaAgencies();
                model.TheList = await _repository.MediaCoverages();
            }
            catch (Exception ex)
            {
                model.TheList = new List<MediaCoverage>();
                _logger.LogError(ex, "Media Coverage Index Exception");
                return RedirectToAction("Error", "Home", new { area = "" });
            }
            model.CurrentPage = "Media Coverage";
            model.CurrentOperation = "Index";
            model.ChangeActive(MenuItemEnum.CommunicationMediaCoverage);
            model.RoleLevel = roleLevel;
            LogEnd(_logger, controllerName, "Index");
            return View(model);
        }

        public async Task<IActionResult> Detail(Guid id)
        {
            LogStart(_logger, controllerName, "Index");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Communication);
            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Detail");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            var model = new MediaCoverageViewModel();
            try
            {
                model.Agencies = await _repository.MediaAgencies();
                model.TheModel = await _repository.MediaCoverageById(id);
                model.AttachmentPath = GetAttachment(id, model.TheModel.Extension);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Media Coverage Detail Exception {id}");
            }
            if (model.TheModel == null)
            {
                return RedirectToAction("Error", "Home", new { area = "" });
            }
            model.CurrentPage = "Media Coverage";
            model.CurrentOperation = "Detail";
            model.ChangeActive(MenuItemEnum.CommunicationMediaCoverage);
            model.RoleLevel = roleLevel;
            LogEnd(_logger, controllerName, "Index");
            return View(model);
        }

        private string  GetAttachment(Guid mediaId, string extension)
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
            var model = await _repository.MediaCoverageById(id);
            
            fileName += model.Extension;
            var fullPath = $"{filePath}\\{fileName}";
           var  title = $"Media Coverage by {model.MediaAgency.FullName} during {model.StartDate.ToString("yyyyMMddhh")}-{model.EndDate.ToString("yyyyMMddhh")}on{DateTime.Now.ToString("yyyyMMdd")}-{DateTime.Now.ToString("hhmmss")}{model.Extension}";
           return await DownLoadFile(filePath, fileName,title);
        }
        private async Task<String> SaveDocument(Guid id, IFormFile file,  string subfolder = "")
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
        public async Task<IActionResult> Save(MediaCoverage model,  IFormFile material)
        {
            LogStart(_logger, controllerName, "Save");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Communication);
            var oldId = model.Id;
           model.Id = model.Id == Guid.Empty ? Guid.NewGuid() : model.Id;
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "Save");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("Index");
            }
            try
            {
                if (material is { Length: > 0 })
                {
                    model.Extension = await SaveDocument(model.Id, material);
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
            if (oldId == Guid.Empty)
                return RedirectToAction("Index");
            return RedirectToAction("Detail", new { id = model.Id });
        }







    }
}
