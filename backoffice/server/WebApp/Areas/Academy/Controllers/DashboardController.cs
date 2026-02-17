using ECCSA.BackOffice.Server.Model.Academy;
using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories.Academy;
using ECCSA.BackOffice.Server.Persistence.Repositories.Security;
using ECCSA.BackOffice.Server.WebApp.Areas.Academy.Model;
using ECCSA.BackOffice.Server.WebApp.Controllers;
using ECCSA.BackOffice.Server.WebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Notification;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Academy.Controllers
{
    [Area("Academy")]
    public class DashboardController : BaseController
    {
        private ILogger<DashboardController> _logger;

        private readonly TrainingRepository _trainingRepository;
        private readonly TopicRepository _topicRepository;
        private readonly RegistrationRepository _registrationRepository;
        private readonly CategoryRepository _categoryRepository;
        public DashboardController(ILogger<DashboardController> logger,
            UserManager<User> userManager,
            CategoryRepository categoryRepository,
            TrainingRepository trainingRepository,
            TopicRepository topicRepository,
        RegistrationRepository registrationRepository,
            PublicUserRepository userRepository, NotificationRepository notificationRepository) : base(userManager, userRepository, notificationRepository)
        {
            _logger = logger;
            _registrationRepository = registrationRepository;
            _categoryRepository = categoryRepository;
            _trainingRepository = trainingRepository;
            _topicRepository = topicRepository;
        }
        [HttpGet]
        public async Task< IActionResult > Index()
        {
            var userName = TheUser.FullName;
            LogStart(_logger, controllerName, "Index");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Academy);
            if(roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Index");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            var model = new AcademyDashboardViewModel
            {
                FullName = userName,
                CurrentPage = "Dashboard",
                CurrentModule = Server.Model.Common.ECCSAModuleFlag.Membership
            };
            model.ChangeActive(MenuItemEnum.AcademyDashboard);

            model.CategoriesCount = _categoryRepository.Categories(TheUser.Id).Where(s => s.Status == 1).Count();
            model.TopicsCount = _topicRepository.Topics(TheUser.Id).Where(t => t.Status == 1).Count();
            model.TrainingsCount = _trainingRepository.Trainings(TheUser.Id).Where(t => t.Status != TrainingStatus.Closed).Count();
            model.RegistrationsCount = _registrationRepository.Registrations(TheUser.Id).Where(
                r => r.RegistrationStatus != RegistrationStatus.Canceled &&
                r.RegistrationStatus != RegistrationStatus.Rejected &&
                r.Training.Status != TrainingStatus.Closed).Count();

            var regSummary = _trainingRepository.TrainingsSummary(TheUser.Id).Where(
                r => r.Status != TrainingStatus.Closed).ToList();

            var summaryDetail = regSummary.GroupBy(g => new 
            { Category = g.Topic.Category.NameEng, Topic = g.Topic.NameEng, 
             TrainingStartDate = g.StartDate.ToString("yyyy-MM-dd")}).Select(s => new DetailModel
            {
                L1 = s.Key.Category,
                L2 = s.Key.Topic,
                L3 = s.Key.TrainingStartDate,
                No = s.Count()
            }).ToList();
            var summary = new SummaryModel()
            {
                L1Label = "Topic Category",
                L2Label = "Topic",
                L3Label = "Training Start Date",
                Details = summaryDetail
            };
            summary.GenerateSummary();
            model.Summary = summary;
            model.SummaryJson = JsonConvert.SerializeObject(model.Summary.Details);
            model.L1Json = JsonConvert.SerializeObject(model.Summary.L1Summary);
            model.L2Json = JsonConvert.SerializeObject(model.Summary.L2Summary);
            model.RoleLevel = roleLevel;
            var setting = await _userRepository.Configuration();
            var plans = await _userRepository.Plans(ECCSAModuleEnum.Academy);
            model.ToPerformance(plans, setting);
            LogEnd(_logger, controllerName, "Index");
            return View(model);
        }
    }
}
