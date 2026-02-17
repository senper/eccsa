using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Notification;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Servicing;
using ECCSA.BackOffice.Server.Persistence.Repositories.Security;
using ECCSA.BackOffice.Server.WebApp.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Events.Controllers
{
    [Area("Events")]
    public class DashboardController : BaseController
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly ServiceRequestRepository _serviceRepository;
        private readonly ServiceTypeRepository _serviceTypeRepository;
        public DashboardController(ILogger<DashboardController> logger,
            ServiceTypeRepository serviceTypeRepository,
        ServiceRequestRepository serviceRepository,
            UserManager<User> userManager,
            PublicUserRepository userRepository, NotificationRepository notificationRepository) : base(userManager, userRepository, notificationRepository)
        {
            _logger = logger;
            _serviceRepository = serviceRepository;
            _serviceTypeRepository = serviceTypeRepository;
        }
        //[HttpGet]
        //public async Task<IActionResult> Index(ECCSAModuleFlag module)
        //{
        //    var userName = TheUser.FullName;

        //    var model = new TradeDashboardViewModel
        //    {
        //        FullName = userName,
        //        CurrentPage = "Dashboard",
        //    };
        //    model.ChangeActive(MenuItemEnum.ArbitrationDashboard);
        //    var year = DateTime.Today.Year;
        //    var month = DateTime.Today.Month;
        //    var days = DateTime.DaysInMonth(DateTime.Today.Year, month);
        //    var daysInMonth12 = DateTime.DaysInMonth(DateTime.Today.Year, 12);
        //    var list = await _serviceRepository.Services(module, string.Empty);
        //    list = list.Where(s =>
        //    s.RequestStatus != WorkflowStateEnum.Rejected &&
        //    s.RequestStatus != WorkflowStateEnum.NotSet).ToList();
        //    var monthly = list.Where(s => s.RequestDate >= new DateTime(year, month, 1) &&
        //        s.RequestDate <= new DateTime(year, month, days));
        //    var yearly = list.Where(s => s.RequestDate >= new DateTime(year, 1, 1) &&
        //        s.RequestDate <= new DateTime(year, 12, daysInMonth12));
        //    var groupedData = list.ToList().GroupBy(g => g.ServiceType.NameEng);
        //    var summaryDetail = groupedData
        //        .Select(s => new DetailModel
        //        {
        //            L1 = s.Key,
        //            No = s.Count()
        //        }).ToList();
        //    var summary = new SummaryModel()
        //    {
        //        L1Label = "No of Services by Type",
        //        Details = summaryDetail
        //    };


        //    var summaryDetail2 = groupedData
        //        .Select(s => new DetailModel
        //        {
        //            L1 = s.Key,
        //            No = int.Parse(Math.Floor(s.Sum(s => s.ServiceType.MemberServiceCharge))
        //            .ToString())
        //        }).ToList();
        //    var summary2 = new SummaryModel()
        //    {
        //        L1Label = "Collected Revenue by Services Type",
        //        Details = summaryDetail2
        //    };
        //    model.CurrentModule = module;
        //    return View(model);
        //}
    }
}
