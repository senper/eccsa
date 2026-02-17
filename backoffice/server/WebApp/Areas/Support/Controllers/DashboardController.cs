using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories.Security;
using ECCSA.BackOffice.Server.WebApp.Areas.Support.Model;
using ECCSA.BackOffice.Server.WebApp.Controllers;
using ECCSA.BackOffice.Server.WebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Notification;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Support.Controllers
{
    [Area("Support")]
    public class DashboardController : BaseController
    {
        private ILogger<DashboardController> _logger;

        public DashboardController(ILogger<DashboardController> logger,
            UserManager<User> userManager,
            PublicUserRepository userRepository
            , NotificationRepository notificationRepository) : base(userManager, userRepository, notificationRepository)
        {
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userName = TheUser.FullName;
            LogStart(_logger, controllerName, "Index");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Support);
            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Index");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            var model = new SupportDashboardViewModel
            {
                FullName = userName,
                CurrentPage = "Dashboard",
                CurrentModule = Server.Model.Common.ECCSAModuleFlag.Support
            };
            model.ChangeActive(MenuItemEnum.Dashboard);

            model.RoleLevel = roleLevel;
            var setting = await _userRepository.Configuration();
            var plans = await _userRepository.Plans(ECCSAModuleEnum.Support);
            model.ToPerformance(plans, setting);
            LogEnd(_logger, controllerName, "Index");
            return View(model);
        }
    }
}
