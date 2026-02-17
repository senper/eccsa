using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Investment;
using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories.Investment;
using ECCSA.BackOffice.Server.Persistence.Repositories.Security;
using ECCSA.BackOffice.Server.WebApp.Areas.Investment.Model;
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

namespace ECCSA.BackOffice.Server.WebApp.Areas.BusinessDirectory.Controllers
{
    [Area("Investment")]
    public class ProfileController : BaseController
    {
        private readonly ILogger<ProfileController> _logger;
        private readonly InvestmentRepository _repository;
        public ProfileController(
            UserManager<User> userManager
            , ILogger<ProfileController> logger
            , InvestmentRepository repository
            , PublicUserRepository userRepository
            , NotificationRepository notificationRepository) : base(userManager, userRepository, notificationRepository)
        {
            _logger = logger;
            _repository = repository;
            controllerName = "Profile";
        }

        public async Task<IActionResult> Index(Guid id)
        {
            var model = new ProfileViewModel();
            LogStart(_logger, controllerName, "Index");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Investment);
            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Index");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            try
            {
                model.TheList = await _repository.RegionSectors();


            }
            catch (Exception ex)
            {
                model.TheList = new List<RegionSector>();
                _logger.LogError(ex, "Profile Exception");
                return RedirectToAction("Error", "Home", new { area = "" });
            }
            model.Regions = model.TheList.Where(i => i.IsRegion && i.Id != RegionSector.EthiopiaId).ToList();
            model.Sectors = model.TheList.Where(i => !i.IsRegion && i.Id != RegionSector.EthiopiaId).ToList();
            model.Ethiopia = model.TheList.FirstOrDefault(i => i.Id == RegionSector.EthiopiaId);
            model.CurrentPage = "Regional | Sectoral Profile";
            model.CurrentOperation = "Index";
            model.ChangeActive(MenuItemEnum.RegionalProfile);
            model.RoleLevel = roleLevel;
            LogEnd(_logger, controllerName, "Index");
            return View(model);
        }

        public async Task<IActionResult> Detail(Guid id)
        {
            LogStart(_logger, controllerName, "Detail");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Investment);
            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Detail");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            var model = new ProfileViewModel();
            if (id == Guid.Empty)
                return RedirectToAction("Index");

            try
            {
                model.TheModel = await _repository.RegionSectorById(id);
                model.ProfileImages = GetProfileImages(id);
                model.ProfileAttachments = GetProfileAttachments(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Profile Exception {id}");
            }

            if (model.TheModel == null)
            {
                return RedirectToAction("Error", "Home", new { area = "" });
            }
            model.CurrentPage = "Profile";
            model.CurrentOperation = "Detail";
            model.ChangeActive(MenuItemEnum.RegionalProfile);
            LogEnd(_logger, controllerName, "Detail");
            return View(model);
        }

        private List<string> GetProfileImages(Guid profileId)
        {
            var id = Guid2String(profileId);
            string filePath = Path.Combine(ProfileImagePath, id);
            if (Directory.Exists(filePath))
            {
                var lst = Directory.GetFiles(filePath).Select(s => $"{ProfileRelativePath}{id}/" +
                (s.Split($"{id}\\").Length > 1 ? s.Split($"{id}\\")[1] :
                (s.Split($"{id}/").Length > 1 ? s.Split($"{id}/")[1] : "")
                )).ToList();

                return lst;
            }
            return new List<string>();
        }

        private List<string> GetProfileAttachments(Guid profileId)
        {
            var id = Guid2String(profileId);
            string filePath = Path.Combine(ProfileAttachmentPath, id);
            if (Directory.Exists(filePath))
            {
                var lst = Directory.GetFiles(filePath).Select(s => $"{ProfileRelativeAttachmentPath}{id}/" +
                (s.Split($"{id}\\").Length > 1 ? s.Split($"{id}\\")[1] :
                (s.Split($"{id}/").Length > 1 ? s.Split($"{id}/")[1] : "")
                )).ToList();

                return lst;
            }
            return new List<string>();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(RegionSector model)
        {
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Investment);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "SaveProfile");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                if (model.Id == Guid.Empty)
                    return RedirectToAction("Index");
                return RedirectToAction("Detail", new { id = model.Id });
            }

            try
            {
                await _repository.Save(model, TheUser.Id);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (Exception ex)
            {
                _logger.LogError("SaveProfile", ex, model);
                CommandStatus(CommandStatusEnum.UnkownError);
            }

            return RedirectToAction("Detail", new { id = model.Id });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveProfile(RegionSector model, IFormFile logo)
        {
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Investment);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "SaveProfile");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                if (model.Id == Guid.Empty)
                    return RedirectToAction("Index");
                return RedirectToAction("Detail", new { id = model.Id });
            }

            try
            {
                await _repository.SaveProfile(model, TheUser.Id);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (Exception ex)
            {
                _logger.LogError("SaveProfile", ex, model);
                CommandStatus(CommandStatusEnum.UnkownError);
            }

            return RedirectToAction("Detail", new { id = model.Id });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveProfileLocal(RegionSector model, IFormFile logo)
        {
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Investment);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "SaveProfileLocal");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                if (model.Id == Guid.Empty)
                    return RedirectToAction("Index");
                return RedirectToAction("Detail", new { id = model.Id });
            }

            try
            {
                await _repository.SaveProfileLocal(model, TheUser.Id);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (Exception ex)
            {
                _logger.LogError("SaveProfileLocal", ex, model);
                CommandStatus(CommandStatusEnum.UnkownError);
            }

            return RedirectToAction("Detail", new { id = model.Id });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveProfileName(RegionSector model)
        {
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Investment);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "SaveProfile");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                if (model.Id == Guid.Empty)
                    return RedirectToAction("Index");
                return RedirectToAction("Detail", new { id = model.Id });
            }

            try
            {
                var m = await _repository.RegionSectorById(model.Id);
                m.Name = model.Name;
                m.NameLocal = model.NameLocal;
                await _repository.Save(m, TheUser.Id);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (Exception ex)
            {
                _logger.LogError("SaveProfile", ex, model);
                CommandStatus(CommandStatusEnum.UnkownError);
            }

            return RedirectToAction("Detail", new { id = model.Id });

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProfile(Guid id)
        {
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Investment);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "DeleteProfile");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("Detail", new { id = id });
            }
            try
            {
                await _repository.Delete(id, TheUser.Id);
                CommandStatus(CommandStatusEnum.Deleted);
            }
            catch (Exception ex)
            {
                _logger.LogError("DeleteProfile", ex, id);
                CommandStatus(CommandStatusEnum.UnkownError);
            }

            return RedirectToAction("Index");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProfileImage(Guid id, IFormFile image)
        {
            try
            {
                var idstr = Guid2String(id);
                string filePath = Path.Combine(ProfileImagePath, idstr);
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
            return RedirectToAction("Detail", new { id, activeTabIndex = 3 });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveProfileImage(Guid id, string fileName)
        {
            var idstr = Guid2String(id);
            string filePath = Path.Combine(ProfileImagePath, idstr);
            var splitedFileName = fileName.Split("/");
            if (splitedFileName.Length > 0)
            {
                var filteredFileName = splitedFileName[^1];
                System.IO.File.Delete(Path.Combine(filePath, filteredFileName));
            }
            CommandStatus(CommandStatusEnum.Deleted);
            return RedirectToAction("Detail", new { id, activeTabIndex = 3 });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProfileAttachment(Guid id, IFormFile file)
        {
            try
            {
                var idstr = Guid2String(id);
                string filePath = Path.Combine(ProfileAttachmentPath, idstr);
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                await SaveFile(filePath, file.FileName, file);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (Exception ex)
            {
                TempData["CommandStatus"] = ex.Message;
            }
            return RedirectToAction("Detail", new { id, activeTabIndex = 4 });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveProfileAttachment(Guid id, string fileName)
        {
            var idstr = Guid2String(id);
            string filePath = Path.Combine(ProfileAttachmentPath, idstr);
            var splitedFileName = fileName.Split("/");
            if (splitedFileName.Length > 0)
            {
                var filteredFileName = splitedFileName[^1];
                System.IO.File.Delete(Path.Combine(filePath, filteredFileName));
            }
            CommandStatus(CommandStatusEnum.Deleted);
            return RedirectToAction("Detail", new { id, activeTabIndex = 4 });
        }
    }
}
