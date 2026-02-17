using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories.UserAdministration;
using ECCSA.BackOffice.Server.WebApp.Areas.Administration.Model;
using ECCSA.BackOffice.Server.WebApp.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Notification;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Administration.Controllers
{
    [Area("Administration")]
    public class DashboardController : BaseController
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly UserAdministrationRepository _repository;
        public DashboardController(ILogger<DashboardController> logger,
        UserAdministrationRepository repository,
            UserManager<User> userManager,
            PublicUserRepository userRepository
        , NotificationRepository notificationRepository) : base(userManager, userRepository, notificationRepository)
        {
            _logger = logger;
            _repository = repository;
            controllerName = "Administration.Dashboard";
        }
        [HttpGet]
        public async Task<IActionResult> Index(ECCSAModuleFlag module)
        {
            var userName = TheUser.FullName;
            LogStart(_logger, controllerName, "Index");
            var roleLevel = await GetRoleLevel(module);
            if(roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Index");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            var model = new DashboardViewModel
            {
                FullName = userName,

                CurrentModule = ECCSAModuleFlag.Administration
            };
        LogEnd(_logger, controllerName, "Index");
            return View(model);
        }
    }
}
