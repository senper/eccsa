using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Membership;
using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories.Membership;
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
    public class ContactUsController : BaseController
    {
        private readonly ILogger<ContactUsController> _logger;
        private readonly IssueRepository _repository;
        public ContactUsController(
            UserManager<User> userManager
            , ILogger<ContactUsController> logger
            , IssueRepository repository
            , PublicUserRepository userRepository, NotificationRepository notificationRepository) : base(userManager, userRepository, notificationRepository)
        {
            _logger = logger;
            _repository = repository;
            controllerName = "ContactUs";
        }
        public async Task<IActionResult> Index(DateTime? date = null)
        {
            var model = new ContactUsViewModel();
            LogStart(_logger, controllerName, "Index");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Communication);
            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Index");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            try
            {
                date = date ?? DateTime.Today.AddDays(-7);
                model.TheList = await _repository.ContactUsList(date.Value);
                model.Date = date.Value;
            }
            catch (Exception ex)
            {
                model.TheList = new List<ContactUs>();
                _logger.LogError(ex, "Index");
                return RedirectToAction("Error", "Home", new { area = "" });
            }
            model.CurrentPage = "ContactUs";
            model.CurrentOperation = "Index";
            model.ChangeActive(MenuItemEnum.CommunicationContactUs);
            model.RoleLevel = roleLevel;
            LogEnd(_logger, controllerName, "Index");

            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Detail(Guid id, DateTime date)
        {
            LogStart(_logger, controllerName, "Detail");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Communication);
            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Detail");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            var model = new ContactUsViewModel();
            try
            {
                model.TheModel = await _repository.ContactUs(id);
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
            model.Date = date;
            LogEnd(_logger, controllerName, "Detail");
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id, DateTime date)
        {
            LogStart(_logger, controllerName, "DeleteContactUs");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Communication);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "DeleteContactUs");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("Index");
            }
            try
            {
                await _repository.DeleteContactUs(id);
                CommandStatus(CommandStatusEnum.Deleted);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("DeleteContactUs", ex, id);
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("DeleteContactUs", ex, id);
                CommandStatus(CommandStatusEnum.UnkownError);
            }

            return RedirectToAction("Index", new { date });
        }
    }
}

