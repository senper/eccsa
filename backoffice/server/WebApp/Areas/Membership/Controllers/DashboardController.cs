using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Notification;
using ECCSA.BackOffice.Server.Persistence.Repositories.Membership;
using ECCSA.BackOffice.Server.Persistence.Repositories.Security;
using ECCSA.BackOffice.Server.WebApp.Areas.Membership.Model;
using ECCSA.BackOffice.Server.WebApp.Controllers;
using ECCSA.BackOffice.Server.WebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Membership.Controllers
{
    [Area("Membership")]
    public class DashboardController : BaseController
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly BusinessInformationRepository _repository;
        private readonly MembershipEnrollmentRepository _enrollmentRepository;
        readonly AssociationRepository _associationRepository;
        private readonly IndividualRepository _individualRepoistory;
        private readonly ApplicantRepository _applicantRepository;

        public DashboardController(ILogger<DashboardController> logger,
            UserManager<User> userManager,
            BusinessInformationRepository businessInformationRepository,
            MembershipEnrollmentRepository enrollmentRepository,
            PublicUserRepository userRepository,
            AssociationRepository associationRepository,
            IndividualRepository individualRepoistory,
            ApplicantRepository applicantRepository
            , NotificationRepository notificationRepository) : base(userManager, userRepository, notificationRepository)
        {
            _logger = logger;
            _repository = businessInformationRepository;
            _enrollmentRepository = enrollmentRepository;
            _associationRepository = associationRepository;
            _individualRepoistory = individualRepoistory;
            _applicantRepository = applicantRepository;

            controllerName = "Membership.Dashboard";

        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            LogStart(_logger, controllerName, "Index");
            var userName = TheUser.FullName;
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Membership);
            if(roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Index");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            var model = new DashboardViewModel
            {
                FullName = userName,
                CurrentPage = "Dashboard",
                CurrentModule = ECCSAModuleFlag.Membership,
                IsMemberUser = TheUser.AssignedModules > 0 && TheUser.AssociationId != Guid.Empty && TheUser.AssociationId != Constants.ECCSAId
            };

            try
            {
                var bi = await _repository.PastBusinessInformationList();
                model.BusinessInformation = bi
                    .GroupBy(g => new { g.EndDate.Year, g.EndDate.Month })
                    .Select(g => new PeriodReport { Year= g.Key.Year,Month= g.Key.Month,Value= g.Count() }).ToList();
               var cbi = await _repository.ActiveBusinessInformationList();
                var tmp = cbi.GroupBy(g => new { g.EndDate.Year, g.EndDate.Month })
                    .Select(g => new PeriodReport { Year = g.Key.Year, Month = g.Key.Month, Value = g.Count() }).ToList();
                model.BusinessInformation.AddRange(tmp);
                model.AssociationsCount = await _associationRepository.ListCount(TheUser.Id);
                model.Members = await _individualRepoistory.ListCount(TheUser.Id);
                model.Applicants = await _applicantRepository.ListCount(TheUser.AssociationId, TheUser.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "RSS Feed");
            }
            model.ChangeActive(MenuItemEnum.Dashboard);
            var setting = await _userRepository.Configuration();
            var plans = await _userRepository.Plans(ECCSAModuleEnum.Membership);
            model.ToPerformance(plans, setting);
            LogEnd(_logger, controllerName, "Index");
            return View(model);
        }

        public IActionResult GetMemberships()
        {
            try
            {
                var list = _enrollmentRepository.AggregateMembershipEnrollments(TheUser.AssociationId, TheUser.Id)
                    .ToList();

               // var data = JsonConvert.SerializeObject(list);

                return Json(list);//, "application/json");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "GetMemberships");
                return Content("Error", "application/json");
            }
        }
    }
}
