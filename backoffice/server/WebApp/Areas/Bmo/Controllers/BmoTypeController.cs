using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Website;
using ECCSA.BackOffice.Server.WebApp.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Identity;
using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories.Website;
using ECCSA.BackOffice.Server.WebApp.Areas.Communication.Controllers;
using Microsoft.Extensions.Logging;
using ECCSA.BackOffice.Server.WebApp.Areas.Bmo.Model;
using System.Linq;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Notification;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Bmo.Controllers
{
    [Area("Bmo")]
    public class BmoTypeController : BaseController
    {
        private readonly ILogger<WebsiteController> _logger;
        private readonly BmoRepository _repository;

        public BmoTypeController(UserManager<User> userManager
             , ILogger<WebsiteController> logger
             , BmoRepository repository
             , PublicUserRepository userRepository, NotificationRepository notificationRepository) : base(userManager, userRepository, notificationRepository)
        {
            _logger = logger;
            _repository = repository;
            controllerName = "BmoType";
        }

       

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(MediaType data)
        {
            LogStart(_logger, controllerName, "Save");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Bmo);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "Save");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("Index");
            }
            if(data.Id < 0 && string.IsNullOrEmpty(data.NameLocal))
            {
                CommandStatus("Local BMO Name is required");
                return RedirectToAction("Index");
            }
            try
            {
               
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
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {

            LogStart(_logger, controllerName, "Delete");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Bmo);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "Delete");
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
                _logger.LogError("Delete", ex, new { id });
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Delete", ex, new { id });
                CommandStatus(CommandStatusEnum.UnkownError);
            }
            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = new BmoTypeViewModel();
            LogStart(_logger, controllerName, "BmoTypes");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Bmo);
            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "BmoTypes");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            try
            {
                model.TheList = (await _repository.BmoTypes()).Where(m => m.Id > 0).ToList();
            }
            catch (Exception ex)
            {
                model.TheList = new List<MediaType>();
                _logger.LogError(ex, "BmoTypes");
            }
            model.CurrentPage = "BmoTypes";
            model.CurrentOperation = "Index";
            model.RoleLevel = roleLevel;
            LogEnd(_logger, controllerName, "Index");

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var model = new BmoTypeViewModel();
            LogStart(_logger, controllerName, "Detail");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Bmo);
            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Detail");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            try
            {
                model.Contents = (await _repository.GetContentsByType(id));
                model.TheList = (await _repository.BmoTypes()).Where(m => m.Id > 0).ToList();
                model.TheModel = model.TheList.FirstOrDefault(m=> m.Id == id);
            }
            catch (Exception ex)
            {
                model.TheList = new List<MediaType>();
                _logger.LogError(ex, "BmoTypes");
            }
            model.CurrentPage = "Detail";
            model.CurrentOperation = "Detail";
            model.RoleLevel = roleLevel;
            LogEnd(_logger, controllerName, "Detail");

            return View(model);
        }
    }
}
