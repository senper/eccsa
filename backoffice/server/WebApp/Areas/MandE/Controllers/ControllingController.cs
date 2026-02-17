using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common;
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
    public class ControllingController : BaseController
    {

        private readonly ILogger<ControllingController> _logger;
        private readonly MandERepository _repository;
        private readonly YesNoRepository _lookupRepository;
        public ControllingController(ILogger<ControllingController> logger, UserManager<User> userManager, MandERepository repository, PublicUserRepository userRepository, YesNoRepository lookupRepository, NotificationRepository notificationRepository) : base(userManager, userRepository, notificationRepository)
        {

            _logger = logger;
            _repository = repository;
            controllerName = "Controlling";
            _lookupRepository = lookupRepository;
        }

        private (ECCSAModuleEnum ResponsibleUnit,int SupportingUnitId) GetUnitId(string responsibleUnitFilter)
        {
            var resUnitFilterList = responsibleUnitFilter == null ? new string[] { "0", "0" } :
                responsibleUnitFilter.Split("_");
            var responsibleUnit = (ECCSAModuleEnum)( resUnitFilterList.Length > 0 && int.TryParse(resUnitFilterList[0], out int val1) ? val1 : 0);
            var supportingUnit = resUnitFilterList.Length > 1 && int.TryParse(resUnitFilterList[1], out int val) ? val : 0;
            return ( responsibleUnit, supportingUnit );

        }
        public async Task<IActionResult> Index( 
            string responsibleUnitFilter ,
            bool treeMode )
        {
            var module = ECCSAModuleFlag.MandE;
            var setting = await _repository.Configuration();
            var roleLevel = await GetRoleLevel(module);
            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Index");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            var respSupp = GetUnitId(responsibleUnitFilter);
            var responsibleUnit = respSupp.ResponsibleUnit;
            var supportingUnit = respSupp.SupportingUnitId;

            var list = await _repository.Plans();
            if (responsibleUnit > 0)
            {
                list = list.Where(l => (l.IsLabel && l.ResponsibleUnit == ECCSAModuleEnum.None) 
                                       || (l.SupportingUnitId == supportingUnit && l.ResponsibleUnit == responsibleUnit))
                    .ToList();
            }
            

          
            if (!treeMode)
            {
               
                list = list.Where(l =>!l.IsLabel )
                    .ToList();
            }

            var model = new PlanViewModel
            {
                TheModel = new Plan(),
                TheList = list,
                FullName = TheUser.FullName,
                PlanYear = setting.PlanYear,
                SupportingUnitId = supportingUnit,
                SelectedResponsibleUnit = (ECCSAModuleEnum)responsibleUnit,
                CurrentPage = "Plan Approval",
                ResponsibleUnitFilter = responsibleUnitFilter,
                TreeView = treeMode,
                PlanVersionNo = setting.PlanVersionNo,
                PlanPhase = setting.PlanningPhase,
                CurrentOperation = "Index"
            };
            model.SupportingUnits = (await _lookupRepository.LookupTypeById(SupportingUnitLookupTypeId))?.LookupValues.ToList();
            model.RoleLevel = roleLevel;
            model.ChangeActive(MenuItemEnum.ApprovePlan);
            model.CurrentModule = module;
            model.CurrentSubsystem = ECCSAModuleEnum.MandE;
            return View(model);
        }

        
        public async Task<IActionResult> Header()
        {
            var setting = await _repository.Configuration();
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.MandE);
            var planYear =  setting.PlanYear;    
            var model = new PlanHeaderViewModel
            {
                TheModel = new Plan(),
                TheList = await _repository.Plans(),
                FullName = TheUser.FullName,
                PlanYear = setting.PlanYear,
                PlanPhase = setting.PlanningPhase,
                PlanVersionNo = setting.PlanVersionNo,
            };
            //model.Parents = model.TheList.FindAll(x => x.CanBeParent);
            model.RoleLevel = roleLevel;
            model.SupportingUnits = (await _lookupRepository.LookupTypeById(SupportingUnitLookupTypeId))?.LookupValues.ToList();
            model.ChangeActive(MenuItemEnum.Header);
            model.CurrentSubsystem = ECCSAModuleEnum.MandE;
            return View(model);
        }
      

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateLabel(Plan plan, string responsibleUnitFilter, ECCSAModuleEnum subsystem)
        {
            LogStart(_logger, controllerName, "CreateLabel");
            var module = (ECCSAModuleFlag)(int)subsystem;
            var setting = await _repository.Configuration();
            if (setting.PlanningPhase == PlanningPhaseEnum.Closed)
            {
                CommandStatus("Planning is closed");
                return RedirectToAction("Header", new { planYear = plan.PlanYear });
            }
            if (setting.PlanningPhase == PlanningPhaseEnum.Unknown)
            {
                CommandStatus("Planning status is Unknown");
                return RedirectToAction("Header", new { planYear = plan.PlanYear });
            }
            var roleLevel = await GetRoleLevel(module);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "Detail");
                return RedirectToAction("Detail", new { id = plan.Id, subsystem });
            }
            var respSup = GetUnitId(responsibleUnitFilter);
            if (!plan.ParentId.HasValue || plan.ParentId.Value == Guid.Empty)
            {
                CommandStatus("Parent is required");
                return RedirectToAction("Header", new { planYear = plan.PlanYear });
            }
            var parent = await _repository.Plan(plan.ParentId.Value);
            plan.IsLabel = true;
            if (plan.Id == Guid.Empty)
            {
                plan.Id = Guid.NewGuid();
                plan.BasePlanId = plan.Id;
                plan.InitMonthlyPlan();
                var maxCode = parent.SubPlans.Max(p => p.InternalCode);
                maxCode = maxCode == null? "0": maxCode.Replace($"{parent.InternalCode}.", "");
                var nextCode = int.Parse(maxCode) + 1;
                maxCode = plan.IsLabel? $"0{nextCode}": $"00{nextCode}";
                maxCode = plan.IsLabel ? maxCode.Substring(maxCode.Length - 2) : maxCode.Substring(maxCode.Length - 3);
                plan.InternalCode = $"{parent.InternalCode}.{maxCode}";
            }

            plan.ResponsibleUnit =  respSup.ResponsibleUnit;
            plan.SupportingUnitId =  respSup.SupportingUnitId;
            plan.IsLeaf = plan.IsLeaf && respSup.ResponsibleUnit == ECCSAModuleEnum.None;
            plan.VersionNo = parent.VersionNo;
            try
            {

                if (plan.ParentId.HasValue && plan.ParentId.Value != Guid.Empty)
                {
                    
                    if (parent != null)
                    {
                        if (!plan.Code.Contains("."))
                        {
                            plan.Code = parent.Code.Trim() + "." + plan.Code.Trim();
                        }
                    }
                }
                await _repository.Save(plan, TheUser);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (Exception ex)
            {
                LogError(_logger, ex, controllerName, "CreateLabel", plan);
                TempData["CommandStatus"] = ex.Message;
            }

            return RedirectToAction("Header", new { planYear = plan.PlanYear });

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRoot(Plan plan, ECCSAModuleEnum subsystem)
        {
            LogStart(_logger, controllerName, "CreateLabel");
            var module = (ECCSAModuleFlag)(int)subsystem;
            var setting = await _repository.Configuration();
            if (setting.PlanningPhase == PlanningPhaseEnum.Closed)
            {
                CommandStatus("Planning is closed");
                return RedirectToAction("Header", new { planYear = plan.PlanYear });
            }
            if (setting.PlanningPhase == PlanningPhaseEnum.Unknown)
            {
                CommandStatus("Planning status is Unknown");
                return RedirectToAction("Header", new { planYear = plan.PlanYear });
            }
            var roleLevel = await GetRoleLevel(module);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "Detail");
                return RedirectToAction("Detail", new { id = plan.Id, subsystem });
            }
            var config = await _repository.Configuration();
            plan.IsLabel = true;
            if (plan.Id == Guid.Empty)
            {
                var rootPlans = await _repository.GetRootPlans(config.PlanYear);
                if (rootPlans == null || rootPlans.Count == 0)
                {
                    plan.InternalCode = "01";
                }
                else
                {
                    var maxCode = rootPlans.Max(p => p.InternalCode);
                    var nextCode = int.Parse(maxCode) + 1;
                    maxCode = $"0{nextCode}";
                    plan.InternalCode =maxCode.Substring(maxCode.Length - 2);
                }
                plan.Id = Guid.NewGuid();
                plan.BasePlanId = plan.Id;
                plan.InitMonthlyPlan();
            }
            try
            {
                
                plan.VersionNo = config.VersionNo;
                plan.PlanYear = config.PlanYear;
                if (plan.ParentId.HasValue && plan.ParentId.Value != Guid.Empty)
                {
                    var parent = await _repository.Plan(plan.ParentId.Value);
                    if (parent != null)
                    {
                        if (!plan.Code.Contains("."))
                        {
                            var code = $"000{plan.Code.Trim()}";
                            plan.Code = parent.Code.Trim() + "." + plan.Code.Trim();
                        }
                        plan.ResponsibleUnit = parent.ResponsibleUnit != ECCSAModuleEnum.None ? parent.ResponsibleUnit : plan.ResponsibleUnit;
                    }
                }
                await _repository.Save(plan, TheUser);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (Exception ex)
            {
                LogError(_logger, ex, controllerName, "CreateLabel", plan);
                TempData["CommandStatus"] = ex.Message;
            }

            return RedirectToAction("Header", new { planYear = plan.PlanYear });

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateLabel(Plan data, ECCSAModuleEnum subsystem, bool header = false)
        {
            LogStart(_logger, controllerName, "UpdateLabel");
            var module = (ECCSAModuleFlag)(int)subsystem;
            var setting = await _repository.Configuration();
            if (setting.PlanningPhase == PlanningPhaseEnum.Closed)
            {
                CommandStatus("Planning is closed");
                return RedirectToAction("Header", new { planYear = data.PlanYear });
            }
            if (setting.PlanningPhase == PlanningPhaseEnum.Unknown)
            {
                CommandStatus("Planning status is Unknown");
                return RedirectToAction("Header", new { planYear = data.PlanYear });
            }
            var roleLevel = await GetRoleLevel(module);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "Detail");
                return RedirectToAction("Index", new { subsystem });
            }
            try
            {
                var plan = await _repository.Plan(data.Id);
                plan.Code = data.Code;
                plan.Name = data.Name;
                await _repository.Save(plan, TheUser);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (Exception ex)
            {
                LogError(_logger, ex, controllerName, "UpdateLabel", data);
                TempData["CommandStatus"] = ex.Message;
            }
            if (header)
                return RedirectToAction("Header", new { planYear = data.PlanYear });
            return RedirectToAction("Index", new { subsystem });

        }

       
       
       
       
        public async Task<IActionResult> Detail(Guid Id)
        {
            LogStart(_logger, controllerName, "Detail");
            var plan = await _repository.Plan(Id);
            var subsystem = ECCSAModuleEnum.MandE;
            var module = (ECCSAModuleFlag)(int)subsystem;
            if (plan == null)
            {
                return RedirectToAction("Index", new { subsystem });
            }
            var userName = TheUser.FullName;
            var roleLevel = await GetRoleLevel(module);

            PlanViewModel model;
            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Detail");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            model = new PlanViewModel
            {
                TheModel = await _repository.Plan(Id),
                RoleLevel = roleLevel,
                CurrentModule = module,
                CurrentSubsystem = subsystem,
                CurrentPage = "Controlling",
                CurrentOperation = "Detail"
            };
            if (model.TheModel == null)
            {
                return RedirectToAction("Error", "Home", new { area = "" });
            }
            model.RoleLevel = roleLevel;

            model.ChangeActive(MenuItemEnum.ReviewPlan);
            TempData["title"] = model.CurrentPage;
            LogEnd(_logger, controllerName, "Detail");
            return View(model);
        }
    

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePlan(Guid id, int planYear, ECCSAModuleEnum subsystem)
        {
            //   ECCSAModuleEnum subsystem = ECCSAModuleEnum.MandE;
            LogStart(_logger, controllerName, "DeletePlan");
            var module = (ECCSAModuleFlag)(int)subsystem;
            var roleLevel = await GetRoleLevel(module);
            var setting = await _repository.Configuration();
            if (setting.PlanningPhase == PlanningPhaseEnum.Closed)
            {
                CommandStatus("Planning is closed");
                return RedirectToAction("Index" );
            }
            if (setting.PlanningPhase == PlanningPhaseEnum.Unknown)
            {
                CommandStatus("Planning status is Unknown");
                return RedirectToAction("Index");
            }
            if (roleLevel < RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "Index");
                return RedirectToAction("Index");
            }
            try
            {

                await _repository.DeletePlan(id);
                CommandStatus(CommandStatusEnum.Deleted);
            }
            catch (Exception ex)
            {
                LogError(_logger, ex, controllerName, "DeletePlan", id);
                TempData["CommandStatus"] = ex.Message;
            }

            return RedirectToAction("Header", new { planYear });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectPlan(Guid id
            , String remark
            , string responsibleUnitFilter,
      
        bool treeMode)
        {
            var setting = await _repository.Configuration();
            if (setting.PlanningPhase == PlanningPhaseEnum.Closed)
            {
                CommandStatus("Planning is closed");
                return RedirectToAction("Index", new { responsibleUnitFilter, treeMode });
            }
            if (setting.PlanningPhase == PlanningPhaseEnum.Unknown)
            {
                CommandStatus("Planning status is Unknown");
                return RedirectToAction("Index", new { responsibleUnitFilter, treeMode });
            }
            //   ECCSAModuleEnum subsystem = ECCSAModuleEnum.MandE;
            LogStart(_logger, controllerName, "RejectPlan");
            var module = (ECCSAModuleFlag)(int)ECCSAModuleEnum.MandE;
            var roleLevel = await GetRoleLevel(module);
            if (roleLevel < RoleLevelEnum.Admin)
            {
                LogSecurityError(_logger, controllerName, "Index");
                return RedirectToAction("Index");
            }
            try
            {
                await _repository.RejectPlan(new Plan
                {
                    Id = id,
                    Remark = remark
                }, TheUser);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (Exception ex)
            {
                LogError(_logger, ex, controllerName, "RejectPlan", id);
                TempData["CommandStatus"] = ex.Message;
            }

            return RedirectToAction("Index", new { responsibleUnitFilter,  treeMode });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApprovePlan(Guid id,
            string responsibleUnitFilter,
            bool treeMode)
        {
            //   ECCSAModuleEnum subsystem = ECCSAModuleEnum.MandE;
            LogStart(_logger, controllerName, "ApprovePlan");
            var module = (ECCSAModuleFlag)(int)ECCSAModuleEnum.MandE;
            var roleLevel = await GetRoleLevel(module);
            var setting = await _repository.Configuration();
            if (setting.PlanningPhase == PlanningPhaseEnum.Closed)
            {
                CommandStatus("Planning is closed");
                return RedirectToAction("Index", new { responsibleUnitFilter, treeMode });
            }
            if (setting.PlanningPhase == PlanningPhaseEnum.Unknown)
            {
                CommandStatus("Planning status is Unknown");
                return RedirectToAction("Index", new { responsibleUnitFilter, treeMode });
            }
            if (roleLevel < RoleLevelEnum.Admin)
            {
                LogSecurityError(_logger, controllerName, "Index");
                return RedirectToAction("Index", new { responsibleUnitFilter, treeMode });
            }
            try
            {
                await _repository.ApprovePlan(new Plan
                {
                    Id = id,
                    Remark = string.Empty,
                },setting.PlanYear, TheUser);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (Exception ex)
            {
                LogError(_logger, ex, controllerName, "ApprovePlan", id);
                TempData["CommandStatus"] = ex.Message;
            }

            return RedirectToAction("Index", new {  responsibleUnitFilter,  treeMode });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveAllPlan(string responsibleUnitFilter,
 
            bool treeMode)
        {
            //   ECCSAModuleEnum subsystem = ECCSAModuleEnum.MandE;
            LogStart(_logger, controllerName, "ApproveAllPlan");
            var module = (ECCSAModuleFlag)(int)ECCSAModuleEnum.MandE;
            var roleLevel = await GetRoleLevel(module);
            var setting = await _repository.Configuration();
            if (setting.PlanningPhase == PlanningPhaseEnum.Closed)
            {
                CommandStatus("Planning is closed");
                return RedirectToAction("Index", new { responsibleUnitFilter, treeMode });
            }
            if (setting.PlanningPhase == PlanningPhaseEnum.Unknown)
            {
                CommandStatus("Planning status is Unknown");
                return RedirectToAction("Index", new { responsibleUnitFilter, treeMode });
            }
            if (roleLevel < RoleLevelEnum.Admin)
            {
                LogSecurityError(_logger, controllerName, "Index");
                return RedirectToAction("Index", new { responsibleUnitFilter, treeMode });
            }
            if (roleLevel < RoleLevelEnum.Admin)
            {
                LogSecurityError(_logger, controllerName, "Index");
                return RedirectToAction("Index");
            }
            try
            {
                await _repository.ApproveAllPlan(new Plan
                {
                    
                    Remark = string.Empty,
                }, setting.PlanYear, TheUser);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (Exception ex)
            {
                LogError(_logger, ex, controllerName, "ApproveAllPlan", responsibleUnitFilter);
                TempData["CommandStatus"] = ex.Message;
            }

            return RedirectToAction("Index", new {  responsibleUnitFilter, treeMode });

        }


        public async Task<IActionResult> Progress(
            string responsibleUnitFilter,
            bool treeMode)
        {
            var module = ECCSAModuleFlag.MandE;
            var setting = await _repository.Configuration();
            var monthNo = setting?.CurrentMonth ?? 0;
            var yearNo = setting?.CurrentYear ?? 0;
            var roleLevel = await GetRoleLevel(module);
            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Index");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            var respSupp = GetUnitId(responsibleUnitFilter);
            var responsibleUnit = respSupp.ResponsibleUnit;
            var supportingUnit = respSupp.SupportingUnitId;

            var list = await _repository.Performance();
            if (responsibleUnit > 0)
            {
                list = list.Where(l => (l.IsLabel && l.ResponsibleUnit == ECCSAModuleEnum.None) || (l.SupportingUnitId == supportingUnit && l.ResponsibleUnit == responsibleUnit))
                    .ToList();
            }
            if(monthNo > 0 && yearNo > 0)
            {
                list = list.Where(l => l.HasPlan(monthNo)).ToList();
            }

            if (!treeMode)
            {

                list = list.Where(l => !l.IsLabel)
                    .ToList();
            }

            var model = new ProgressViewModel()
            {
                TheModel = new Plan(),
                TheList = list,
                FullName = TheUser.FullName,
                PlanYear = setting.PlanYear,
                SupportingUnitId = supportingUnit,
                SelectedResponsibleUnit = (ECCSAModuleEnum)responsibleUnit,
                CurrentPage = "Progress Approval",
                ResponsibleUnitFilter = responsibleUnitFilter,
                TreeView = treeMode,
                PlanVersionNo = setting.PlanVersionNo,
                PlanPhase = setting.PlanningPhase,
                CurrentMonth = setting.CurrentMonth,
                CurrentYear = setting.CurrentYear,
                CurrentOperation = "Index"
            };
            model.SupportingUnits = (await _lookupRepository.LookupTypeById(SupportingUnitLookupTypeId))?.LookupValues.ToList();
            model.RoleLevel = roleLevel;
            model.ChangeActive(MenuItemEnum.ReviewProgress);
            model.CurrentModule = module;
            model.CurrentSubsystem = ECCSAModuleEnum.MandE;
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectProgress(Guid id
         , String remark
         , string responsibleUnitFilter,

     bool treeMode)
        {
            var setting = await _repository.Configuration();
           
            //   ECCSAModuleEnum subsystem = ECCSAModuleEnum.MandE;
            LogStart(_logger, controllerName, "RejectProgress");
            var module = (ECCSAModuleFlag)(int)ECCSAModuleEnum.MandE;
            var roleLevel = await GetRoleLevel(module);
            if (roleLevel < RoleLevelEnum.Admin)
            {
                LogSecurityError(_logger, controllerName, "Progress");
                return RedirectToAction("Progress");
            }
            try
            {
                await _repository.RejectProgress(new MonthlyPlan
                {
                    PlanId = id,
                    Remark = remark
                }, TheUser);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (Exception ex)
            {
                LogError(_logger, ex, controllerName, "RejectProgress", id);
                TempData["CommandStatus"] = ex.Message;
            }

            return RedirectToAction("Progress", new { responsibleUnitFilter, treeMode });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveProgress(Guid id,
            string responsibleUnitFilter,
            bool treeMode)
        {
            //   ECCSAModuleEnum subsystem = ECCSAModuleEnum.MandE;
            LogStart(_logger, controllerName, "ApprovePlan");
            var module = (ECCSAModuleFlag)(int)ECCSAModuleEnum.MandE;
            var roleLevel = await GetRoleLevel(module);
            var setting = await _repository.Configuration();
          
            if (roleLevel < RoleLevelEnum.Admin)
            {
                LogSecurityError(_logger, controllerName, "Index");
                return RedirectToAction("Index", new { responsibleUnitFilter, treeMode });
            }
            try
            {
                await _repository.ApproveProgress(new MonthlyPlan
                {
                    PlanId = id,
                    MonthNo = setting.CurrentMonth,
                    Remark = string.Empty,
                }, setting.PlanYear, TheUser);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (Exception ex)
            {
                LogError(_logger, ex, controllerName, "ApprovePlan", id);
                TempData["CommandStatus"] = ex.Message;
            }

            return RedirectToAction("Progress", new { responsibleUnitFilter, treeMode });

        }

    }
}
