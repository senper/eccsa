using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Advocacy;
using ECCSA.BackOffice.Server.Persistence.Repositories.Security;
using ECCSA.BackOffice.Server.WebApp.Areas.Advocacy.Model;
using ECCSA.BackOffice.Server.WebApp.Controllers;
using ECCSA.BackOffice.Server.WebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Notification;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Advocacy.Controllers
{
    [Area("Advocacy")]
    public class DashboardController : BaseController
    {
        private readonly ILogger<DashboardController> _logger;
        readonly AdvocacyRepository _repository;
        public DashboardController(ILogger<DashboardController> logger,
            UserManager<User> userManager,
            AdvocacyRepository repository, 
            PublicUserRepository userRepository
            , NotificationRepository notificationRepository) : base(userManager, userRepository, notificationRepository)
        {
            _logger = logger;
            _repository = repository;
            controllerName = "Advocacy.Dashboard";
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userName = TheUser.FullName;
            LogStart(_logger, controllerName, "Index");

            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Research);
            if(roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Index");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            var model = new DashboardViewModel
            {
                FullName = userName,
                CurrentPage = "Dashboard",
                CurrentModule = ECCSAModuleFlag.Research
            };
            var year = DateTime.Today.Year;
            var month = DateTime.Today.Month;
            var days = DateTime.DaysInMonth(DateTime.Today.Year, month);
            var daysInMonth12 = DateTime.DaysInMonth(DateTime.Today.Year, 12);
            try
            {
               var list1 = await _repository.Dialogs();
                var list2 = await _repository.Researches();
                var list3 = await _repository.Projects();
                var list4 = await _repository.Publications();
                model.Dialogs = list1.Count;
                model.Researches= list2.Count;
                model.Projects = list3.Count;
                model.Publications = list4.Count;

                var groupedData = list2.ToList().GroupBy(g => g.Stage);
                var summaryDetail = groupedData
                    .Select(s => new DetailModel
                    {
                        L1 = s.Key.GetDisplayName(),
                        No = s.Count()
                    }).ToList();
                var summary = new SummaryModel()
                {
                    L1Label = "No of Researches by Stage",
                    Details = summaryDetail
                };
                model.Summary = summary;
                model.SummaryJson = JsonConvert.SerializeObject(model.Summary.Details);

                var groupedData2 = list3.GroupBy(g => g.ProjectStatus);
                var summaryDetail2 = groupedData2
               .Select(s => new DetailModel
               {
                   L1 = s.Key.GetDisplayName(),
                   No = s.Count()
               }).ToList();
                var summary2 = new SummaryModel()
                {
                    L1Label = "Project Status",
                    Details = summaryDetail2
                };
                model.Summary2 = summary2;
                model.SummaryJson2 = JsonConvert.SerializeObject(model.Summary2.Details);
                var setting = await _userRepository.Configuration();
                var plans = await _userRepository.Plans(ECCSAModuleEnum.Research);
                model.ToPerformance(plans, setting);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "RSS Feed");
            }
            LogEnd(_logger, controllerName, "Index");
            model.ChangeActive(MenuItemEnum.Dashboard);
            return View(model);
        }

        
    }
}
