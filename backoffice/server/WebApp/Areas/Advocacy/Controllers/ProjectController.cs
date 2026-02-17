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
    public class ProjectController : BaseController
    {
        private ILogger<ProjectController> _logger;
        AdvocacyRepository _repository;
        
        public ProjectController(ILogger<ProjectController> logger,
            UserManager<User> userManager,
            PublicUserRepository userRepository,
            AdvocacyRepository repository, NotificationRepository notificationRepository) : base(userManager,userRepository, notificationRepository)
        {

            _logger = logger;
            _repository = repository;

            controllerName = "Project";

        }

        public async Task<IActionResult> Index()
        {
            var model = new ProjectViewModel();
           LogStart(_logger, controllerName, "Index");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Research);
            if(roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Index");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            try
            {
                model.TheList = await _repository.Projects();
            }
            catch (Exception ex)
            {
                model.TheList = new List<Project>();
                _logger.LogError(ex, "Project Index Exception");
            }
            model.CurrentPage = "Project";
            model.CurrentOperation = "Index";
            model.RoleLevel = roleLevel;
            LogEnd(_logger, controllerName, "Index");
            model.ChangeActive(MenuItemEnum.AdvocacyProject);
            return View(model);
        }

        public async Task<IActionResult> Detail(Guid id)
        {
            
            var model = new ProjectViewModel();
            LogStart(_logger, controllerName, "Index");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Research);

            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Detail");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            try
            {
                model.TheModel = await _repository.ProjectById(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Project Detail Exception {id}");
            }
            // model.TheModel.MediaType2Bool();
            model.RoleLevel = roleLevel;
            model.CurrentOperation = "Detail";
            if (model.TheModel == null)
            {
                return RedirectToAction("Error", "Home", new { area = "" });
            }
            model.ChangeActive(MenuItemEnum.AdvocacyProject);
            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> Save(Project model)
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

        [HttpPost]
        public async Task<IActionResult> SaveFollowup(ProjectFollowup model)
        {
            LogStart(_logger, controllerName, "SaveFollowup");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Research);

            if (roleLevel < RoleLevelEnum.User)
            {
                LogSecurityError(_logger, controllerName, "SaveFollowup");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            try
            {
                await _repository.Save(model, TheUser.Id);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("Save Followup", ex, model);
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Save Followup", ex, model);
                CommandStatus(CommandStatusEnum.UnkownError);
            }

            return RedirectToAction("Detail", new {id=model.ProjectId });
        }
    }

}
