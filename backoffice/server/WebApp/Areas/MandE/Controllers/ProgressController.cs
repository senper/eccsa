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
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Model.Common.Planning;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using ClosedXML.Excel;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Notification;
using Npgsql.EntityFrameworkCore.PostgreSQL.Storage.Internal.Mapping;
using ECCSA.BackOffice.Server.Persistence.Repositories.Membership;
using ECCSA.BackOffice.Server.WebApp.Areas.Membership.Model;
using CCSA.BackOffice.Server.WebApp.Areas.MandE.Model;

namespace ECCSA.BackOffice.Server.WebApp.Areas.MandE.Controllers
{
    [Area("mande")]
    public class ProgressController : BaseController
    {
        private readonly ILogger<ProgressController> _logger;
        private readonly MandERepository _repository;
        private readonly YesNoRepository _lookupRepository;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public ProgressController(ILogger<ProgressController> logger, UserManager<User> userManager, MandERepository repository, PublicUserRepository userRepository, IWebHostEnvironment hostingEnvironment, YesNoRepository lookupRepository, NotificationRepository notificationRepository) : base(userManager, userRepository, notificationRepository)
        {

            _logger = logger;
            _repository = repository;
            controllerName = "Progress";
            _hostingEnvironment = hostingEnvironment;

            _lookupRepository = lookupRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index(
            ECCSAModuleEnum subSystem)
        {
            var module = (ECCSAModuleFlag)(int)subSystem;

            var roleLevel = await GetRoleLevel(module);
            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Index");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            ProgressViewModel model;
            var setting = await _repository.Configuration();
            var monthNo = setting?.CurrentMonth ?? 0;
            var yearNo = setting?.CurrentYear ?? 0;
            if (setting == null || setting.CurrentYear == 0 || setting.CurrentMonth == 0)
            {
                model = new ProgressViewModel
                {
                    TheModel = new Plan(),
                    TheList = new List<Plan>(),
                    FullName = TheUser.FullName,
                    CurrentModule = module,
                    CurrentSubsystem = subSystem,
                    CurrentMonth = monthNo,
                    CurrentYear = yearNo,
                };
                return View(model);
            }
            var list = await _repository.GetMonitoring();
            var reportingUnit = ReportingUnit.List.FirstOrDefault(r => r.Id == (int)subSystem || r.Units.Any(u => u == subSystem));
            var supportingUnitId = TheUser.DepartmentId ?? 0;
            var unplanned = reportingUnit != null ? await _repository.GetUnplannedActivity(yearNo, monthNo, reportingUnit.Id, supportingUnitId) : null;
            list = list.Where(l => l.SupportingUnitId == supportingUnitId && (l.ResponsibleUnit == subSystem) && l.HasPlan(setting.CurrentMonth) && !l.IsLabel).ToList();

            model = new ProgressViewModel
            {
                TheModel = new Plan(),
                TheList = list.OrderBy(s => s.Code).ToList(),
                FullName = TheUser.FullName,
                CurrentModule = module,
                CurrentSubsystem = subSystem,
                CurrentMonth = monthNo,
                CurrentYear = yearNo,
                RoleLevel = roleLevel,
                CurrentActivity = unplanned,
                SupportingUnitId = supportingUnitId,
                SelectedUnit = supportingUnitId
            };

            model.ChangeActive(MenuItemEnum.Progress);
            return View(model);
        }
        private (ECCSAModuleEnum ResponsibleUnit, int SupportingUnitId) GetUnitId(string responsibleUnitFilter)
        {
            var resUnitFilterList = responsibleUnitFilter == null ? new string[] { "0", "0" } :
                responsibleUnitFilter.Split("_");
            var responsibleUnit = (ECCSAModuleEnum)(resUnitFilterList.Length > 0 && int.TryParse(resUnitFilterList[0], out int val1) ? val1 : 0);
            var supportingUnit = resUnitFilterList.Length > 1 && int.TryParse(resUnitFilterList[1], out int val) ? val : 0;
            return (responsibleUnit, supportingUnit);

        }

        [HttpGet]
        public async Task<IActionResult> Report(
              int selectedYear
            , string selectedPeriod
            , string responsibleUnitFilter
            , int selectedScore
            , bool treeMode
            , ECCSAModuleEnum subSystem)
        {
            var module = (ECCSAModuleFlag)(int)subSystem;

            var roleLevel = await GetRoleLevel(module);
            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Index");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            var respSupp = GetUnitId(responsibleUnitFilter);
            var responsibleUnit = respSupp.ResponsibleUnit;
            var supportingUnit = respSupp.SupportingUnitId;
            if (subSystem != ECCSAModuleEnum.MandE)
            {
                responsibleUnit = subSystem;
                supportingUnit = TheUser.DepartmentId ?? 0;
            }
            var supportingUnits = (await _lookupRepository.LookupTypeById(SupportingUnitLookupTypeId))?.LookupValues.ToList()
                ?? new List<LookupValue>();
            ProgressViewModel model;
            var setting = await _repository.Configuration();
            selectedYear = selectedYear == 0 ? setting.CurrentYear : selectedYear;
            var versionNo = 1;
            var monthNo = setting != null ? setting.CurrentMonth : 0;
            var yearNo = setting != null ? setting.CurrentYear : 0;
            if (string.IsNullOrEmpty(selectedPeriod))
                selectedPeriod = monthNo.ToString();
            if (setting == null || setting.CurrentYear == 0 || setting.CurrentMonth == 0)
            {
                model = new ProgressViewModel
                {
                    TheModel = new Plan(),
                    TheList = new List<Plan>(),
                    FullName = TheUser.FullName,
                    CurrentModule = module,
                    CurrentSubsystem = subSystem,
                    CurrentMonth = monthNo,
                    CurrentYear = yearNo,
                    SelectedYear = selectedYear,
                    SelectedPeriod = selectedPeriod,
                    ResponsibleUnitFilter = responsibleUnitFilter,
                    SelectedScore = selectedScore,
                    TreeView = treeMode,
                    PlanVersionNo = versionNo
                };
                model.ChangeActive(MenuItemEnum.ProgressReport);
                model.Config = setting;
                model.SupportingUnits = supportingUnits;
                return View(model);
            }

            var list = await _repository.Performance(selectedYear, versionNo);
            var reportingUnit = ReportingUnit.List.FirstOrDefault(r => r.Id == (int)subSystem);
            var unplanned = reportingUnit != null ? await _repository.GetUnplannedActivity(yearNo, monthNo, reportingUnit.Id, TheUser.DepartmentId) : new UnplannedActivity();
           
            if (responsibleUnit > 0)
            {
                list = list.Where(l => (l.IsLabel && l.ResponsibleUnit == ECCSAModuleEnum.None)
                                       || (l.SupportingUnitId == supportingUnit && l.ResponsibleUnit == responsibleUnit))
                    .ToList();
            }
            if (!treeMode)
            {
                list = list.Where(l => !l.IsLabel).ToList();
            }

            if (selectedPeriod.StartsWith("Q"))
            {
                var qno = 0;
                int.TryParse(selectedPeriod.Replace("Q", ""), out qno);

                foreach (var plan in list)
                {
                    decimal totalActual = 0;
                    for (var i = 1; i <= qno; i++)
                    {
                        totalActual += plan.QuarterActual(i * 3, setting.CurrentMonth);
                    }
                    var qPlan = plan.QuarterPlan(qno * 3, setting.CurrentMonth);
                    var qActual = plan.QuarterActual(qno * 3, setting.CurrentMonth);
                    if (qPlan == qActual && qPlan == decimal.Zero)
                    {
                        plan.ReportedPlan = string.Empty;
                        plan.ReportedActual = string.Empty;
                        plan.ReportedProgress = string.Empty;
                        plan.ReportedYtDActual = string.Empty;
                    }
                    else
                    {
                        plan.ReportedPlan = qPlan.ToString(CultureInfo.InvariantCulture);
                        plan.ReportedActual = qActual.ToString(CultureInfo.InvariantCulture);
                        plan.ReportedProgress = (qActual * 100 / qPlan).ToString("N");
                        plan.ReportedYtDActual = $" | {totalActual}";
                    }

                    plan.ReportedPerformance = plan.ReportedPlan == string.Empty
                        ? String.Empty
                        : $"{plan.ReportedPlan} | {plan.ReportedActual} | {plan.ReportedProgress}%";
                    plan.ReportedOutput = plan.QuarterOutput(qno * 3);
                }
            }
            else
            {
                var mNo = 0;
                int.TryParse(selectedPeriod, out mNo);
                // list = list.Where(l => l.IsLabel || l.HasPlan(mNo)).ToList();
                foreach (var plan in list)
                {
                    var pmno = plan.PlanMonth(mNo);
                    var pl = pmno.Plan;
                    var act = pmno.Actual;
                    decimal totalActual = 0;
                    for (var i = 1; i <= mNo; i++)
                    {
                        totalActual += plan.PlanMonth(i).Actual;
                    }
                    if (pl == act && pl == decimal.Zero)
                    {
                        plan.ReportedPlan = string.Empty;
                        plan.ReportedActual = string.Empty;
                        plan.ReportedProgress = string.Empty;
                        plan.ReportedYtDActual = string.Empty;
                    }
                    else
                    {
                        plan.ReportedPlan = pmno.Plan.ToString(CultureInfo.InvariantCulture);
                        plan.ReportedActual = pmno.Actual.ToString(CultureInfo.InvariantCulture);
                        plan.ReportedProgress = (pmno.Actual * 100 / pmno.Plan).ToString("N");
                        plan.ReportedYtDActual = $" | {totalActual}";
                    }
                    plan.ReportedPerformance = plan.ReportedPlan == string.Empty
                        ? String.Empty
                        : $"{plan.ReportedPlan} | {plan.ReportedActual} | {plan.ReportedProgress}%";
                    plan.ReportedOutput = string.IsNullOrEmpty(pmno.CumulativeWorkDone)? pmno.Output : pmno.CumulativeWorkDone;
                }

            }

            var newList = list.Where(l => l.IsLabel || (l.ReportedPlan.Length > 0 && (l.ReportedProgressRange == "" || selectedScore == 0 || l.ReportedProgressRange == selectedScore.ToString()))).ToList();


            model = new ProgressViewModel
            {
                TheModel = new Plan(),
                TheList = newList,
                FullName = TheUser.FullName,
                CurrentModule = module,
                CurrentSubsystem = subSystem,
                CurrentMonth = monthNo,
                CurrentYear = yearNo,
                RoleLevel = roleLevel,
                SelectedPeriod = selectedPeriod,
                ResponsibleUnitFilter = responsibleUnitFilter,
                SelectedScore = selectedScore,
                SelectedYear = selectedYear,
                TreeView = treeMode,
                UnplannedActivity = unplanned
            };
            model.ScoreCards = PrepareScoreCards(setting, list); ;
            model.ChangeActive(MenuItemEnum.ProgressReport);
            model.Config = setting;
            model.SupportingUnits = supportingUnits;
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AggregateReport(
             int selectedYear
           , string selectedPeriod
           , string responsibleUnitFilter
           , int selectedScore
           , bool treeMode
           , ECCSAModuleEnum subSystem)
        {
            var module = (ECCSAModuleFlag)(int)subSystem;

            var roleLevel = await GetRoleLevel(module);
            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Index");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            var respSupp = GetUnitId(responsibleUnitFilter);
            var responsibleUnit = respSupp.ResponsibleUnit;
            var supportingUnit = respSupp.SupportingUnitId;
            if (subSystem != ECCSAModuleEnum.MandE)
            {
                responsibleUnit = subSystem;
                supportingUnit = TheUser.DepartmentId ?? 0;
            }
            var supportingUnits = (await _lookupRepository.LookupTypeById(SupportingUnitLookupTypeId))?.LookupValues.ToList()
                ?? new List<LookupValue>();
            ProgressViewModel model = null;
            var setting = await _repository.Configuration();
            selectedYear = selectedYear == 0 ? setting.CurrentYear : selectedYear;
            var versionNo = 1;
            var monthNo = setting != null ? setting.CurrentMonth : 0;
            var yearNo = setting != null ? setting.CurrentYear : 0;
            if (string.IsNullOrEmpty(selectedPeriod))
                selectedPeriod = monthNo.ToString();
            if (setting == null || setting.CurrentYear == 0 || setting.CurrentMonth == 0)
            {
                model = new ProgressViewModel
                {
                    TheModel = new Plan(),
                    TheList = new List<Plan>(),
                    FullName = TheUser.FullName,
                    CurrentModule = module,
                    CurrentSubsystem = subSystem,
                    CurrentMonth = monthNo,
                    CurrentYear = yearNo,
                    SelectedYear = selectedYear,
                    SelectedPeriod = selectedPeriod,
                    ResponsibleUnitFilter = responsibleUnitFilter,
                    SelectedScore = selectedScore,
                    TreeView = treeMode,
                    PlanVersionNo = versionNo,
                    
                };
                model.ChangeActive(MenuItemEnum.AggregateReport);
                model.Config = setting;
                model.SupportingUnits = supportingUnits;
                return View(model);
            }

            model = await GetAggregateReport(selectedYear, selectedPeriod, responsibleUnitFilter, selectedScore,
                treeMode, subSystem, module, roleLevel, responsibleUnit, supportingUnit, supportingUnits,
                model, setting, versionNo, monthNo, yearNo);
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> ExportAggregateReport(int selectedYear
           , string selectedPeriod
           , string responsibleUnitFilter
           , int selectedScore
           , bool treeMode
           , ECCSAModuleEnum subSystem)
        {
            try
            {
                #region ModelGET
                var module = (ECCSAModuleFlag)(int)subSystem;

                var roleLevel = await GetRoleLevel(module);
                if (roleLevel <= RoleLevelEnum.None)
                {
                    LogSecurityError(_logger, controllerName, "Index");
                    return RedirectToAction("Login", "Account", new { area = "Identity" });
                }
                var respSupp = GetUnitId(responsibleUnitFilter);
                var responsibleUnit = respSupp.ResponsibleUnit;
                var supportingUnit = respSupp.SupportingUnitId;
                if (subSystem != ECCSAModuleEnum.MandE)
                {
                    responsibleUnit = subSystem;
                    supportingUnit = TheUser.DepartmentId ?? 0;
                }
                var supportingUnits = (await _lookupRepository.LookupTypeById(SupportingUnitLookupTypeId))?.LookupValues.ToList()
                    ?? new List<LookupValue>();
                ProgressViewModel model = null;
                var setting = await _repository.Configuration();
                selectedYear = selectedYear == 0 ? setting.CurrentYear : selectedYear;
                var versionNo = 1;
                var monthNo = setting != null ? setting.CurrentMonth : 0;
                var yearNo = setting != null ? setting.CurrentYear : 0;
                if (string.IsNullOrEmpty(selectedPeriod))
                    selectedPeriod = monthNo.ToString();
                if (setting == null || setting.CurrentYear == 0 || setting.CurrentMonth == 0)
                {
                    model = new ProgressViewModel
                    {
                        TheModel = new Plan(),
                        TheList = new List<Plan>(),
                        FullName = TheUser.FullName,
                        CurrentModule = module,
                        CurrentSubsystem = subSystem,
                        CurrentMonth = monthNo,
                        CurrentYear = yearNo,
                        SelectedYear = selectedYear,
                        SelectedPeriod = selectedPeriod,
                        ResponsibleUnitFilter = responsibleUnitFilter,
                        SelectedScore = selectedScore,
                        TreeView = treeMode,
                        PlanVersionNo = versionNo,

                    };
                    model.ChangeActive(MenuItemEnum.AggregateReport);
                    model.Config = setting;
                    model.SupportingUnits = supportingUnits;
                    return View("AggregateReport", model);
                }
                model = await GetAggregateReport(selectedYear, selectedPeriod, responsibleUnitFilter, selectedScore, 
                    treeMode, subSystem, module, roleLevel, responsibleUnit, supportingUnit, supportingUnits, 
                    model, setting, versionNo, monthNo, yearNo);
                #endregion

                var workbook = AggregateReportExportColumnMap.ExportExcel(model);

                if (!Directory.Exists(TempAbsolutePath))
                {
                    Directory.CreateDirectory(TempAbsolutePath);
                }
                string fullPath = Path.Combine(TempAbsolutePath, "ExportMandEAggregateReport.xls");
                var stream = new MemoryStream();
                workbook.Write(stream);
                var fileName = "MandE Aggregate Report.xls";
                Response.Headers.Add("content-disposition", "inline;filename=" + $"{fileName}");

                return File(stream.ToArray(), "application/octet-stream", fileName);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private async Task<ProgressViewModel> GetAggregateReport(int selectedYear, string selectedPeriod, string responsibleUnitFilter, int selectedScore, bool treeMode, ECCSAModuleEnum subSystem, ECCSAModuleFlag module, RoleLevelEnum roleLevel, ECCSAModuleEnum responsibleUnit, int supportingUnit, List<LookupValue> supportingUnits, ProgressViewModel model, Configuration setting, int versionNo, int monthNo, int yearNo)
        {
            var list = await _repository.Performance(selectedYear, versionNo);
            var reportingUnit = ReportingUnit.List.FirstOrDefault(r => r.Id == (int)subSystem);
            var unplanned = reportingUnit != null ? await _repository.GetUnplannedActivity(yearNo, monthNo, reportingUnit.Id, TheUser.DepartmentId) : new UnplannedActivity();

            if (responsibleUnit > 0)
            {
                list = list.Where(l => (l.IsLabel && l.ResponsibleUnit == ECCSAModuleEnum.None)
                                       || (l.SupportingUnitId == supportingUnit && l.ResponsibleUnit == responsibleUnit))
                    .ToList();
            }
            if (!treeMode)
            {
                list = list.Where(l => !l.IsLabel).ToList();
            }

            if (selectedPeriod.StartsWith("Q"))
            {
                var qno = 0;
                int.TryParse(selectedPeriod.Replace("Q", ""), out qno);

                foreach (var plan in list)
                {
                    decimal totalActual = 0;
                    for (var i = 1; i <= qno; i++)
                    {
                        totalActual += plan.QuarterActual(i * 3, setting.CurrentMonth);
                    }
                    var qPlan = plan.QuarterPlan(qno * 3, setting.CurrentMonth);
                    var qActual = plan.QuarterActual(qno * 3, setting.CurrentMonth);
                    if (qPlan == qActual && qPlan == decimal.Zero)
                    {
                        plan.ReportedPlan = string.Empty;
                        plan.ReportedActual = string.Empty;
                        plan.ReportedProgress = string.Empty;
                        plan.ReportedYtDActual = string.Empty;
                    }
                    else
                    {
                        plan.ReportedPlan = qPlan.ToString(CultureInfo.InvariantCulture);
                        plan.ReportedActual = qActual.ToString(CultureInfo.InvariantCulture);
                        plan.ReportedProgress = (qActual * 100 / qPlan).ToString("N");
                        plan.ReportedYtDActual = $" | {totalActual}";
                    }

                    plan.ReportedPerformance = plan.ReportedPlan == string.Empty
                        ? String.Empty
                        : $"{plan.ReportedPlan} | {plan.ReportedActual} | {plan.ReportedProgress}%";
                    plan.ReportedOutput = plan.QuarterOutput(qno * 3);
                }
            }
            else
            {
                var mNo = 0;
                int.TryParse(selectedPeriod, out mNo);
                // list = list.Where(l => l.IsLabel || l.HasPlan(mNo)).ToList();
                foreach (var plan in list)
                {
                    var pmno = plan.PlanMonth(mNo);
                    var result = plan.TotalPlanActual(mNo);

                    var ytdPlan = result.plan;
                    var ytdActual = result.actual;
                    var cummWork = result.cummulativeWork;
                    var pl = pmno.Plan;
                    var act = pmno.Actual;
                    decimal totalActual = 0;

                    if (!(pl == act && pl == decimal.Zero))

                    {
                        plan.ReportedPlan = pmno.Plan.ToString(CultureInfo.InvariantCulture);
                        plan.ReportedActual = pmno.Actual.ToString(CultureInfo.InvariantCulture);
                        plan.ReportedProgress = (pmno.Actual * 100 / pmno.Plan).ToString("N");
                        plan.ReportedYtDActual = $" | {totalActual}";
                    }

                    if (!(ytdPlan == act && ytdPlan == decimal.Zero))

                    {
                        plan.ReportedYtDPlan = ytdPlan.ToString(CultureInfo.InvariantCulture);
                        plan.ReportedYtDActual = ytdActual.ToString(CultureInfo.InvariantCulture);
                        if (ytdPlan != decimal.Zero)
                        {
                            plan.ReportedYtDProgress = (ytdActual * 100 / ytdPlan).ToString("N");
                        }

                    }
                    plan.ReportedYtDOutput = cummWork;
                    plan.ReportedOutput = pmno.Output;
                }

            }

            var newList = list.Where(l => l.IsLabel || (l.ReportedPlan.Length > 0 && (l.ReportedProgressRange == "" || selectedScore == 0 || l.ReportedProgressRange == selectedScore.ToString()))).ToList();


            model = new ProgressViewModel
            {
                TheModel = new Plan(),
                TheList = newList,
                FullName = TheUser.FullName,
                CurrentModule = module,
                CurrentSubsystem = subSystem,
                CurrentMonth = monthNo,
                CurrentYear = yearNo,
                RoleLevel = roleLevel,
                SelectedPeriod = selectedPeriod,
                ResponsibleUnitFilter = responsibleUnitFilter,
                SelectedScore = selectedScore,
                SelectedYear = selectedYear,
                TreeView = treeMode,
                UnplannedActivity = unplanned
            };
            model.ScoreCards = PrepareScoreCards(setting, list); ;
            model.ChangeActive(MenuItemEnum.AggregateReport);
            model.Config = setting;
            model.SupportingUnits = supportingUnits;
            return model;
        }

        [HttpGet]
        public async Task<IActionResult> AnnualReport(
             int selectedYear
           , string responsibleUnitFilter
           , bool treeMode
           , ECCSAModuleEnum subSystem)
        {
            var module = (ECCSAModuleFlag)(int)subSystem;

            var roleLevel = await GetRoleLevel(module);
            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Index");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            var respSupp = GetUnitId(responsibleUnitFilter);
            var responsibleUnit = respSupp.ResponsibleUnit;
            var supportingUnit = respSupp.SupportingUnitId;

            var supportingUnits = (await _lookupRepository.LookupTypeById(SupportingUnitLookupTypeId))?.LookupValues.ToList()
                ?? new List<LookupValue>();
            ProgressViewModel model;
            var setting = await _repository.Configuration();
            var versionNo = 1;
            var monthNo = 12;

            selectedYear = selectedYear == 0 ? setting.CurrentYear : selectedYear;

            var list = await _repository.Performance(selectedYear, versionNo);
            var unplanned = await _repository.GetLatestUnplannedActivity(selectedYear);

            if (responsibleUnit > 0)
            {
                list = list.Where(l => (l.IsLabel && l.ResponsibleUnit == ECCSAModuleEnum.None)
                                       || (l.SupportingUnitId == supportingUnit && l.ResponsibleUnit == responsibleUnit))
                    .ToList();
                unplanned = unplanned.Where(l =>
                    l.SupportingUnitId == supportingUnit && l.ReportingUnit == (int)responsibleUnit).ToList();
            }
            if (!treeMode)
            {
                list = list.Where(l => !l.IsLabel).OrderBy(l=> l.Code).ToList();
            }



            foreach (var plan in list)
            {
                if (plan.IsLabel) continue;


                var qPlan = plan.PlanAmount;
                var qActual = plan.AnnualProgress;
                if (qPlan == qActual && qPlan == decimal.Zero) continue;

                plan.ReportedPlan = qPlan.ToString(CultureInfo.InvariantCulture);
                plan.ReportedActual = qActual.ToString(CultureInfo.InvariantCulture);
                plan.ReportedProgress = (qActual * 100 / qPlan).ToString("N");
                plan.ReportedYtDActual = $" | {qActual}";
               

                plan.ReportedPerformance = plan.ReportedPlan == string.Empty
                    ? String.Empty
                    : $"{plan.ReportedPlan} | {plan.ReportedActual} | {plan.ReportedProgress}%";
               
            }





            model = new ProgressViewModel
            {
                TheModel = new Plan(),
                TheList = list,
                FullName = TheUser.FullName,
                CurrentModule = module,
                CurrentSubsystem = subSystem,
                CurrentMonth = monthNo,
                CurrentYear = selectedYear,
                RoleLevel = roleLevel,
                ResponsibleUnitFilter = responsibleUnitFilter,
                SelectedYear = selectedYear,
                TreeView = treeMode,
                UnplannedActivities = unplanned
            };
            model.ScoreCards = PrepareScoreCards(setting, list); ;
            model.ChangeActive(MenuItemEnum.AnnualReport);
            model.Config = setting;
            model.SupportingUnits = supportingUnits;
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Quarterly(
           ECCSAModuleEnum subSystem)
        {
            var module = (ECCSAModuleFlag)(int)subSystem;

            var roleLevel = await GetRoleLevel(module);
            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Index");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            ProgressViewModel model;
            var setting = await _repository.Configuration();
            var monthNo = setting != null ? setting.CurrentMonth : 0;
            var yearNo = setting != null ? setting.CurrentYear : 0;
            if (monthNo % 3 != 0)
            {
                return RedirectToAction("Index", new { subSystem });
            }
            if (setting == null || setting.CurrentYear == 0 || setting.CurrentMonth == 0)
            {
                model = new ProgressViewModel
                {
                    TheModel = new Plan(),
                    TheList = new List<Plan>(),
                    FullName = TheUser.FullName,
                    CurrentModule = module,
                    CurrentSubsystem = subSystem,
                    CurrentMonth = monthNo,
                    CurrentYear = yearNo,
                };
                return View(model);
            }
            var list = await _repository.Performance();
            list = list.Where(l => (l.ResponsibleUnit == ECCSAModuleEnum.None || l.ResponsibleUnit == subSystem) && l.HasQuarterlyPlan(monthNo)).ToList();
            var lst = new List<Plan>();

            model = new ProgressViewModel
            {
                TheModel = new Plan(),
                TheList = list,
                FullName = TheUser.FullName,
                CurrentModule = module,
                CurrentSubsystem = subSystem,
                CurrentMonth = monthNo,
                CurrentYear = yearNo,
                RoleLevel = roleLevel,
            };

            model.ChangeActive(MenuItemEnum.Progress);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(MonthlyPlan data,
            ECCSAModuleEnum subSystem)
        {
            try
            {
                LogStart(_logger, controllerName, "SaveProgress");
                await _repository.SaveProgress(data, TheUser);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (Exception ex)
            {
                LogError(_logger, ex, controllerName, "SaveProgress", data);
                TempData["CommandStatus"] = ex.Message;
            }

            return RedirectToAction("Index", new { subSystem });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveUnplanned(UnplannedActivity data,
            ECCSAModuleEnum subSystem)
        {
            try
            {
                LogStart(_logger, controllerName, "SaveProgress");
                data.SupportingUnitId = TheUser.DepartmentId ?? 0;
                await _repository.Save(data);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (Exception ex)
            {
                LogError(_logger, ex, controllerName, "SaveProgress", data);
                TempData["CommandStatus"] = ex.Message;
            }

            return RedirectToAction("Index", new { subSystem });
        }

        [HttpPost]
        public async Task<IActionResult> SaveQuarterlyForm(MonthlyPlan data)
        {
            try
            {
                LogStart(_logger, controllerName, "SaveProgress");
                //await _repository.SaveQuarterlyProgress(data,TheUser);
            }
            catch (Exception ex)
            {
                LogError(_logger, ex, controllerName, "SaveProgress", data);
                return BadRequest(ex.Message);
            }

            return Ok(true);
        }

        [HttpPost]
        public async Task<IActionResult> SaveForm(MonthlyPlan data)
        {
            try
            {
                LogStart(_logger, controllerName, "SaveProgress");
                await _repository.SaveProgress(data, TheUser);
            }
            catch (Exception ex)
            {
                LogError(_logger, ex, controllerName, "SaveProgress", data);
                return BadRequest(ex.Message);
            }

            return Ok(true);
        }

        private static List<ScoreCard> PrepareScoreCards(Configuration config, List<Plan> list, ECCSAModuleEnum unit = ECCSAModuleEnum.None)
        {
            var scoreCards = new List<ScoreCard>();
            if (unit != ECCSAModuleEnum.None)
            {
                list = list.Where(l => l.ResponsibleUnit == unit).ToList();
            }
            foreach (string unitName in ScoreCard.ResponsibleUnits)
            {
                var tmp = list.Where(l => ScoreCard.GetResponsibleUnitCode(l.ResponsibleUnit) == unitName).ToList();
                if (tmp.Count == 0) continue;
                tmp = tmp.Where(i => i.ReportedProgress.Length > 0).ToList();
                // tmp = tmp.Where(i=> i.PlanMonth(monthNo).Plan > 0).ToList();

                var f = tmp.Count(i => decimal.Parse(i.ReportedProgress) <= config.YelloValue);
                var d = tmp.Count(i => decimal.Parse(i.ReportedProgress) > config.YelloValue && decimal.Parse(i.ReportedProgress) <= config.BlueValue);
                var c = tmp.Count(i => decimal.Parse(i.ReportedProgress) > config.BlueValue && decimal.Parse(i.ReportedProgress) <= config.BlueGreenValue);
                var b = tmp.Count(i => decimal.Parse(i.ReportedProgress) > config.BlueGreenValue && decimal.Parse(i.ReportedProgress) <= config.GreenValue);
                var a = tmp.Count(i => decimal.Parse(i.ReportedProgress) > config.GreenValue);
                if (f + d + c + b + a == 0) continue;
                var card = new ScoreCard()
                {
                    ResponsibleUnit = unitName,
                    TotalIndicators = tmp.Count,
                    FIndicators = f,
                    DIndicators = d,
                    BIndicators = b,
                    AIndicators = a,
                };

                scoreCards.Add(card);
            }

            return scoreCards;
        }
        [HttpGet]
        public async Task<FileResult> DownLoadImportDatasheet(ECCSAModuleEnum subSystem)
        {
            var setting = await _repository.Configuration();
            var monthNo = setting?.CurrentMonth ?? 0;
            var yearNo = setting?.CurrentYear ?? 0;
            var exportList = await GetProgressTemplate(subSystem, yearNo, monthNo);
            var reportingUnit = ReportingUnit.List.FirstOrDefault(r => r.Id == (int)subSystem);
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add($"ProgressTemplate{reportingUnit.Code}.{yearNo}.{monthNo}");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Code";
                worksheet.Cell(currentRow, 2).Value = "Name";
                worksheet.Cell(currentRow, 3).Value = "Unit";
                worksheet.Cell(currentRow, 4).Value = "AnnualPlan";
                worksheet.Cell(currentRow, 5).Value = "Plan";
                worksheet.Cell(currentRow, 6).Value = "Actual";
                worksheet.Cell(currentRow, 7).Value = "Output";
                worksheet.Cell(currentRow, 8).Value = "CumulativeWork";
                worksheet.Cell(currentRow, 9).Value = "Remark";
                foreach (var plan in exportList)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = plan.Code;
                    worksheet.Cell(currentRow, 2).Value = plan.Name;
                    worksheet.Cell(currentRow, 3).Value = plan.Unit;
                    worksheet.Cell(currentRow, 4).Value = plan.AnnualPlan;
                    worksheet.Cell(currentRow, 5).Value = plan.Plan;
                    worksheet.Cell(currentRow, 6).Value = plan.Actual;
                    worksheet.Cell(currentRow, 7).Value = plan.Outcome;
                    worksheet.Cell(currentRow, 8).Value = plan.CumulativeWork;
                    worksheet.Cell(currentRow, 9).Value = plan.Remark;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        $"ProgressTemplate{reportingUnit.Code}.{yearNo}.{monthNo}.xlsx");
                }
            }


            var fileName = "ProgressReportImportDatasheet.xlsx";
            var path = Path.Combine(_hostingEnvironment.WebRootPath, "ReportTemplates", fileName);
            return await DownLoadFile(path, fileName);
        }

        private async Task<List<ProgressTemplate>> GetProgressTemplate(ECCSAModuleEnum subSystem, int yearNo, int monthNo)
        {

            if (yearNo == 0 || monthNo == 0)
                return new List<ProgressTemplate>();
            var list = await _repository.Performance();
            list = list.Where(l => (l.ResponsibleUnit == ECCSAModuleEnum.None || l.ResponsibleUnit == subSystem) && l.HasPlan(monthNo)).ToList();
            var exportList = new List<ProgressTemplate>();
            foreach (var plan in list)
            {
                var planMonth = plan.PlanMonth(monthNo);
                if (plan.IsLabel || plan.TotalPlanAmount == 0 || planMonth.Plan == 0) continue;

                var d = new ProgressTemplate
                {
                    Id = plan.Id,
                    Code = plan.Code,
                    Name = plan.Name,
                    Unit = plan.UnitName,
                    AnnualPlan = plan.TotalPlanAmount,
                    Plan = planMonth.Plan,
                    Actual = planMonth.Actual,
                    Outcome = planMonth.Output,
                    Remark = planMonth.Remark,
                };
                exportList.Add(d);
            }

            return exportList;
        }

        [HttpPost]
        public async Task<IActionResult> ImportProgressDatasheet(ECCSAModuleEnum subSystem, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Invalid file format");
            }
            var setting = await _repository.Configuration();
            var monthNo = setting?.CurrentMonth ?? 0;
            var yearNo = setting?.CurrentYear ?? 0;
            var exportList = await GetProgressTemplate(subSystem, yearNo, monthNo);
            var reportingUnit = ReportingUnit.List.FirstOrDefault(r => r.Id == (int)subSystem);
            var lst = ProgressImportColumnMap.ImportExcel(file, exportList, TempAbsolutePath);

            foreach (var item in lst)
            {

                var data = new MonthlyPlan
                {
                    PlanId = item.Id,
                    MonthNo = monthNo,
                    Plan = item.Plan,
                    Actual = item.Actual,
                    Output = item.Outcome,
                    Remark = item.Remark,
                    CumulativeWorkDone = item.CumulativeWork,
                    Progress = item.Plan != Decimal.Zero ? item.Actual / item.Plan : Decimal.Zero
                };

                try
                {
                    await _repository.SaveProgress(data, TheUser);
                }
                catch (Exception ex)
                {
                    TempData["CommandStatus"] = ex.Message;
                }
            }
            return RedirectToAction("Index", new { subSystem });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitProgress(int supportingUnitId, ECCSAModuleEnum subsystem)
        {
            var setting = await _repository.Configuration();

            try
            {
                var data = new Plan { ResponsibleUnit = subsystem, SupportingUnitId = supportingUnitId };
                await _repository.SubmitProgress(data, TheUser);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (Exception ex)
            {
                LogError(_logger, ex, controllerName, "SubmitProgress", new { setting.CurrentYear, subsystem });
                TempData["CommandStatus"] = ex.Message;
            }

            return RedirectToAction("Index", new { subsystem, draft = false, rejected = false, submitted = true, approved = false, completed = false, treeMode = false });
        }
    }
}
