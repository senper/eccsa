using ECCSA.BackOffice.Server.Model.Advocacy;
using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Advocacy;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Notification;
using ECCSA.BackOffice.Server.Persistence.Repositories.Security;
using ECCSA.BackOffice.Server.WebApp.Areas.Advocacy.Model;
using ECCSA.BackOffice.Server.WebApp.Controllers;
using ECCSA.BackOffice.Server.WebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Advocacy.Controllers
{
    [Area("Advocacy")]
    public class PublicationController : BaseController
    {
        private readonly ILogger<PublicationController> _logger;
        private readonly AdvocacyRepository _repository;
        
        public PublicationController(
            ILogger<PublicationController> logger,
            UserManager<User> userManager,
            PublicUserRepository userRepository,
            AdvocacyRepository repository, NotificationRepository notificationRepository) : base(userManager,userRepository, notificationRepository)
        {

            _logger = logger;
            _repository = repository;

            controllerName = "Publication";

        }

        public async Task<IActionResult> Index()
        {
            var model = new PublicationViewModel();
            LogStart(_logger, controllerName, "Index");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Research);

            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Index");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            try
            {
                model.TheList = await _repository.Publications();
            }
            catch (Exception ex)
            {
                model.TheList = new List<Publication>();
                _logger.LogError(ex, "Publication Index Exception");
            }
            model.CurrentPage = "Publication";
            model.CurrentOperation = "Index";
            model.ChangeActive(MenuItemEnum.AdvocacyPublication);
            model.RoleLevel = roleLevel;
            LogEnd(_logger, controllerName, "Index");
            return View(model);
        }

        public async Task<IActionResult> Detail(Guid id)
        {
            LogStart(_logger, controllerName, "Detail");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Research);

            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Detail");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            var model = new PublicationViewModel();
            try
            {
                model.TheModel = await _repository.PublicationById(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Publication Detail Exception {id}");
            }
            // model.TheModel.MediaType2Bool();
            model.CurrentPage = "Publication";
            model.CurrentOperation = "Detail";

            model.ChangeActive(MenuItemEnum.AdvocacyPublication);
            model.RoleLevel = roleLevel;
            LogEnd(_logger, controllerName, "Detail");
            if (model.TheModel == null)
            {
                return RedirectToAction("Error", "Home", new { area = "" });
            }
            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> Save(Publication model)
        {
            LogStart(_logger, controllerName, "Save");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Research);

            if (roleLevel < RoleLevelEnum.User)
            {
                LogSecurityError(_logger, controllerName, "Save");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
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

            return RedirectToAction("Index");
        }
    }

}
