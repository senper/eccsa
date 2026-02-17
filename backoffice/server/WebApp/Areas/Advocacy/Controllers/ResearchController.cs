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
    public class ResearchController : BaseController
    {
        private ILogger<ResearchController> _logger;
        AdvocacyRepository _repository;
        
        public ResearchController(ILogger<ResearchController> logger,
            UserManager<User> userManager,
            PublicUserRepository userRepository,
            AdvocacyRepository repository, NotificationRepository notificationRepository) : base(userManager, userRepository, notificationRepository)
        {

            _logger = logger;
            _repository = repository;

            controllerName = "Research";

        }

        public async Task<IActionResult> Index()
        {
            var model = new ResearchViewModel();
            LogStart(_logger, controllerName, "Index");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Research);

            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Index");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            try
            {
                model.TheList = await _repository.Researches();
            }
            catch (Exception ex)
            {
                model.TheList = new List<Research>();
                LogError(_logger, ex, controllerName, "Index", null);
            }
            model.RoleLevel = roleLevel;
            model.CurrentPage = "Research";
            model.CurrentOperation = "Index";
            
            model.ChangeActive(MenuItemEnum.AdvocacyResearch);
            LogEnd(_logger, controllerName, "Index");
            return View(model);
        }

        public async Task<IActionResult> Detail(Guid id)
        {
            LogStart(_logger, controllerName, "Detail");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Research);

            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Index");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            var model = new ResearchViewModel();
            try
            {
                model.TheModel = await _repository.ResearchById(id);
            }
            catch (Exception ex)
            {
                LogError(_logger, ex, controllerName, "Detail", id);
            }
           
            model.CurrentPage = "Research";
            model.CurrentOperation = "Detail";

            model.ChangeActive(MenuItemEnum.AdvocacyResearch);
            model.RoleLevel = roleLevel;
            LogEnd(_logger, controllerName, "Detail");
            if (model.TheModel == null)
            {
                return RedirectToAction("Error", "Home", new { area = "" });
            }
            return View(model);
        }

      

        [HttpPost]
        public async Task<IActionResult> Save(Research model)
        {
            LogStart(_logger, controllerName, "Save");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Research);

            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "Save");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            try
            {
                await _repository.Save(model, TheUser.Id);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch(InvalidOperationException ex)
            {
                LogError(_logger, ex, controllerName, "Save", model);
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                LogError(_logger, ex, controllerName, "Save", model);
                CommandStatus(CommandStatusEnum.UnkownError);
            }
            LogEnd(_logger, controllerName, "Save");
            return RedirectToAction("Index");
        }
    }

}
