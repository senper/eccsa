using ECCSA.BackOffice.Server.Model.Common;
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
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Communication.Controllers
{
    [Area("Communication")]
    public class ResourceController : BaseController
    {

        private readonly ILogger<ResourceController> _logger;
        readonly ResourceRepository _repository;
        private readonly YesNoRepository _lookupRepository;
        public ResourceController(ILogger<ResourceController> logger,
            UserManager<User> userManager,
            ResourceRepository repository,
            PublicUserRepository userRepository
        , YesNoRepository lookupRepository
            , NotificationRepository notificationRepository
            ) : base(userManager, userRepository, notificationRepository)
        {
            _logger = logger;
            _repository = repository;
            _lookupRepository = lookupRepository;
            controllerName = "Communication.Resource";
        }

        public async Task<IActionResult> Index(ECCSAModuleFlag module)
        {
            LogStart(_logger, controllerName, "Index");
            var roleLevel = await GetRoleLevel(module);
            //if(roleLevel <= RoleLevelEnum.None)
            //{
            //    LogSecurityError(_logger, controllerName, "Index");
            //    return RedirectToAction("Login", "Account", new { area = "Identity" });
            //}
            var model = new ResourceViewModel();

            try
            {
                model.TheList = await _repository.Resources(module);
                var tmp = await _lookupRepository.LookupTypeById(Persistence.Repositories.Constants
                    .ResourceTypeLookupTypeId);
                model.ResourceTypes = tmp.LookupValues.ToList();
            }
            catch (Exception ex)
            {
                model.TheList = new List<Resource>();
                _logger.LogError(ex, "Resource Index Exception");
                return RedirectToAction("Error", "Home", new { area = "" });
            }


            model.CurrentPage = "Resource";
            model.CurrentOperation = "Index";
            model.ChangeActive(MenuItemEnum.Resource);
            model.RoleLevel = roleLevel;
            model.CurrentModule = module;
            LogEnd(_logger, controllerName, "Index");
            return View(model);
        }

        public async Task<IActionResult> Detail(Guid id, ECCSAModuleFlag module)
        {
            LogStart(_logger, controllerName, "Detail");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Communication);
            //if(roleLevel <= RoleLevelEnum.None)
            //{
            //    LogSecurityError(_logger, controllerName, "Detail");
            //    return RedirectToAction("Login", "Account", new { area = "Identity" });
            //}
            var model = new ResourceViewModel();
            try
            {
                model.TheModel = await _repository.ModuleById(id, module);
                model.CurrentModule = model.TheModel.OwnerModule;
                model.ProfileImages = GetProfileAttachedImages(id);
                var tmp = await _lookupRepository.LookupTypeById(Persistence.Repositories.Constants
                    .ResourceTypeLookupTypeId);
                model.ResourceTypes = tmp.LookupValues.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Resource Detail Exception {id}");
            }
            if (model.TheModel == null)
            {
                return RedirectToAction("Error", "Home", new { area = "" });
            }
            model.CurrentPage = "Resource";
            model.CurrentOperation = "Detail";
            model.ChangeActive(MenuItemEnum.Resource);
            model.RoleLevel = roleLevel;
            LogEnd(_logger, controllerName, "Index");
            return View(model);
        }

        private async Task<String> SaveDocument(Guid id, IFormFile file, ECCSAModuleFlag module)
        {
            var absolutePath = AbsolutePath(module);
            var shortName = Guid2String(id);
            string fileName = file.FileName;
            string extension = Path.GetExtension(Path.GetFileName(fileName));
            var newFileName = String.Concat(shortName, extension);
            await SaveFile(absolutePath, newFileName, file, shortName);

            return extension;
        }

        [HttpPost]
        public async Task<IActionResult> Save(Resource model, IFormFile upload)
        {
            model.Id = model.Id == Guid.Empty ? Guid.NewGuid() : model.Id;
            LogStart(_logger, controllerName, "Save");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Communication);
            //if(roleLevel <= RoleLevelEnum.View)
            //{
            //    LogSecurityError(_logger, controllerName, "Save");
            //    CommandStatus(CommandStatusEnum.InsufficientPrevilage);
            //    return RedirectToAction("Index");
            //}
            try
            {
                if (upload != null && upload.Length > 0)
                {
                    model.Extension = await SaveDocument(model.Id, upload, model.OwnerModule);
                }
                model.Url = String.IsNullOrEmpty(model.Url) ? "" : model.Url;
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
            if (model.Id == Guid.Empty)
                return RedirectToAction("Index", new { module = model.OwnerModule });
            else
                return RedirectToAction("Detail", new { id = model.Id, module = model.OwnerModule });
        }
        [HttpPost]
        public async Task<IActionResult> Delete(Guid id, ECCSAModuleFlag module)
        {
            LogStart(_logger, controllerName, "Delete");
            var roleLevel = await GetRoleLevel(module);
            //if (roleLevel <= RoleLevelEnum.View)
            //{
            //    LogSecurityError(_logger, controllerName, "Delete");
            //    CommandStatus(CommandStatusEnum.InsufficientPrevilage);
            //    return RedirectToAction("Index");
            //}
            try
            {
                var res = await _repository.ById(id);
                await _repository.Delete(id);
                if (!string.IsNullOrEmpty(res.Extension))
                {
                    var fname = Guid2String(id) + res.Extension;
                    var path = Path.Combine(AbsolutePath(res.OwnerModule), fname);
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                }
                var idstr = Guid2String(id);
                string filePath = Path.Combine(ResourceProfileImagePath, idstr);
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
                CommandStatus(CommandStatusEnum.Saved);
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

            return RedirectToAction("Index", new { module });
        }
        [HttpGet]
        public async Task<FileResult> DownLoadFile(Guid id, ECCSAModuleFlag type, string fileName)
        {
            var fname = Guid2String(id) + Path.GetExtension(fileName);
            return await DownLoadFile(Path.Combine(AbsolutePath(type), fname), fileName);
        }
        [HttpPost]
        public async Task<IActionResult> SetProfile(Guid id, String profile, ECCSAModuleFlag module)
        {
            var idstr = Guid2String(id);
            _ = Path.Combine(MembershipPhotoAbsolutePath, idstr);
            //var listofImageTags = profile.Where(s => )
            //get img 

            //check if img scr is d/f from filepath

            //if d/f remove img
            _ = await _repository.UpdateProfile(id, profile, TheUser.Id);
            CommandStatus(CommandStatusEnum.Saved);
            return RedirectToAction("Detail", new { id, module });
        }
        [HttpPost]
        public async Task<IActionResult> SetLocalProfile(Guid id, String profile, ECCSAModuleFlag module)
        {
            var idstr = Guid2String(id);
            _ = Path.Combine(MembershipPhotoAbsolutePath, idstr);
            //var listofImageTags = profile.Where(s => )
            //get img 

            //check if img scr is d/f from filepath

            //if d/f remove img
            _ = await _repository.UpdateLocalProfile(id, profile, TheUser.Id);
            CommandStatus(CommandStatusEnum.Saved);
            return RedirectToAction("Detail", new { id, module });
        }

        private List<string> GetProfileAttachedImages(Guid resourceId)
        {
            var id = Guid2String(resourceId);
            string filePath = Path.Combine(ResourceProfileImagePath, id);
            if (Directory.Exists(filePath))
            {
                var lst = Directory.GetFiles(filePath).Select(s => $"{ResourceProfileRelativePath}{id}/" +
                (s.Split($"{id}\\").Length > 1 ? s.Split($"{id}\\")[1] :
                (s.Split($"{id}/").Length > 1 ? s.Split($"{id}/")[1] : "")
                )).ToList();

                return lst;
            }
            return new List<string>();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProfileImage(Guid id, IFormFile image, ECCSAModuleFlag module)
        {
            try
            {
                var idstr = Guid2String(id);
                string filePath = Path.Combine(ResourceProfileImagePath, idstr);
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
            string filePath = Path.Combine(ResourceProfileImagePath, idstr);
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
