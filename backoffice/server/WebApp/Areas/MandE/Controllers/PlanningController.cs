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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Model.Common.Planning;
using NPOI.POIFS.Properties;
using System.Numerics;
using DocumentFormat.OpenXml.Vml.Spreadsheet;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Notification;
using Microsoft.AspNetCore.Http;
using System.Net.Http;

namespace ECCSA.BackOffice.Server.WebApp.Areas.MandE.Controllers
{
    [Area("mande")]
    public class PlanningController : BaseController
    {

        private readonly ILogger<PlanningController> _logger;
        private readonly MandERepository _repository;
        private readonly YesNoRepository _lookupRepository;
        public PlanningController(ILogger<PlanningController> logger,
        UserManager<User> userManager,
        MandERepository repository,
        PublicUserRepository userRepository,
        YesNoRepository lookupRepository
        , NotificationRepository notificationRepository) : base(userManager, userRepository, notificationRepository)
        {

            _logger = logger;
            _repository = repository;
            controllerName = "Plan";
            _lookupRepository = lookupRepository;
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
                ECCSAModuleFlag.Support => "Support M&E",
                _ => string.Empty,
            };
        }

        
        
        public async Task<IActionResult> Index(
            ECCSAModuleEnum subSystem,
       
            bool treeMode)
        {
            
            var module = (ECCSAModuleFlag)(int)subSystem;
            var setting = await _repository.Configuration();
            var roleLevel = await GetRoleLevel(module);
            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Index");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            
            var model = new PlanViewModel
            {
                TheModel = new Plan(),
                TheList = new List<Plan>(),
                FullName = TheUser.FullName,
                PlanYear = setting.PlanYear,
                PlanPhase = setting.PlanningPhase,
                PlanVersionNo = setting.PlanVersionNo,
                SupportingUnitId = TheUser.DepartmentId ?? 0,
                TreeView = treeMode,
                TheUser = TheUser,
  
            };
            model.RoleLevel = roleLevel;
            model.ChangeActive(MenuItemEnum.Plan);
            model.CurrentModule = module;
            model.CurrentSubsystem = subSystem;
            if (subSystem == ECCSAModuleEnum.None)
                return View(model);

            var list = await _repository.Plans();
            var supportId = TheUser.DepartmentId ?? 0;
            list = list.Where(l => (l.ResponsibleUnit == ECCSAModuleEnum.None && l.IsLabel)
                                   ||
                                   (l.ResponsibleUnit == subSystem &&
                                    l.SupportingUnitId == supportId)).ToList();
            if (list.Any(l => !l.IsLabel))
            {
                model.TheList = treeMode ? list : list.Where(l => !l.IsLabel).ToList();
            }
            else
            {
                model.TheList = list;
                model.TreeView = true;
            }

            model.SupportingUnits = (await _lookupRepository.LookupTypeById(SupportingUnitLookupTypeId))?.LookupValues.ToList();
            model.FinancialItems = (await _repository.FinancialItems()).Where(f=> !f.IsLabel && f.Status == StatusEnum.Active).ToList();
            return View(model);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateLabel(Plan plan, ECCSAModuleEnum subsystem)
        {
            LogStart(_logger, controllerName, "CreateLabel");
            var module = (ECCSAModuleFlag)(int)subsystem;
            var setting = await _repository.Configuration();
            if (setting.PlanningPhase == PlanningPhaseEnum.Closed)
            {
                CommandStatus("Planning is closed");
                return RedirectToAction("Index", new { subsystem, treeMode = true });
            }
            if (setting.PlanningPhase == PlanningPhaseEnum.Unknown)
            {
                CommandStatus("Planning status is Unknown");
                return RedirectToAction("Index", new { subsystem, treeMode = true });
            }
            var roleLevel = await GetRoleLevel(module);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "Detail");
                return RedirectToAction("Detail", new { id = plan.Id, subsystem });
            }
            plan.IsLabel = true;
            if (!plan.ParentId.HasValue || plan.ParentId.Value == Guid.Empty)
            {
                CommandStatus("Parent is required");
                return RedirectToAction("Index", new { planYear = plan.PlanYear });
            }
            var parent = await _repository.Plan(plan.ParentId.Value);
            await CreateUpdatePlan(plan,parent);
            try
            {

               
                await _repository.Save(plan, TheUser);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (Exception ex)
            {
                LogError(_logger, ex, controllerName, "CreateLabel", plan);
                TempData["CommandStatus"] = ex.Message;
            }

            return RedirectToAction("Index", new { subsystem, draft = true, rejected=false,submitted = false,approved=false, completed = false, treeMode = true });

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateLabel(Plan data, ECCSAModuleEnum subsystem)
        {
            LogStart(_logger, controllerName, "UpdateLabel");
            var module = (ECCSAModuleFlag)(int)subsystem;
            var setting = await _repository.Configuration();
            if (setting.PlanningPhase == PlanningPhaseEnum.Closed)
            {
                CommandStatus("Planning is closed");
                return RedirectToAction("Index", new { subsystem, treeMode = true });
            }
            if (setting.PlanningPhase == PlanningPhaseEnum.Unknown)
            {
                CommandStatus("Planning status is Unknown");
                return RedirectToAction("Index", new { subsystem, treeMode = true });
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
                await CreateUpdatePlan(plan,plan.ParentPlan);
                await _repository.Save(plan, TheUser);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (Exception ex)
            {
                LogError(_logger, ex, controllerName, "UpdateLabel", data);
                TempData["CommandStatus"] = ex.Message;
            }
            return RedirectToAction("Index", new { subsystem, draft = true, rejected = false, submitted = false, approved = false, completed = false, treeMode = true });

        }


        private async Task<Plan> CreateUpdatePlan(Plan plan, Plan parentPlan)
        {
            var config = await _repository.Configuration();
            var parent = await _repository.Plan(plan.ParentId.Value);
           
            if (plan.Id == Guid.Empty)
            {
                
                plan.InitMonthlyPlan();
                var maxCode = parent.SubPlans.Max(p => p.InternalCode);
                maxCode = maxCode == null ? "0" : maxCode.Replace($"{parent.InternalCode}.", "");
                var nextCode = int.Parse(maxCode) + 1;
                maxCode = plan.IsLabel ? $"0{nextCode}" : $"00{nextCode}";
                maxCode = plan.IsLabel ? maxCode.Substring(maxCode.Length - 2) : maxCode.Substring(maxCode.Length - 3);
                plan.InternalCode = $"{parent.InternalCode}.{maxCode}";
                plan.VersionNo = config.VersionNo;
               // plan.IsLeaf = true;
                plan.Id = Guid.NewGuid();
                plan.BasePlanId = plan.Id;
            }
            if (!plan.Code.Contains("."))
            {
                plan.Code = parent.Code + "." + plan.Code;
            }
            plan.ResponsibleUnit = parent.ResponsibleUnit != ECCSAModuleEnum.None ? parent.ResponsibleUnit : plan.ResponsibleUnit;
            plan.SupportingUnitId = plan.ResponsibleUnit != ECCSAModuleEnum.Support? 0: parent.SupportingUnitId > 0 ? parent.SupportingUnitId : plan.SupportingUnitId;

            return plan;
        }
      

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveMainPlan(Plan plan, ECCSAModuleEnum subsystem)
        {
            LogStart(_logger, controllerName, "SaveMainPlan");
            var module = (ECCSAModuleFlag)(int)subsystem;
            var roleLevel = await GetRoleLevel(module);
            var setting = await _repository.Configuration();
            if (setting.PlanningPhase == PlanningPhaseEnum.Closed)
            {
                CommandStatus("Planning is closed");
                return RedirectToAction("Index", new { subsystem, treeMode = true });
            }
            if (setting.PlanningPhase == PlanningPhaseEnum.Unknown)
            {
                CommandStatus("Planning status is Unknown");
                return RedirectToAction("Index", new { subsystem, treeMode = true });
            }
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "Detail");
                return RedirectToAction("Detail", new { id = plan.Id, subsystem });
            }
            if (!plan.ParentId.HasValue || plan.ParentId.Value == Guid.Empty)
            {
                TempData["CommandStatus"] = "Parent Indicator must be selected";
                return RedirectToAction("Index", new { subsystem,  treeMode = true });
            }
            plan.IsLabel = false;
            var isNew = false;
            var parent = await _repository.Plan(plan.ParentId.Value);
            if (plan.Id == Guid.Empty)
            {
                plan.SupportingUnitId = TheUser.DepartmentId ?? 0;
                plan.ResponsibleUnit = subsystem;
                plan = await CreateUpdatePlan(plan, parent);
                isNew = true;
            }

            try
            {


                if (!isNew)
                {
                    var currentDetail = await _repository.Plan(plan.Id);
                    plan.Status = currentDetail.Status == PlanningStatusEnum.Rejected ? PlanningStatusEnum.Planning : currentDetail.Status;
                    currentDetail.Status = currentDetail.Status == PlanningStatusEnum.Rejected ? PlanningStatusEnum.Planning : currentDetail.Status;

                    plan.PlanYear = currentDetail.PlanYear;
                    plan.ResponsibleUnit = currentDetail.ResponsibleUnit;
                    plan.SupportingUnitId = currentDetail.SupportingUnitId;
                    plan.BasePlanId = currentDetail.BasePlanId;
                    plan.Month01 = currentDetail.Month01;
                    plan.Month02 = currentDetail.Month02;
                    plan.Month03 = currentDetail.Month03;
                    plan.Month04 = currentDetail.Month04;
                    plan.Month05 = currentDetail.Month05;
                    plan.Month06 = currentDetail.Month06;
                    plan.Month07 = currentDetail.Month07;
                    plan.Month08 = currentDetail.Month08;
                    plan.Month09 = currentDetail.Month09;
                    plan.Month10 = currentDetail.Month10;
                    plan.Month11 = currentDetail.Month11;
                    plan.Month12 = currentDetail.Month12;
                    plan.Quarter1Remark = currentDetail.Quarter1Remark;
                    plan.Quarter2Remark = currentDetail.Quarter2Remark;
                    plan.Quarter3Remark = currentDetail.Quarter3Remark;
                    plan.Quarter4Remark = currentDetail.Quarter4Remark;
                }

                await _repository.Save(plan, TheUser);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (Exception ex)
            {
                LogError(_logger, ex, controllerName, "SaveMainPlan", plan);
                TempData["CommandStatus"] = ex.Message;
            }

            return RedirectToAction("Detail", new { id = plan.Id, subsystem });

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SavePlan(Plan model, ECCSAModuleEnum subsystem)
        {
            LogStart(_logger, controllerName, "SavePlan");
            var module = (ECCSAModuleFlag)(int)subsystem;
            var roleLevel = await GetRoleLevel(module);
            var setting = await _repository.Configuration();
            if (setting.PlanningPhase == PlanningPhaseEnum.Closed )
            {
                CommandStatus("Planning is closed");
                return RedirectToAction("Detail", new { id = model.Id, subsystem });
            }
            if (setting.PlanningPhase == PlanningPhaseEnum.Unknown)
            {
                CommandStatus("Planning status is Unknown");
                return RedirectToAction("Detail", new { id = model.Id, subsystem });
            }
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "Detail");
                return RedirectToAction("Detail", new { id = model.Id, subsystem });
            }
            if (model.PlanAmount != model.TotalPlanAmount)
            {
                TempData["CommandStatus"] = $"Monthly Plan Amount {model.TotalPlanAmount} does not add up to Annual Target {model.PlanAmount}";
                return RedirectToAction("Detail", new { id = model.Id, subsystem });
            }
            try
            {
                var plan = await _repository.Plan(model.Id);
                plan.Status = plan.Status == PlanningStatusEnum.Rejected ? PlanningStatusEnum.Planning: plan.Status;
                plan.Month01 = model.Month01;
                plan.Month02 = model.Month02;
                plan.Month03 = model.Month03;
                plan.Month04 = model.Month04;
                plan.Month05 = model.Month05;
                plan.Month06 = model.Month06;
                plan.Month07 = model.Month07;
                plan.Month08 = model.Month08;
                plan.Month09 = model.Month09;
                plan.Month10 = model.Month10;
                plan.Month11 = model.Month11;
                plan.Month12 = model.Month12;
                await _repository.Save(plan, TheUser);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (Exception ex)
            {
                LogError(_logger, ex, controllerName, "SavePlan", model);
                TempData["CommandStatus"] = ex.Message;
            }

            return RedirectToAction("Detail", new { id = model.Id, subsystem });

        }

        public async Task<IActionResult> Detail(Guid Id, ECCSAModuleEnum subsystem = ECCSAModuleEnum.None)
        {
            LogStart(_logger, controllerName, "Detail");
            var plan = await _repository.Plan(Id);
            var module = (ECCSAModuleFlag)(int)subsystem;
            var setting = await _repository.Configuration();
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

                TheUser = TheUser,
                TheModel = await _repository.Plan(Id),
                RoleLevel = roleLevel,
                CurrentModule = module,
                CurrentSubsystem = subsystem,
                PlanPhase = setting.PlanningPhase
            };
            model.RoleLevel = roleLevel;
            if (model.TheModel == null)
            {
                return RedirectToAction("Error", "Home", new { area = "" });
            }
            model.ChangeActive(MenuItemEnum.Plan);
            TempData["title"] = model.CurrentPage;
            LogEnd(_logger, controllerName, "Detail");
            return View(model);
        }
        public async Task<IActionResult> EditPlan(Guid id)
        {
            LogStart(_logger, controllerName, "EditPlan");



            var plan = new Plan()
            {
                Id = Guid.Empty,
                ResponsibleUnit = (ECCSAModuleEnum)(int)TheUser.DefaultModule
            };
            var module = ECCSAModuleFlag.MandE;
            var userName = TheUser.FullName;
            var roleLevel = await GetRoleLevel(module);

            var model = new PlanViewModel
            {


                TheModel = plan
            };

            if (id == Guid.Empty)
            {


                if (plan == null)
                {
                    return RedirectToAction("Index", new { module });
                }

                model = new PlanViewModel
                {


                    TheModel = plan
                };

            }
            else
            {
                model.TheModel = await _repository.Plan(id);

            }



            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePlan(Guid id, int planYear, ECCSAModuleEnum subsystem)
        {
            //   ECCSAModuleEnum subsystem = ECCSAModuleEnum.MandE;
            LogStart(_logger, controllerName, "DeletePlan");
            var module = (ECCSAModuleFlag)(int)subsystem;
            var setting = await _repository.Configuration();
            if (setting.PlanningPhase == PlanningPhaseEnum.Closed)
            {
                CommandStatus("Planning is closed");
                return RedirectToAction("Index", new { subsystem, treeMode = true });
            }
            if (setting.PlanningPhase == PlanningPhaseEnum.Unknown)
            {
                CommandStatus("Planning status is Unknown");
                return RedirectToAction("Index", new { subsystem, treeMode = true });
            }
            var roleLevel = await GetRoleLevel(module);
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

            return RedirectToAction("Index", new { subsystem,treeMode = true });

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePlanModule(Guid id, int planYear, ECCSAModuleEnum subsystem)
        {
            //   ECCSAModuleEnum subsystem = ECCSAModuleEnum.MandE;
            LogStart(_logger, controllerName, "DeletePlan");
            var module = (ECCSAModuleFlag)(int)subsystem;
            var setting = await _repository.Configuration();
            if (setting.PlanningPhase == PlanningPhaseEnum.Closed)
            {
                CommandStatus("Planning is closed");
                return RedirectToAction("Index", new { subsystem, treeMode = true });
            }
            if (setting.PlanningPhase == PlanningPhaseEnum.Unknown)
            {
                CommandStatus("Planning status is Unknown");
                return RedirectToAction("Index", new { subsystem, treeMode = true });
            }
            var roleLevel = await GetRoleLevel(module);
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

            return RedirectToAction("Index", new { subsystem, draft = true, rejected = false, submitted = false, approved = false, completed = false, treeMode = true });

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitPlans(int planYear, ECCSAModuleEnum subsystem)
        {
            var setting = await _repository.Configuration();
            if (setting.PlanningPhase == PlanningPhaseEnum.Closed)
            {
                CommandStatus("Planning is closed");
                return RedirectToAction("Index", new { subsystem, treeMode = true });
            }
            if (setting.PlanningPhase == PlanningPhaseEnum.Unknown)
            {
                CommandStatus("Planning status is Unknown");
                return RedirectToAction("Index", new { subsystem, treeMode = true });
            }
            try
            {
                var data = new Plan { ResponsibleUnit = subsystem, SupportingUnitId = TheUser.DepartmentId ?? 0 };
                await _repository.SubmitPlan(data, planYear, TheUser);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (Exception ex)
            {
                LogError(_logger, ex, controllerName, "SubmitPlan", new { planYear, subsystem });
                TempData["CommandStatus"] = ex.Message;
            }

            return RedirectToAction("Index", new { subsystem, draft = false, rejected = false, submitted = true, approved = false, completed = false, treeMode = false });
        }




    }
}
