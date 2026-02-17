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
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Notification;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Communication.Controllers
{
    [Area("Communication")]
    public class MediaAgencyController : BaseController
    {
        private readonly ILogger<MediaAgencyController> _logger;
        readonly MediaAgencyRepository _repository;
        public MediaAgencyController(ILogger<MediaAgencyController> logger,
            UserManager<User> userManager,
            MediaAgencyRepository repository,
            PublicUserRepository userRepository
            , NotificationRepository notificationRepository) : base(userManager, userRepository, notificationRepository)
        {

            _logger = logger;
            _repository = repository;

            controllerName = "MediaAgency";

        }

        public async Task<IActionResult> Index()
        {
            var model = new MediaAgencyViewModel();
            LogStart(_logger, controllerName, "Index");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Communication);
            if(roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Index");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            try
            {
                model.TheList = await _repository.MediaAgencies();
            }
            catch (Exception ex)
            {
                model.TheList = new List<MediaAgency>();
                _logger.LogError(ex, "Media Agency Index Exception");
                return RedirectToAction("Error", "Home", new { area = "" });
            }
            model.CurrentPage = "Media Agency";
            model.CurrentOperation = "Index";
            model.ChangeActive(MenuItemEnum.CommunicationMediaAgency);
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
            var model = new MediaAgencyViewModel();
            try
            {
                model.TheModel = await _repository.MediaAgencyById(id,TheUser.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Media Agency Detail Exception {id}");
            }
            if (model.TheModel == null)
            {
                return RedirectToAction("Error", "Home", new { area = "" });
            }
            model.CurrentPage = "Media Agency";
            model.CurrentOperation = "Detail";

            model.ChangeActive(MenuItemEnum.CommunicationMediaAgency);
            model.RoleLevel = roleLevel;
            LogEnd(_logger, controllerName, "Detail");
            return View(model);
        }

      

        [HttpPost]
        public async Task<IActionResult> Save(MediaAgency model)
        {
            LogStart(_logger, controllerName, "Save");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Communication);
            if(roleLevel <= RoleLevelEnum.User)
            {
                LogSecurityError(_logger, controllerName, "Save");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("Index");
            }
            try
            {
               // model.MediaTypeFromBool();
                await _repository.Save(model, TheUser.Id);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch(InvalidOperationException ex)
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
    }
}
