using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Servicing;
using ECCSA.BackOffice.Server.Persistence.Repositories.Security;
using ECCSA.BackOffice.Server.WebApp.Areas.Service.Model;
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

namespace ECCSA.BackOffice.Server.WebApp.Areas.Service.Controllers
{
    [Area("Service")]
    public class DashboardController : BaseController
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly ServiceRequestRepository _serviceRepository;
        private readonly ServiceTypeRepository _serviceTypeRepository;
        public DashboardController(ILogger<DashboardController> logger,
            ServiceTypeRepository serviceTypeRepository,
        ServiceRequestRepository serviceRepository,
            UserManager<User> userManager,
            PublicUserRepository userRepository
            , NotificationRepository notificationRepository) : base(userManager, userRepository, notificationRepository)
        {
            _logger = logger;
            _serviceRepository = serviceRepository;
            _serviceTypeRepository = serviceTypeRepository;
            controllerName = "Service.Dashboard";
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
            var model = new TradeDashboardViewModel
            {
                FullName = userName,
                CurrentPage = "Dashboard",
            };
            model.ChangeActive(MenuItemEnum.ArbitrationDashboard);
            var year = DateTime.Today.Year;
            var month = DateTime.Today.Month;
            var days = DateTime.DaysInMonth(DateTime.Today.Year, month);
            var daysInMonth12 = DateTime.DaysInMonth(DateTime.Today.Year, 12);

            var list = await _serviceRepository.Services(module, string.Empty);
            list = list.Where(s =>
            s.RequestStatus != WorkflowStateEnum.Rejected &&
            s.RequestStatus != WorkflowStateEnum.NotSet).ToList();
            var monthly = list.Where(s => s.RequestDate >= new DateTime(year, month, 1) &&
                s.RequestDate <= new DateTime(year, month, days));
            var yearly = list.Where(s => s.RequestDate >= new DateTime(year, 1, 1) &&
                s.RequestDate <= new DateTime(year, 12, daysInMonth12));
            model.MonthlyEarnings = monthly.Sum(s => s.ServiceType.MemberServiceCharge);
            model.AnnualEarnings = yearly.Sum(s => s.ServiceType.MemberServiceCharge);
            model.MonthlyNoOfProvidedServices = monthly.Count();
            model.AnnualNoOfProvidedServices = yearly.Count();
            model.ServiceTypes = await _serviceTypeRepository.ServiceTypes(module);
            var groupedData = list.ToList().GroupBy(g => g.ServiceType.NameEng);
            var summaryDetail = groupedData
                .Select(s => new DetailModel
                {
                    L1 = s.Key,
                    No = s.Count()
                }).ToList();
            var summary = new SummaryModel()
            {
                L1Label = "No of Services by Type",
                Details = summaryDetail
            };
            model.Summary = summary;
            model.SummaryJson = JsonConvert.SerializeObject(model.Summary.Details);


            var summaryDetail2 = groupedData
                .Select(s => new DetailModel
                {
                    L1 = s.Key,
                    No = int.Parse(Math.Floor(s.Sum(s => s.ServiceType.MemberServiceCharge))
                    .ToString())
                }).ToList();
            var summary2 = new SummaryModel()
            {
                L1Label = "Collected Revenue by Services Type",
                Details = summaryDetail2
            };
            model.Summary2 = summary2;
            model.SummaryJson2 = JsonConvert.SerializeObject(model.Summary2.Details);
            model.CurrentModule = module;
            model.RoleLevel = roleLevel;
            model.CurrentSubsystem = (ECCSAModuleEnum)(int)module;
            var setting = await _userRepository.Configuration();
            var plans = await _userRepository.Plans(model.CurrentSubsystem);
            model.ToPerformance(plans, setting);
            LogEnd(_logger, controllerName, "Index");
            return View(model);
        }
    }
}
