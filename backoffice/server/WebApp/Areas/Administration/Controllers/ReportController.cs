using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common;
using ECCSA.BackOffice.Server.Persistence.Repositories.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories.UserAdministration;
using ECCSA.BackOffice.Server.WebApp.Areas.Administration.Model;
using ECCSA.BackOffice.Server.WebApp.Controllers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Reports;
using ECCSA.BackOffice.Server.Model.Common.Reports;
using System.Reflection.PortableExecutable;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Notification;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Administration.Controllers
{
    [Area("Administration")]
    public class ReportController : BaseController
    {
        private readonly ILogger<ReportController> _logger;
        private readonly ReportsRepository _repository;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly YesNoRepository _lookupRepository;

        public ReportController(ILogger<ReportController> logger,
             UserManager<User> userManager,
             ReportsRepository repository,
              IWebHostEnvironment hostingEnvironment,
             PublicUserRepository userRepository,
         YesNoRepository lookupRepository
             , NotificationRepository notificationRepository) : base(userManager, userRepository, notificationRepository)
        {

            _logger = logger;
            _repository = repository;
            _hostingEnvironment = hostingEnvironment;
            controllerName = "Report";
            _lookupRepository = lookupRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            LogStart(_logger, controllerName, "Index");
            var userName = TheUser.FullName;
            ReportViewModel model;

            if (!TheUser.AssignedModules.HasFlag(ECCSAModuleFlag.Administration))
            {
                _logger.LogError("UserAdministration",
                                 "Index",
                                 "Insufficient Privilege",
                                 TheUser,
                                 DateTime.UtcNow);
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            var supportingUnits = (await _lookupRepository.LookupTypeById(new Guid("424063b8-7cef-4ff0-8a13-c88334a100a5")))?.LookupValues.ToList()
    ?? new List<LookupValue>();

            model = new ReportViewModel
            {
                CurrentOperation = "List",
                TheList = await _repository.Reports(),
                Sources = await _repository.ReportSources(),

            };

            model.RoleLevel = await GetRoleLevel(ECCSAModuleFlag.Administration);
            TempData["title"] = model.CurrentPage;
            LogEnd(_logger, controllerName, "Index");

            return View(model);
        }



        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            LogStart(_logger, controllerName, "Detail");
            var userName = TheUser.FullName;
            ReportViewModel model;
            var lookupTypes = await _lookupRepository.LookupTypes();
            lookupTypes.Insert(0, new LookupType { Id = Guid.Empty, Name = "Select" });

            var sourceList = await _repository.ReportSources();
            sourceList.Insert(0, new ReportSource { SourceName = "Select" });

            model = new ReportViewModel
            {
                FullName = userName,
                CurrentOperation = "List",
                TheList = new List<Server.Model.Common.Reports.Report>(),
                TheModel = await _repository.ReportById(id),
                Sources = sourceList,
                LookupTypes = lookupTypes
            };
            if (model.TheModel == null)
            {
                return RedirectToAction("Error", "Home", new { area = "" });
            }
            foreach (var mod in Enum.GetValues<ECCSAModuleFlag>())
            {
                model.AllModules.Add(new Server.Model.Membership.SimpleLookup()
                {
                    Id = ((int)mod),
                    Name = mod.GetDisplayName()
                });

            }
            TempData["title"] = model.CurrentPage;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Save(Server.Model.Common.Reports.Report data)
        {
            LogStart(_logger, controllerName, "Save");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Administration);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "Save");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("Detail", new { id = data.Id });
            }
            try
            {

                await _repository.Save(data);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("Save Lookup Type", ex, data);
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Save Lookup Type", ex, data);
                CommandStatus(CommandStatusEnum.UnkownError);
            }
            if (data.Id == 0)
                return RedirectToAction("Index");
            else
                return RedirectToAction("Detail", new { id = data.Id });
        }

        [HttpPost]
        public async Task<IActionResult> SaveReportSubsystems(int reportId, List<int> subsystems)
        {
            LogStart(_logger, controllerName, "SaveReportSubsystems");
            try
            {
                await _repository.ChangeReportSubsystems(reportId, subsystems);
                CommandStatus("Report Subsystems saved Successfully");
            }
            catch (Exception ex)
            {
                LogError(_logger, ex, controllerName, "SaveReportSubsystems", new { reportId, subsystems });
                TempData["CommandStatus"] = ex.Message;
            }
            LogEnd(_logger, controllerName, "SaveReportSubsystems");
            return RedirectToAction("Detail", new { id = reportId });
        }

        [HttpPost]
        public async Task<IActionResult> SaveReportField(Server.Model.Common.Reports.ReportField data)
        {

            LogStart(_logger, controllerName, "SaveReportField");
            try
            {
                await _repository.ChangeReportField(data);
                CommandStatus("ReportField saved Successfully");
            }
            catch (Exception ex)
            {
                LogError(_logger, ex, controllerName, "SaveReportField", new {data });
                TempData["CommandStatus"] = ex.Message;
            }
            LogEnd(_logger, controllerName, "SaveUserSubsystems");
            return RedirectToAction("Detail", new { id = data.ReportId });
        }


        [HttpPost]
        public async Task<IActionResult> SaveEditReport(Server.Model.Common.Reports.Report data)
        {
            LogStart(_logger, controllerName, "SaveEditReport");
            try
            {
                await _repository.ChangeEditReport(data);
                CommandStatus("Report saved Successfully");
            }
            catch (Exception ex)
            {
                LogError(_logger, ex, controllerName, "SaveEditReport", new { data });
                TempData["CommandStatus"] = ex.Message;
            }
            LogEnd(_logger, controllerName, "SaveEditReport");
            return RedirectToAction("Detail", new { id = data.Id });
        }

    }
}
