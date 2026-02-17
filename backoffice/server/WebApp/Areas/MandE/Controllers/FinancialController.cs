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
using System.Linq;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Model.Common.Planning;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Notification;

namespace ECCSA.BackOffice.Server.WebApp.Areas.MandE.Controllers
{
    [Area("mande")]
    public class FinancialController : BaseController
    {
        
        private readonly ILogger<FinancialController> _logger;
        private readonly MandEFinancialRepository _repository;
        public FinancialController(ILogger<FinancialController> logger,
        UserManager<User> userManager,
        MandEFinancialRepository repository,
        PublicUserRepository userRepository

        , NotificationRepository notificationRepository) : base(userManager, userRepository, notificationRepository)
        {

            _logger = logger;
            _repository = repository;
            controllerName = "Financial";
        }
        private static string GetCurrentPage(ECCSAModuleFlag module)
        {
            return module switch
            {
                ECCSAModuleFlag.Academy => "Chamber Academy M&E",
                ECCSAModuleFlag.Arbitration => "Arbitration M&E",
                ECCSAModuleFlag.Communication => "Communication M&E",
                ECCSAModuleFlag.Investment => "Trade Promotion M&E",
                ECCSAModuleFlag.Membership => "Membership M&E",
                ECCSAModuleFlag.Reporting => "Reporting M&E",
                ECCSAModuleFlag.Research => "Research & Advocacy M&E",
                ECCSAModuleFlag.TradePromotion => "Trade Facilitation M&E",
                _ => string.Empty,
            };
        }
        public async Task<IActionResult> Index(int planYear = 0 )
        {
            ECCSAModuleEnum subSystem = ECCSAModuleEnum.MandE;
            var module = (ECCSAModuleFlag)(int)subSystem;
            var setting = await _repository.Configuration();
            var roleLevel = await GetRoleLevel(module);
            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Index");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            var list = await _repository.FinancialItems();

            var model = new FinancialPlanViewModel
            {
                TheModel = new FinancialPlan(),
                TheList = await _repository.Financials(planYear),
                Items = await _repository.FinancialItems(),
                FullName = TheUser.FullName,
                PlanYear = setting.PlanYear
            };
            
            model.RoleLevel = roleLevel;
            model.ChangeActive(MenuItemEnum.FinancialPlan);
            model.CurrentModule = module;
            model.CurrentSubsystem = subSystem;
            model.CurrentOperation = "List";
            return View(model);
        }

        public async Task<IActionResult> Progress(int planYear = 0)
        {
            ECCSAModuleEnum subSystem = ECCSAModuleEnum.MandE;
            var module = (ECCSAModuleFlag)(int)subSystem;
            var setting = await _repository.Configuration();
            var roleLevel = await GetRoleLevel(module);
            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Index");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            var list = await _repository.FinancialItems();

            var model = new FinancialPlanViewModel
            {
                TheModel = new FinancialPlan(),
                TheList = await _repository.Financials(planYear),
                Items = await _repository.FinancialItems(),
                FullName = TheUser.FullName,
                PlanYear = setting.PlanYear
            };

            model.RoleLevel = roleLevel;
            model.ChangeActive(MenuItemEnum.ReviewProgress);
            model.CurrentModule = module;
            model.CurrentSubsystem = subSystem;
            model.CurrentMonth = setting.CurrentMonth;
            model.CurrentYear = setting.CurrentYear;
            model.CurrentOperation = "Progress";
            return View(model);
        }

        public async Task<IActionResult> Items(bool showActive, bool showInactive)
        {
            var setting = await _repository.Configuration();
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.MandE);
            var model = new FinancialItemViewModel
            {
                TheModel = new FinancialItem(),
                TheList = await _repository.FinancialItems(),
                FullName = TheUser.FullName,
                PlanYear = setting.PlanYear,
                ShowActive = showActive,
                ShowInactive = showInactive,
            };
            if (showActive)
            {
                model.TheList = showInactive ? model.TheList : model.TheList.Where(i => i.Status == StatusEnum.Active).ToList();
            }
            else
            {
                model.TheList = showInactive ? model.TheList.Where(i => i.Status == StatusEnum.Inactive || i.IsLabel).ToList() : model.TheList;
            }
            model.Parents = model.TheList.Where(f=> f.Status == StatusEnum.Active).OrderBy(f=> f.Code).ThenBy(f=> f.Name).ToList();
            model.CurrentOperation = "List";
            model.RoleLevel = roleLevel;
            model.ChangeActive(MenuItemEnum.FinancialItems);
            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateLabel(FinancialItem plan )
        {
            ECCSAModuleEnum subsystem = ECCSAModuleEnum.MandE;
            LogStart(_logger, controllerName, "CreateLabel");
            var module = (ECCSAModuleFlag)(int)subsystem;
            var roleLevel = await GetRoleLevel(module);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "Detail");
                return RedirectToAction("Index", new { id = plan.Id, subsystem });
            }
            if (plan.Id == Guid.Empty)
            {
                plan.Id = Guid.NewGuid();
            }
            try
            {
                
                 await _repository.Save(plan, TheUser.Id);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (Exception ex)
            {
                LogError(_logger, ex, controllerName, "CreateLabel", plan);
                TempData["CommandStatus"] = ex.Message;
            }

            return RedirectToAction("Items");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SavePlan(QuarterlyPlan data)
        {
            ECCSAModuleEnum subsystem = ECCSAModuleEnum.MandE;
            LogStart(_logger, controllerName, "SaveQuarterly");
            var module = (ECCSAModuleFlag)(int)subsystem;
            var roleLevel = await GetRoleLevel(module);
  
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "Detail");
                return RedirectToAction("Index", new { subsystem });
            }
            try
            {
                
                var plan = new FinancialPlan(data.Id,data.Q1, data.Q2, data.Q3, data.Q4);
                await _repository.Save(plan, TheUser.Id);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (Exception ex)
            {
                LogError(_logger, ex, controllerName, "UpdateLabel", data);
                TempData["CommandStatus"] = ex.Message;
            }

            return RedirectToAction("Index");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveProgress(QuarterlyProgress data)
        {
            ECCSAModuleEnum subsystem = ECCSAModuleEnum.MandE;
            LogStart(_logger, controllerName, "SaveProgress");
            var module = (ECCSAModuleFlag)(int)subsystem;
            var roleLevel = await GetRoleLevel(module);

            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "Detail");
                return RedirectToAction("Index", new { subsystem });
            }
            try
            {

               
                await _repository.Save(data, TheUser.Id);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (Exception ex)
            {
                LogError(_logger, ex, controllerName, "UpdateLabel", data);
                TempData["CommandStatus"] = ex.Message;
            }

            return RedirectToAction("Progress");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteItem(Guid id)
        {
            ECCSAModuleEnum subsystem = ECCSAModuleEnum.MandE;
            LogStart(_logger, controllerName, "UpdateLabel");
            var module = (ECCSAModuleFlag)(int)subsystem;
            var roleLevel = await GetRoleLevel(module);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "Detail");
                return RedirectToAction("Items", new { subsystem });
            }
            try
            {
               
                await _repository.DeleteItem(id);
                CommandStatus(CommandStatusEnum.Deleted);
            }
            catch (Exception ex)
            {
                LogError(_logger, ex, controllerName, "DeleteItem", id);
                TempData["CommandStatus"] = ex.Message;
            }

            return RedirectToAction("Items");

        }
        //public async Task<IActionResult> Detail(Guid Id)
        //{
        //    ECCSAModuleEnum subsystem = ECCSAModuleEnum.MandE;
        //    LogStart(_logger, controllerName, "Detail");
        //    var plan = await _repository.FinancialItem(Id);
        //    var module = (ECCSAModuleFlag)(int)subsystem;
        //    if (plan == null)
        //    {
        //        return RedirectToAction("Index", new { subsystem });
        //    }
        //    var userName = TheUser.FullName;
        //    var roleLevel = await GetRoleLevel(module);

        //    FinancialItemViewModel model;
        //    if (roleLevel <= RoleLevelEnum.None)
        //    {
        //        LogSecurityError(_logger, controllerName, "Detail");
        //        return RedirectToAction("Login", "Account", new { area = "Identity" });
        //    }
        //    model = new FinancialItemViewModel
        //    {
                
                
        //        TheModel = await _repository.FinancialItem(Id),
        //        RoleLevel = roleLevel,
        //        CurrentModule = module,
        //        CurrentSubsystem = subsystem,


                



        //    };
        //    model.RoleLevel = roleLevel;

        //    model.ChangeActive(MenuItemEnum.Plan);
        //    TempData["title"] = model.CurrentPage;
        //    LogEnd(_logger, controllerName, "Detail");
        //    return View(model);
        //}
    
    
    }
}
