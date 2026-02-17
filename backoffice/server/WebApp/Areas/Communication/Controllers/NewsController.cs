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
using System.Linq;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Notification;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Communication.Controllers
{
    [Area("Communication")]
    public class NewsController : BaseController
    {
        private readonly ILogger<NewsController> _logger;
        private readonly NewsRepository _repository;
        public NewsController(
            UserManager<User> userManager
            , ILogger<NewsController> logger
            , NewsRepository repository
            , PublicUserRepository userRepository, NotificationRepository notificationRepository) : base(userManager, userRepository, notificationRepository)
        {
            _logger = logger;
            _repository = repository;
            controllerName = "News";
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = new NewsViewModel();
            LogStart(_logger, controllerName, "Index");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Communication);
            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Index");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            try
            {
                model.TheList = await _repository.GetNews();
            }
            catch (Exception ex)
            {
                model.TheList = new List<News>();
                _logger.LogError(ex, "Index");
                return RedirectToAction("Error", "Home", new { area = "" });
            }
            model.CurrentPage = "News";
            model.CurrentOperation = "Index";
            model.ChangeActive(MenuItemEnum.News);
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
            var model = new NewsViewModel();
            try
            {
                model.TheModel = await _repository.NewsById(id);
                model.ProfileImages = GetProfileAttachedImages(id);
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
            model.CurrentPage = $"News";
            model.CurrentOperation = "Detail";
            model.ChangeActive(MenuItemEnum.News);
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
        public async Task<IActionResult> Save(News model, IFormFile photo, IFormFile homePhoto)
        {
            model.OrderNo = String.IsNullOrEmpty(model.OrderNo) ? "" : model.OrderNo;
            model.Id = model.Id == Guid.Empty ? Guid.NewGuid() : model.Id;
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
                if (homePhoto != null && homePhoto.Length > 0)
                {
                    model.ExtensionHome = await SaveDocument(model.Id, homePhoto, ECCSAModuleFlag.Communication, "home");
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
        public async Task<IActionResult> RemovePhoto(Guid id)
        {
            if (id == Guid.Empty)
            {
                return RedirectToAction("Index");
            }

            LogStart(_logger, controllerName, "RemovePhoto");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Communication);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "RemovePhoto");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("Index");
            }
            try
            {
                var data = await _repository.NewsById(id);
                if (!string.IsNullOrEmpty(data.Extension))
                {
                    var fname = Guid2String(id) + data.Extension;
                    var path = Path.Combine(AbsolutePath(ECCSAModuleFlag.Communication), fname);
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                }
                data.Extension = "";
                await _repository.Save(data, TheUser.Id);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("RemovePhoto", ex, id);
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("RemovePhoto", ex, id);
                CommandStatus(CommandStatusEnum.UnkownError);
            }
            return RedirectToAction("Detail", new { id });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveHomePhoto(Guid id)
        {
            if (id == Guid.Empty)
            {
                return RedirectToAction("Index");
            }

            LogStart(_logger, controllerName, "RemovePhoto");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Communication);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "RemovePhoto");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("Index");
            }
            try
            {
                var data = await _repository.NewsById(id);
                if (!string.IsNullOrEmpty(data.ExtensionHome))
                {
                    var fname = Guid2String(id) + data.ExtensionHome;
                    var path = Path.Combine(AbsolutePath(ECCSAModuleFlag.Communication), "home", fname);
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                }
                data.ExtensionHome = "";
                await _repository.Save(data, TheUser.Id);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("RemovePhoto", ex, id);
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("RemovePhoto", ex, id);
                CommandStatus(CommandStatusEnum.UnkownError);
            }
            return RedirectToAction("Detail", new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetContent(News model)
        {
            if (model.Id == Guid.Empty)
            {
                return RedirectToAction("Index");
            }

            LogStart(_logger, controllerName, "SetContent");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Communication);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "SetContent");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("Index");
            }
            try
            {
                var data = await _repository.NewsById(model.Id);
                data.Full = model.Full;
                await _repository.Save(data, TheUser.Id);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("SetContent", ex, model);
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("SetContent", ex, model);
                CommandStatus(CommandStatusEnum.UnkownError);
            }
            return RedirectToAction("Detail", new { id = model.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetContentLocal(News model)
        {
            if (model.Id == Guid.Empty)
            {
                return RedirectToAction("Index");
            }

            LogStart(_logger, controllerName, "SetContentLocal");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Communication);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "SetContentLocal");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("Index");
            }
            try
            {
                var data = await _repository.NewsById(model.Id);
                data.FullLocal = model.FullLocal;
                await _repository.Save(data, TheUser.Id);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("SetContent", ex, model);
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("SetContent", ex, model);
                CommandStatus(CommandStatusEnum.UnkownError);
            }
            return RedirectToAction("Detail", new { id = model.Id });
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
                var res = await _repository.NewsById(id);
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
                if (!string.IsNullOrEmpty(res.ExtensionHome))
                {
                    var fname = Guid2String(id) + res.ExtensionHome;
                    var path = Path.Combine(AbsolutePath(ECCSAModuleFlag.Communication), "home", fname);
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                }
                var idstr = Guid2String(id);
                string filePath = Path.Combine(NewsProfileImagePath, idstr);
                DirectoryInfo di = new(filePath);
                if (di.Exists)
                {
                    foreach (FileInfo file in di.GetFiles())
                    {
                        file.Delete();
                    }
                    foreach (DirectoryInfo dir in di.GetDirectories())
                    {
                        dir.Delete(true);
                    }
                    di.Delete(true);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProfileImage(Guid id, IFormFile image, ECCSAModuleFlag module)
        {
            try
            {
                var idstr = Guid2String(id);
                string filePath = Path.Combine(NewsProfileImagePath, idstr);
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                await SaveFile(filePath, image.FileName, image);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (Exception ex)
            {
                TempData["CommandStatus"] = ex.Message;
            }
            return RedirectToAction("Detail", new { id, module });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveProfileImage(Guid id, string fileName, ECCSAModuleFlag module)
        {
            var idstr = Guid2String(id);
            string filePath = Path.Combine(NewsProfileImagePath, idstr);
            var splitedFileName = fileName.Split("/");
            if (splitedFileName.Length > 0)
            {
                var filteredFileName = splitedFileName[^1];
                System.IO.File.Delete(Path.Combine(filePath, filteredFileName));
            }
            CommandStatus(CommandStatusEnum.Deleted);
            return RedirectToAction("Detail", new { id, module });
        }
    }
}
