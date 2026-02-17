using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Model.Website;
using ECCSA.BackOffice.Server.Persistence.Repositories.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories.Website;
using ECCSA.BackOffice.Server.WebApp.Areas.Communication.Model;
using ECCSA.BackOffice.Server.WebApp.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Notification;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Communication.Controllers
{
    [Area("Communication")]
    public class WebsiteController : BaseController
    {
        private readonly ILogger<WebsiteController> _logger;
        private readonly WebsiteRepository _repository;
        public WebsiteController(
            UserManager<User> userManager
            , ILogger<WebsiteController> logger
            , WebsiteRepository repository
            , PublicUserRepository userRepository, NotificationRepository notificationRepository) : base(userManager, userRepository, notificationRepository)
        {
            _logger = logger;
            _repository = repository;
            controllerName = "Website";
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = new WebsiteViewModel();
            LogStart(_logger, controllerName, "Index");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Communication);
            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Index");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            try
            {
                model.TheList = await _repository.Menus();
                model.QuickLinkHeaders = await _repository.Headers();
            }
            catch (Exception ex)
            {
                model.TheList = new List<Menu>();
                _logger.LogError(ex, "Index");
                return RedirectToAction("Error", "Home", new { area = "" });
            }
            model.CurrentPage = "Menues";
            model.CurrentOperation = "Index";
            model.RoleLevel = roleLevel;
            LogEnd(_logger, controllerName, "Index");

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var model = new WebsiteViewModel();
            LogStart(_logger, controllerName, "Detail");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Communication);
            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Detail");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            try
            {
                model.TheModel = await _repository.MenuById(id);
                model.Links = await _repository.Links();
                model.QuickLinkHeaders = await _repository.Headers();
                model.Links.Insert(0, new QuickLink()
                {
                    Id = -1,
                    TitleLocal = String.Empty,
                    Title = string.Empty
                });
            }
            catch (Exception ex)
            {
                model.TheList = new List<Menu>();
                _logger.LogError(ex, "Index");
            }
            if (model.TheModel == null)
            {
                return RedirectToAction("Error", "Home", new { area = "" });
            }
            model.CurrentPage = "Menues";
            model.CurrentOperation = "Index";
            model.RoleLevel = roleLevel;
            LogEnd(_logger, controllerName, "Index");

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Translations()
        {
            var model = new WebsiteTranslationViewModel();
            LogStart(_logger, controllerName, "Translations");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Communication);
            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Index");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            try
            {
                model.TheList = await _repository.Translations();
            }
            catch (Exception ex)
            {
                model.TheList = new List<Translation>();
                _logger.LogError(ex, "Index");
            }
            model.CurrentPage = "Translations";
            model.CurrentOperation = "List";
            model.RoleLevel = roleLevel;
            LogEnd(_logger, controllerName, "Translations");

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveTranslation(Translation model)
        {

            LogStart(_logger, controllerName, "Save");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Communication);
            if (roleLevel <= RoleLevelEnum.User)
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
            return RedirectToAction("Translations");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTranslation(string key)
        {

            LogStart(_logger, controllerName, "DeleteTranslation");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Communication);
            if (roleLevel <= RoleLevelEnum.User)
            {
                LogSecurityError(_logger, controllerName, "DeleteTranslation");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("Translations");
            }
            try
            {
                await _repository.DeleteTranslation(key);
                CommandStatus(CommandStatusEnum.Deleted);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("DeleteTranslation", ex, new { key });
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("DeleteTranslation", ex, new { key });
                CommandStatus(CommandStatusEnum.UnkownError);
            }
            return RedirectToAction("Translations");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateMenu(Menu model)
        {

            LogStart(_logger, controllerName, "CreateMenu");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Communication);
            if (roleLevel <= RoleLevelEnum.User)
            {
                LogSecurityError(_logger, controllerName, "CreateMenu");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("Index");
            }
            try
            {
                await _repository.Save(model);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("CreateMenu", ex, model);
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
        public async Task<IActionResult> SubMenuSave(Menu model)
        {

            LogStart(_logger, controllerName, "SubMenuSave");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Communication);
            if (roleLevel <= RoleLevelEnum.User)
            {
                LogSecurityError(_logger, controllerName, "SubMenuSave");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("Index");
            }
            try
            {
                await _repository.Save(model);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("SubMenuSave", ex, model);
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("SubMenuSave", ex, model);
                CommandStatus(CommandStatusEnum.UnkownError);
            }
            return RedirectToAction("Detail", new { Id = model.ParentId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateMenu(Menu model)
        {

            LogStart(_logger, controllerName, "UpdateMenu");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Communication);
            if (roleLevel <= RoleLevelEnum.User)
            {
                LogSecurityError(_logger, controllerName, "UpdateMenu");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("Index");
            }
            try
            {
                await _repository.Save(model);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("UpdateMenu", ex, model);
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("UpdateMenu", ex, model);
                CommandStatus(CommandStatusEnum.UnkownError);
            }
            return RedirectToAction("Detail", new { Id = model.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMenu(int menuId, int? parentId)
        {

            LogStart(_logger, controllerName, "DeleteMenu");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Communication);
            if (roleLevel <= RoleLevelEnum.User)
            {
                LogSecurityError(_logger, controllerName, "DeleteMenu");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                if (parentId.HasValue)
                    return RedirectToAction("Detail", new { id = parentId });

                return RedirectToAction("Index");
            }
            try
            {
                await _repository.DeleteMenu(menuId);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("DeleteMenu", ex, new { menuId });
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("DeleteMenu", ex, new { menuId });
                CommandStatus(CommandStatusEnum.UnkownError);
            }
            if (parentId.HasValue)
                return RedirectToAction("Detail", new { id = parentId });
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> HeaderIndex()
        {
            var model = new QuickLinkHeaderViewModel();
            LogStart(_logger, controllerName, "HeaderIndex");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Communication);
            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "HeaderIndex");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            try
            {
                model.TheList = await _repository.Headers();
            }
            catch (Exception ex)
            {
                model.TheList = new List<QuickLinkHeader>();
                _logger.LogError(ex, "HeaderIndex");
            }
            model.CurrentPage = "Menus";
            model.CurrentOperation = "HeaderIndex";
            model.RoleLevel = roleLevel;
            LogEnd(_logger, controllerName, "HeaderIndex");

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> HeaderDetail(int id)
        {
            var model = new QuickLinkHeaderViewModel();
            LogStart(_logger, controllerName, "Detail");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Communication);
            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Detail");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            try
            {
                model.TheModel = await _repository.HeaderById(id);
                model.Menus = await _repository.ChildMenus();
            }
            catch (Exception ex)
            {
                model.TheList = new List<QuickLinkHeader>();
                _logger.LogError(ex, "Index");
            }
            model.Menus.Insert(0, new Menu { Id = -1, Title = string.Empty, TitleLocal = string.Empty });
            model.CurrentPage = "QuickLinkHeader";
            model.CurrentOperation = "HeaderDetail";
            model.RoleLevel = roleLevel;
            LogEnd(_logger, controllerName, "HeaderDetail");

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateHeader(QuickLinkHeader data)
        {
            LogStart(_logger, controllerName, "Save");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Communication);
            if (roleLevel <= RoleLevelEnum.User)
            {
                LogSecurityError(_logger, controllerName, "Save");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("HeaderIndex");
            }
            try
            {
                await _repository.Save(data);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("CreateHeader", ex, data);
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("CreateHeader", ex, data);
                CommandStatus(CommandStatusEnum.UnkownError);
            }
            return RedirectToAction("HeaderIndex");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateHeader(QuickLinkHeader data)
        {
            LogStart(_logger, controllerName, "Save");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Communication);
            if (roleLevel <= RoleLevelEnum.User)
            {
                LogSecurityError(_logger, controllerName, "Save");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("HeaderDetail", new { data.Id });
            }
            try
            {
                await _repository.Save(data);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("UpdateHeader", ex, data);
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("UpdateHeader", ex, data);
                CommandStatus(CommandStatusEnum.UnkownError);
            }
            return RedirectToAction("HeaderDetail", new { data.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteQuickLink(int menuId, int id)
        {

            LogStart(_logger, controllerName, "DeleteQuickLink");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Communication);
            if (roleLevel <= RoleLevelEnum.User)
            {
                LogSecurityError(_logger, controllerName, "DeleteQuickLink");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("HeaderDetail", new { Id = menuId });
            }
            try
            {
                await _repository.DeleteLink(menuId, id);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("DeleteQuickLink", ex, new { menuId, id });
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("DeleteQuickLink", ex, new { menuId, id });
                CommandStatus(CommandStatusEnum.UnkownError);
            }
            return RedirectToAction("HeaderDetail", new { Id = menuId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveQuickLink(QuickLink model)
        {

            LogStart(_logger, controllerName, "SaveQuickLink");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Communication);
            if (roleLevel <= RoleLevelEnum.User)
            {
                LogSecurityError(_logger, controllerName, "SaveQuickLink");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("HeaderDetail", new { Id = model.HeaderId });
            }
            try
            {
                await _repository.Save(model);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("SaveQuickLink", ex, model);
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("SaveQuickLink", ex, model);
                CommandStatus(CommandStatusEnum.UnkownError);
            }
            return RedirectToAction("HeaderDetail", new { Id = model.HeaderId });
        }

        [HttpGet]
        public async Task<IActionResult> MediaTypes()
        {
            var model = new MediaTypeViewModel();
            LogStart(_logger, controllerName, "MediaTypes");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Communication);
            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "MediaTypes");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            try
            {
                model.TheList = (await _repository.MediaTypes()).Where(m => m.Id > 0).ToList();
                model.Menus = await _repository.Menus();
                model.Menus = model.Menus.Where(m => !model.Menus.Any(c => c.ParentId == m.Id)).ToList();
                model.Links = await _repository.Links();
            }
            catch (Exception ex)
            {
                model.TheList = new List<MediaType>();
                _logger.LogError(ex, "MediaTypes");
            }
            model.Menus.Insert(0, new Menu { Id = -1, Title = string.Empty, TitleLocal = string.Empty });
            model.Links.Insert(0, new QuickLink() { Id = -1, Title = string.Empty, TitleLocal = string.Empty });
            model.CurrentPage = "Menus";
            model.CurrentOperation = "HeaderIndex";
            model.RoleLevel = roleLevel;
            LogEnd(_logger, controllerName, "MediaTypes");

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveMediaType(MediaType data)
        {
            LogStart(_logger, controllerName, "Save");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Communication);
            if (roleLevel <= RoleLevelEnum.User)
            {
                LogSecurityError(_logger, controllerName, "Save");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("MediaTypes");
            }
            try
            {
                if (data.MenuId == -1)
                {
                    data.MenuId = null;
                }
                if (data.QuickLinkId == -1)
                {
                    data.QuickLinkId = null;
                }
                await _repository.Save(data);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("CreateMediaType", ex, data);
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("CreateMediaType", ex, data);
                CommandStatus(CommandStatusEnum.UnkownError);
            }
            return RedirectToAction("MediaTypes");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMediaType(int id)
        {

            LogStart(_logger, controllerName, "DeleteMediaType");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Communication);
            if (roleLevel <= RoleLevelEnum.User)
            {
                LogSecurityError(_logger, controllerName, "DeleteMediaType");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("MediaTypes");
            }
            try
            {
                await _repository.DeleteMediaType(id);
                CommandStatus(CommandStatusEnum.Deleted);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("DeleteMediaType", ex, new { id });
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("DeleteMediaType", ex, new { id });
                CommandStatus(CommandStatusEnum.UnkownError);
            }
            return RedirectToAction("MediaTypes");
        }

        [HttpGet]
        public async Task<IActionResult> FooterMenu(int activeTabIndex = 0)
        {
            var model = new FooterMenuViewModel();
            LogStart(_logger, controllerName, "FooterMenu");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Communication);
            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "FooterMenu");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            try
            {
                model.TheModel = await _repository.FooterMenu();
            }
            catch (Exception ex)
            {
                model.TheModel = new FooterMenu();
                _logger.LogError(ex, "FooterMenu");
            }
            model.CurrentPage = "Footer Menu";
            model.CurrentOperation = "List";
            model.RoleLevel = roleLevel;
            model.ActiveTabIndex = activeTabIndex;
            LogEnd(_logger, controllerName, "FooterMenu");

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveFooterLink(FooterLink data, FooterLink Old, bool isUsefulLink)
        {
            LogStart(_logger, controllerName, "SaveFooterLink");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Communication);
            if (roleLevel <= RoleLevelEnum.User)
            {
                LogSecurityError(_logger, controllerName, "SaveFooterLink");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("FooterMenu");
            }
            try
            {
                if (isUsefulLink)
                    await _repository.SaveUsefulLink(data, Old);
                else
                    await _repository.SaveImportantLink(data, Old);

                CommandStatus(CommandStatusEnum.Updated);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("UpdateFooterContact", ex, data);
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("UpdateFooterContact", ex, data);
                CommandStatus(CommandStatusEnum.UnkownError);
            }
            return RedirectToAction("FooterMenu", new { activeTabIndex = isUsefulLink ? 0 : 1 });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveFooterContact(FooterContact data)
        {
            LogStart(_logger, controllerName, "SaveFooterContact");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Communication);
            if (roleLevel <= RoleLevelEnum.User)
            {
                LogSecurityError(_logger, controllerName, "SaveFooterContact");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("FooterMenu");
            }
            try
            {
                await _repository.UpdateFooterContact(data);
                CommandStatus(CommandStatusEnum.Updated);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("UpdateFooterContact", ex, data);
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("UpdateFooterContact", ex, data);
                CommandStatus(CommandStatusEnum.UnkownError);
            }
            return RedirectToAction("FooterMenu");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteFooterLink(FooterLink data, bool isUsefulLink)
        {

            LogStart(_logger, controllerName, "DeleteFooterLink");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Communication);
            if (roleLevel <= RoleLevelEnum.User)
            {
                LogSecurityError(_logger, controllerName, "DeleteFooterLink");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("FooterMenu");
            }
            try
            {
                if (isUsefulLink)
                    await _repository.RemoveUsefulLink(data);
                else
                    await _repository.RemoveImportantLink(data);
                CommandStatus(CommandStatusEnum.Deleted);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("DeleteFooterLink", ex, data);
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("DeleteFooterLink", ex, data);
                CommandStatus(CommandStatusEnum.UnkownError);
            }
            return RedirectToAction("FooterMenu", new { activeTabIndex = isUsefulLink ? 0 : 1 });
        }
    }
}
