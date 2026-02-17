using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Model.Website;
using ECCSA.BackOffice.Server.Persistence.Repositories.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories.Website;
using ECCSA.BackOffice.Server.WebApp.Areas.Communication.Model;
using ECCSA.BackOffice.Server.WebApp.Controllers;
using ECCSA.BackOffice.Server.WebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Notification;
using Microsoft.AspNetCore.Authorization;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Communication.Controllers
{
    [Area("Communication")]
    [Authorize()]
    public class LawsController : BaseController
    {
        private readonly ILogger<LawsController> _logger;
        private readonly LawsRepository _repository;
        public LawsController(
            UserManager<User> userManager
            , ILogger<LawsController> logger
            , LawsRepository repository
            , PublicUserRepository userRepository, NotificationRepository notificationRepository) : base(userManager, userRepository, notificationRepository)
        {
            _logger = logger;
            _repository = repository;
            controllerName = "Laws";
        }
        public async Task<IActionResult> Index()
        {
            var model = new LawsViewModel();
            LogStart(_logger, controllerName, "Index");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Communication);
            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Index");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            try
            {
                model.TheList = await _repository.List();
            }
            catch (Exception ex)
            {
                model.TheList = new List<Law>();
                _logger.LogError(ex, "Index");
                return RedirectToAction("Error", "Home", new { area = "" });
            }
            model.CurrentPage = "Laws";
            model.CurrentOperation = "Index";
            model.ChangeActive(MenuItemEnum.CommunicationLaws);
            model.RoleLevel = roleLevel;
            LogEnd(_logger, controllerName, "Index");

            return View(model);
        }

        [HttpGet]
        public async Task<FileResult> DownLoadFile(Guid id,  string fileName)
        {
            var fname = Guid2String(id) + Path.GetExtension(fileName);
            return await DownLoadFile(Path.Combine(AbsolutePath(ECCSAModuleFlag.Communication), fname), fileName);
        }
        [HttpGet]
        public async Task<FileResult> DownLoadFileLocal(Guid id, string fileName)
        {
            var fname = GetLocalFileName( Guid2String(id)) + Path.GetExtension(fileName);
            return await DownLoadFile(Path.Combine(AbsolutePath(ECCSAModuleFlag.Communication), fname), fileName);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
      
        public async Task<IActionResult> Save(Law model, IFormFile upload, IFormFile uploadLocal)
        {
            model.Id = model.Id == Guid.Empty ? Guid.NewGuid() : model.Id;
            LogStart(_logger, controllerName, "Save");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Communication);
            if(upload == null || upload.Length == 0)
            {
                CommandStatus("upload file missing");
                return RedirectToAction("Index");
            }
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "Save");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("Index");
            }
            try
            {
                if (upload != null && upload.Length > 0)
                {
                    model.Extension = await SaveDocumentExtension(model.Id, ECCSAModuleFlag.Communication, upload);
                }
                if (uploadLocal != null && uploadLocal.Length > 0)
                {
                    model.ExtensionLocal = await SaveDocumentExtension(model.Id, ECCSAModuleFlag.Communication, uploadLocal, true);
                }
                if(model.IsValid())
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
                var res = await _repository.ById(id);
                await _repository.Delete(id);
                if (!string.IsNullOrEmpty(res.Extension))
                {
                    var fname = Guid2String(id) + res.Extension;
                    var path = Path.Combine(AbsolutePath(ECCSAModuleFlag.Communication), fname);
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                    
                }
                await _repository.Delete(id);
                if (!string.IsNullOrEmpty(res.ExtensionLocal))
                {
                    var fnameLocal = GetLocalFileName(Guid2String(id)) + res.ExtensionLocal;
                    var pathLocal = Path.Combine(AbsolutePath(ECCSAModuleFlag.Communication), fnameLocal);
                    if (System.IO.File.Exists(pathLocal))
                    {
                        System.IO.File.Delete(pathLocal);
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
    }
}

