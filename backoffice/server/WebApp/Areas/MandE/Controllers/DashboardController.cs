using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.mande;
using ECCSA.BackOffice.Server.Persistence.Repositories.Security;
using ECCSA.BackOffice.Server.WebApp.Areas.MandE.Model;
using ECCSA.BackOffice.Server.WebApp.Controllers;
using ECCSA.BackOffice.Server.WebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Model.Common.Planning;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Notification;

namespace ECCSA.BackOffice.Server.WebApp.Areas.MandE.Controllers
{
    [Area("mande")]
    public class DashboardController : BaseController
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly MandERepository _repository;
        private readonly MandEFinancialRepository _financialRepository;


        public DashboardController(ILogger<DashboardController> logger,
            UserManager<User> userManager,
            MandERepository repository,
            MandEFinancialRepository financialRepository,
                       PublicUserRepository userRepository
            , NotificationRepository notificationRepository) : base(userManager, userRepository, notificationRepository)
        {
            _logger = logger;
            _repository = repository;
            _financialRepository = financialRepository;
            controllerName = "M&E.Dashboard";

        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            LogStart(_logger, controllerName, "Index");
            var userName = TheUser.FullName;
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.MandE);
            if(roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Index");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            var setting = await  _repository.Configuration();
            var model = new DashboardViewModel
            {
                FullName = userName,
                CurrentPage = "Dashboard",
                CurrentModule = ECCSAModuleFlag.MandE,
                Configuration = setting,
                AllPlanYears = await _repository.PlanYears(),
                PlanPhase = setting.PlanningPhase,
                PlanVersionNo = setting.PlanVersionNo,
            };
            
            model.PlanYears.AddRange(model.AllPlanYears);
            foreach (var i in model.Configuration.ClosedYears)
                model.PlanYears.Remove(i);
            
            try
            {
                model.TheList = await _repository.Plans();
                model.List = await _financialRepository.Financials(model.Configuration.CurrentYear);
                if (model.List == null)
                    model.List = new System.Collections.Generic.List<FinancialPlan>();
            }
            
            catch (Exception ex)
            {
                model.TheList = new System.Collections.Generic.List<Plan>();
                _logger.LogError(ex, "RSS Feed");
            }
            model.ChangeActive(MenuItemEnum.Dashboard);
            model.RoleLevel = roleLevel;
            LogEnd(_logger, controllerName, "Index");
            foreach(var item in model.TheList)
            {
                try
                {
                    var itemName = item.NameWithCode;
                    var unit = item.UnitName;
                    var planAmount = item.PlanAmountName;
                    var prog = item.AnnualProgress;
                }
                catch (Exception ex)
                {
                    var msg = ex;
                }
            }
            return View(model);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeCurrentPeriod()
        {
            LogStart(_logger, controllerName, "Index");
            var userName = TheUser.FullName;
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.MandE);
            if (roleLevel < RoleLevelEnum.Admin)
            {
                LogSecurityError(_logger, controllerName, "Index");
                return RedirectToAction("Index");
            }
            try
            {
                await _repository.ChangeCurrentPeriod();
                CommandStatus("Period Changed!");
            }
            catch(InvalidOperationException ex)
            {
                CommandStatus(ex.Message);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ClosePlanning()
        {
            LogStart(_logger, controllerName, "ClosePlanning");
            var userName = TheUser.FullName;
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.MandE);
            if (roleLevel < RoleLevelEnum.Admin)
            {
                LogSecurityError(_logger, controllerName, "ClosePlanning");
                return RedirectToAction("Index");
            }
            try
            {
                await _repository.ClosePlanning();
                CommandStatus("Planning Phase Closed!");
            }
            catch (InvalidOperationException ex)
            {
                CommandStatus(ex.Message);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePlanYear(int planYear, int templateYear = 0)
        {
            LogStart(_logger, controllerName, "Index");
            var userName = TheUser.FullName;
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.MandE);
            if (roleLevel < RoleLevelEnum.Admin)
            {
                LogSecurityError(_logger, controllerName, "Index");
                return RedirectToAction("Index");
            }
            try
            {
                await _repository.ChangePlanYear(planYear, templateYear, TheUser);
                CommandStatus("Plan Year Changed!");
            }
            catch (InvalidOperationException ex)
            {
                CommandStatus(ex.Message);
            }
            
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Replan()
        {
            LogStart(_logger, controllerName, "Index");
            var userName = TheUser.FullName;
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.MandE);
            if (roleLevel < RoleLevelEnum.Admin)
            {
                LogSecurityError(_logger, controllerName, "Index");
                return RedirectToAction("Index");
            }
            try
            {
                var config = await _repository.Configuration();
                var currentYear = config.CurrentYear;
                var currentMonth = config.CurrentMonth;
                if (currentMonth > 9)
                {
                    CommandStatus("Short Period to replan");
                    return RedirectToAction("Index");
                }
                await _repository.RevisePlan(currentYear, currentMonth + 1, TheUser);
                CommandStatus("Plan Version Changed!");
            }
            catch (InvalidOperationException ex)
            {
                CommandStatus(ex.Message);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ClosePeriod(int planYear)
        {
            LogStart(_logger, controllerName, "Index");
            var userName = TheUser.FullName;
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.MandE);
            if (roleLevel < RoleLevelEnum.Admin)
            {
                LogSecurityError(_logger, controllerName, "Index");
                return RedirectToAction("Index");
            }
            try
            {
                await _repository.ClosePeriod(planYear);
                CommandStatus("Period Closed!");
            }
            catch (InvalidOperationException ex)
            {
                CommandStatus(ex.Message);
            }

            return RedirectToAction("Index");
        }

    }
}
