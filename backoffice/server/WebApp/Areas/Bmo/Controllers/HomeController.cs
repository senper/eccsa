using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Model.Website;
using ECCSA.BackOffice.Server.Persistence.Repositories.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories.Website;
using ECCSA.BackOffice.Server.WebApp.Areas.Bmo.Model;
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

namespace ECCSA.BackOffice.Server.WebApp.Areas.Bmo.Controllers
{
    [Area("Bmo")]
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MediaRepository _repository;
      
        public HomeController(ILogger<HomeController> logger,
            UserManager<User> userManager,
         MediaRepository repository,
        
        PublicUserRepository userRepository
            , NotificationRepository notificationRepository) : base(userManager, userRepository, notificationRepository)
        {

            _logger = logger;
            _repository = repository;
            controllerName = "Home";
        }

        public async Task<IActionResult> Index()
        {
            var model = new BmoViewModel();
            LogStart(_logger, controllerName, "Index");
            var roleLevel = await GetRoleLevel(model.CurrentModule);
            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Index");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            try
            {
                model.TheList = (await _repository.List()).Where(m=> m.MediaType.MediaAuthor == ECCSAModuleEnum.Bmo).ToList();
                model.MediaTypes = (await _repository.MediaTypes()).Where(m => m.MediaAuthor == ECCSAModuleEnum.Bmo).OrderBy(m => m.GroupName).ThenBy(m => m.Name).ToList();
            }
            catch (Exception ex)
            {
                model.TheList = new List<OtherMedia>();
                _logger.LogError(ex, "Index");
            }
            
            model.CurrentOperation = "Index";
            model.RoleLevel = roleLevel;
            LogEnd(_logger, controllerName, "Index");

            return View(model);
        }


        public async Task<IActionResult> Detail(Guid id)
        {
            LogStart(_logger, controllerName, "Detail");
            var model = new BmoViewModel();
            var roleLevel = await GetRoleLevel(model.CurrentModule);
            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Detail");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
                
            }
           
            try
            {
                model.TheModel = await _repository.ById(id);
                model.ProfileImages = GetProfileAttachedImages(id);
                model.MediaTypes = (await _repository.MediaTypes()).Where(m => m.MediaAuthor == ECCSAModuleEnum.Bmo).OrderBy(m => m.GroupName).ThenBy(m => m.Name).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $" Detail Exception {id}");
            }

            model.CurrentOperation = "Detail";
            model.ChangeActive(MenuItemEnum.WebsiteContent);
            model.RoleLevel = roleLevel;
            LogEnd(_logger, controllerName, "Detail");
            if (model.TheModel == null)
            {
               
                return RedirectToAction("Error", "Home", new { area = "" });
            }
            return View(model);
        }
        private List<string> GetProfileAttachedImages(Guid associationId)
        {
            var id = Guid2String(associationId);
            string filePath = Path.Combine(TextMediaProfileImagePath, id);
            if (Directory.Exists(filePath))
            {
                var lst = Directory.GetFiles(filePath).Select(s => $"{TextMediaProfileRelativePath}{id}/" +
                (s.Split($"{id}\\").Length > 1 ? s.Split($"{id}\\")[1] :
                (s.Split($"{id}/").Length > 1 ? s.Split($"{id}/")[1] : "")
                )).ToList();

                return lst;
            }
            return new List<string>();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(OtherMedia model)
        {
            var isNew = model.Id == Guid.Empty;

            model.Id = model.Id == Guid.Empty ? Guid.NewGuid() : model.Id;
            LogStart(_logger, controllerName, "Save");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Bmo);
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
                if (isNew)
                    return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError("Save", ex, model);
                CommandStatus(CommandStatusEnum.UnkownError);
                if (isNew)
                    return RedirectToAction("Index");
            }

            return RedirectToAction("Detail", new { id = model.Id });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetContent(OtherMedia model)
        {
            if (model.Id == Guid.Empty)
            {
                return RedirectToAction("Index");
            }

            LogStart(_logger, controllerName, "Save");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Bmo);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "Save");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("Index");
            }
            try
            {
                var data = await _repository.ById(model.Id);
                data.Content = model.Content;
                await _repository.Save(data, TheUser.Id);
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
            return RedirectToAction("Detail", new { id = model.Id });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetContentLocal(OtherMedia model)
        {
            if (model.Id == Guid.Empty)
            {
                return RedirectToAction("Index");
            }

            LogStart(_logger, controllerName, "Save");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Bmo);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "Save");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("Index");
            }
            try
            {
                var data = await _repository.ById(model.Id);
                data.ContentLocal = model.ContentLocal;
                await _repository.Save(data, TheUser.Id);
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
            return RedirectToAction("Detail", new { id = model.Id });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveComment(string commentId, Guid blogId)
        {
            LogStart(_logger, controllerName, "Save");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Bmo);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "Save");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("Detail", new { id = blogId });
            }
            try
            {
                await _repository.RemoveComment(blogId, commentId);
                CommandStatus(CommandStatusEnum.Deleted);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("Save", ex, new { blogId, commentId });
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Save", ex, new { blogId, commentId });
                CommandStatus(CommandStatusEnum.UnkownError);
            }
            return RedirectToAction("Detail", new { id = blogId });
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            LogStart(_logger, controllerName, "Delete");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Bmo);
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
                var idstr = Guid2String(id);
                string filePath = Path.Combine(TextMediaProfileImagePath, idstr);
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
                string filePath = Path.Combine(TextMediaProfileImagePath, idstr);
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                await SaveFile(filePath, image.FileName, image);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch(Exception ex)
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
            string filePath = Path.Combine(TextMediaProfileImagePath, idstr);
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
