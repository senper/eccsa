using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Notification;
using ECCSA.BackOffice.Server.Persistence.Repositories.Communication;
using ECCSA.BackOffice.Server.Persistence.Repositories.Security;
using ECCSA.BackOffice.Server.WebApp.Areas.Communication.Model;
using ECCSA.BackOffice.Server.WebApp.Controllers;
using ECCSA.BackOffice.Server.WebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Communication.Controllers
{
    [Area("Communication")]
    public class DashboardController : BaseController
    {
        private readonly ILogger<DashboardController> _logger;
        readonly MediaAgencyRepository _repository;
        public DashboardController(ILogger<DashboardController> logger,
            UserManager<User> userManager,
            MediaAgencyRepository repository,
            PublicUserRepository userRepository
            , NotificationRepository notificationRepository) : base(userManager, userRepository, notificationRepository)
        {
            _logger = logger;
            _repository = repository;
            controllerName = "Communication.Dashboard";
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userName = TheUser.FullName;
            LogStart(_logger, controllerName, "Index");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Communication);
            if(roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Index");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            var model = new DashboardViewModel
            {
                FullName = userName,
                CurrentPage = "Dashboard",
                CurrentModule = ECCSAModuleFlag.Communication
            };
            var year = DateTime.Today.Year;
            var month = DateTime.Today.Month;
            var days = DateTime.DaysInMonth(DateTime.Today.Year, month);
            var daysInMonth12 = DateTime.DaysInMonth(DateTime.Today.Year, 12);
            var list1 = await _repository.MediaAgencies();
            var list2 = await _repository.MediaCoverages();
            var list3 = await _repository.Campaigns();
            model.MonthlyCoverage = list2.Count(l => l.StartDate >= new DateTime(year, month, 1));
            model.AnnualCoverage = list2.Count(l => l.StartDate >= new DateTime(year, 1, 1));
            model.MonthlyCampaings = list3.Count(l => l.StartDate >= new DateTime(year, month, 1));
            model.AnnualCampaings = list3.Count(l => l.StartDate >= new DateTime(year, 1, 1));

            var groupedData = list2.ToList().GroupBy(g => g.MediaAgency.Name);
            var summaryDetail = groupedData
                .Select(s => new DetailModel
                {
                    L1 = s.Key,
                    No = s.Count()
                }).ToList();
            var summary = new SummaryModel()
            {
                L1Label = "No of Media Coverage",
                Details = summaryDetail
            };
            model.Summary = summary;
            model.SummaryJson = JsonConvert.SerializeObject(model.Summary.Details);
            

            model.ChangeActive(MenuItemEnum.Dashboard);
            model.RoleLevel = roleLevel;
            var setting = await _userRepository.Configuration();
            var plans = await _userRepository.Plans(ECCSAModuleEnum.Communication);
            model.ToPerformance(plans, setting);
            LogEnd(_logger, controllerName, "Index");
            return View(model);
        }

        
    }
}
