using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Membership;
using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories.Membership;
using ECCSA.BackOffice.Server.Persistence.Repositories.Security;
using ECCSA.BackOffice.Server.WebApp.Areas.Membership.Model;
using ECCSA.BackOffice.Server.WebApp.Controllers;
using ECCSA.BackOffice.Server.WebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Notification;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Membership.Controllers
{
    [Area("Membership")]
    public class IssueController : BaseController
    {
        private readonly ILogger<IssueController> _logger;
        readonly IssueRepository _repository;
        public IssueController(ILogger<IssueController> logger, UserManager<User> userManager, IssueRepository repository, PublicUserRepository userRepository, NotificationRepository notificationRepository) : base(userManager, userRepository, notificationRepository)
        {

            _logger = logger;
            _repository = repository;

            controllerName = "Issue";

        }

        public async Task<IActionResult> Index(
            ECCSAModuleFlag module
            , IssueStatus status = IssueStatus.New
            , Guid? issueTypeId = null)
        {
            var model = new IssueViewModel();
            LogStart(_logger, controllerName, "Index");
            var roleLevel = await GetRoleLevel(module);
            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Index");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            try
            {
                if (status == IssueStatus.New && module != ECCSAModuleFlag.TradePromotion)
                    status = IssueStatus.Assigned;
                model.TheList = await _repository.Issues(module, status, issueTypeId);
                model.IssueTypes = await _repository.IssueTypes();
            }
            catch (Exception ex)
            {
                model.TheList = new List<Issue>();
                _logger.LogError(ex, "Issue Index Exception");
                return RedirectToAction("Error", "Home", new { area = "" });
            }

            model.CurrentOperation = "Index";
            model.CurrentModule = module;
            model.Status = status;
            model.IssueTypeId = issueTypeId ?? Guid.Empty;
            model.ChangeActive(MenuItemEnum.Issue);
            return View(model);
        }

        public async Task<IActionResult> Detail(ECCSAModuleFlag module, Guid id)
        {
            _logger.LogInformation("Issue Detail Started");
            var model = new IssueViewModel();
            try
            {
                model.TheModel = await _repository.Issue(id);
                model.IssueTypes = await _repository.IssueTypes();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Issue Detail Exception {id}");
            }
            if (model.TheModel == null)
            {
                return RedirectToAction("Error", "Home", new { area = "" });
            }
            // model.TheModel.MediaType2Bool();
            model.CurrentOperation = "Detail";
            model.CurrentModule = module;
            model.ChangeActive(MenuItemEnum.Issue);
            return View(model);
        }

        public async Task<IActionResult> Report(
            ECCSAModuleFlag module
            , DateTime issueDate)
        {
            var model = new IssueReportViewModel();
            LogStart(_logger, controllerName, "Index");
            var roleLevel = await GetRoleLevel(module);
            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Index");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            try
            {
                model.Issues = await _repository.Issues(issueDate, module);
                model.IssueTypes = await _repository.IssueTypes();
            }
            catch (Exception ex)
            {
                model.TheList = new List<IssueReport>();
                _logger.LogError(ex, "Issue Index Exception");
            }
            foreach (var type in model.IssueTypes)
            {
                var lst = model.Issues.Where(i => i.TypeId == type.Id).ToList();
                if (lst.Count == 0) continue;
                var report = new IssueReport
                {
                    IssueType = type.NameEng,
                };
                foreach (var item in lst)
                {
                    if (item.Status == IssueStatus.Resolved)
                    {
                        report.Resolved += 1;
                    }
                    else
                    {
                        var days = DateTime.Today.Subtract(item.IssueDate).Days;
                        if (days < 30)
                            report.UnresolvedCurrent += 1;
                        else if (days < 60)
                            report.Unresolved30Days += 1;
                        else
                            report.Unresolved60Days += 1;

                    }
                }
                model.TheList.Add(report);
            }
            if (model.TheList.Count <= 0)
            {
                model.TheList = IssueReport.FakeData(model.IssueTypes);
            }
            model.ReportModel =
                    JsonConvert.SerializeObject(MultiHorizontalBarChartModel.
                    FromIssueReportList(model.TheList));
            model.CurrentOperation = "Report";
            model.CurrentModule = module;
            model.ChangeActive(MenuItemEnum.Issue);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CloseIssue(Guid id, String remark, ECCSAModuleFlag module)
        {
            try
            {
                await _repository.CloseIssue(id, remark, TheUser.Id);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("CloseIssue", ex, id);
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("CloseIssue", ex, id);
                CommandStatus(CommandStatusEnum.UnkownError);
            }

            return RedirectToAction("Detail", new { id, module });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateIssueRemark(Guid id, String remark, ECCSAModuleFlag module)
        {
            try
            {
                await _repository.UpdateIssueRemark(id, remark, TheUser.Id);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("CloseIssue", ex, id);
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("CloseIssue", ex, id);
                CommandStatus(CommandStatusEnum.UnkownError);
            }

            return RedirectToAction("Detail", new { id, module });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignIssue(Guid id, ECCSAModuleFlag owner, String remark, ECCSAModuleFlag module)
        {
            try
            {
                await _repository.AssignIssue(id, owner, remark, TheUser.Id);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("AssignIssue", ex, id);
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("AssignIssue", ex, id);
                CommandStatus(CommandStatusEnum.UnkownError);
            }

            return RedirectToAction("Detail", new { id, module });

        }

        [HttpPost]
        public async Task<IActionResult> Save(Issue model)
        {
            try
            {
                await _repository.Save(model, TheUser.Id);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("Save", ex, model);
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Save", ex, model);
                CommandStatus(CommandStatusEnum.UnkownError);
            }

            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<FileResult> DownLoadFile(Guid id, string fileName, string extention)
        {
            return await DownLoadFile(IssuesAbsolutePath + "\\" + Guid2String(id).ToString() + extention, fileName);
        }

    }

}
