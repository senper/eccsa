using ECCSA.BackOffice.Server.Model.Academy;
using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Persistence.DTO;
using ECCSA.BackOffice.Server.Persistence.DTO.Academy;
using ECCSA.BackOffice.Server.Persistence.Repositories.Academy;
using ECCSA.BackOffice.Server.Persistence.Repositories.Security;
using ECCSA.BackOffice.Server.WebApp.Areas.Academy.Model;
using ECCSA.BackOffice.Server.WebApp.Controllers;
using ECCSA.BackOffice.Server.WebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Notification;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Academy.Controllers
{
    [Area("Academy")]
    public class AcademySettingsController : BaseController
    {
        private readonly ILogger<AcademySettingsController> _logger;

        private readonly SponsorRepository _sponsorRepository;
        private readonly SettingVariableRepository _settingVariableRepository;
        public AcademySettingsController(ILogger<AcademySettingsController> logger,
            UserManager<User> userManager,
            SponsorRepository sponsorRepository,
            SettingVariableRepository settingVariableRepository,
            PublicUserRepository userRepository, NotificationRepository notificationRepository) : base(userManager, userRepository, notificationRepository)
        {
            _logger = logger;
            _sponsorRepository = sponsorRepository;
            _settingVariableRepository = settingVariableRepository;
        }
        [HttpGet]
        public async Task<IActionResult> SponsorsList()
        {
            var userName = TheUser.FullName;
            LogStart(_logger, controllerName, "SponsorsList");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Academy);
            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "SponsorsList");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            var model = new SponsorViewModel
            {
                FullName = userName,
                CurrentPage = "Sponsors",
                CurrentOperation = "List",
                TheList = _sponsorRepository.Sponsors(TheUser.Id).ToList(),
            };
            model.ChangeActive(MenuItemEnum.AcademySponsors);
            model.RoleLevel = roleLevel;
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> SponsorEdit(Guid id)
        {
            var userName = TheUser.FullName;

            var model = new SponsorViewModel
            {
                FullName = userName,
                CurrentPage = "Sponsors",
                TheModel = (await _sponsorRepository.SponsorById(id, TheUser.Id)) ?? new Sponsor(),

            };
            model.ChangeActive(MenuItemEnum.AcademySponsors);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CreateSponsor(CreateSponsorInput theModel)
        {
            LogStart(_logger, controllerName, "CreateSponsor");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Academy);
            if (roleLevel <= RoleLevelEnum.User)
            {
                LogSecurityError(_logger, controllerName, "CreateSponsor");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            try
            {
                await _sponsorRepository.Create(new GenericInput<CreateSponsorInput>(theModel, TheUser.Id));
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unknown Error");
                TempData["CommandStatus"] = ex.Message;
            }
            return RedirectToAction("SponsorsList");
        }
        [HttpPost]
        public async Task<IActionResult> UpdateSponsor(UpdateSponsorInput theModel)
        {
            LogStart(_logger, controllerName, "UpdateSponsor");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Academy);
            if (roleLevel <= RoleLevelEnum.User)
            {
                LogSecurityError(_logger, controllerName, "UpdateSponsor");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            var payload = await _sponsorRepository.Update(new GenericInput<UpdateSponsorInput>(theModel, TheUser.Id));
            CommandStatus(CommandStatusEnum.Updated);
            return RedirectToAction("SponsorsList", new { id = payload.PayLoad.Id });
        }
        [HttpPost]
        public async Task<IActionResult> DeleteSponsor(Guid id)
        {
            LogStart(_logger, controllerName, "DeleteSponsor");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Academy);
            if (roleLevel <= RoleLevelEnum.User)
            {
                LogSecurityError(_logger, controllerName, "DeleteSponsor");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            var sponsor = (await _sponsorRepository.SponsorById(id, TheUser.Id));
            if (sponsor != null)
            {
                await _sponsorRepository.Delete(sponsor);
                CommandStatus(CommandStatusEnum.Deleted);
            }
            else
            {
                TempData["CommandStatus"] = "Invalid Sponsor";
            }
            return RedirectToAction("SponsorsList");
        }
        ///
        [HttpGet]
        public async Task<IActionResult> SettingVariablesList()
        {
            var userName = TheUser.FullName;
            LogStart(_logger, controllerName, "SettingVariablesList");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Academy);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "SettingVariablesList");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            var model = new SettingVariableViewModel
            {
                FullName = userName,
                CurrentPage = "Setting Variables",
                CurrentOperation = "List",
                TheList = _settingVariableRepository.SettingVariables(TheUser.Id).ToList(),
            };
            model.ChangeActive(MenuItemEnum.AcademySettingVariables);
            model.RoleLevel = roleLevel;
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> SettingVariableEdit(int id)
        {
            var userName = TheUser.FullName;

            var model = new SettingVariableViewModel
            {
                FullName = userName,
                CurrentPage = "Setting Variables",
                TheModel = (await _settingVariableRepository.SettingVariableById(id, TheUser.Id)) ?? new SettingVariable(),

            };
            model.ChangeActive(MenuItemEnum.AcademySettingVariables);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CreateSettingVariable(CreateSettingVariableInput theModel)
        {
            LogStart(_logger, controllerName, "CreateSettingVariable");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Academy);
            if (roleLevel <= RoleLevelEnum.User)
            {
                LogSecurityError(_logger, controllerName, "CreateSettingVariable");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            try
            {
                await _settingVariableRepository.Create(new GenericInput<CreateSettingVariableInput>(theModel, TheUser.Id));
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unknown Error");
                TempData["CommandStatus"] = ex.Message;
            }
            return RedirectToAction("SettingVariablesList");
        }
        [HttpPost]
        public async Task<IActionResult> UpdateSettingVariable(UpdateSettingVariableInput theModel)
        {
            LogStart(_logger, controllerName, "UpdateSettingVariable");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Academy);
            if (roleLevel <= RoleLevelEnum.User)
            {
                LogSecurityError(_logger, controllerName, "UpdateSettingVariable");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            var payload = await _settingVariableRepository.Update(new GenericInput<UpdateSettingVariableInput>(theModel, TheUser.Id));
            CommandStatus(CommandStatusEnum.Updated);
            return RedirectToAction("SettingVariablesList", new { id = payload.PayLoad.Id });
        }
        [HttpPost]
        public async Task<IActionResult> DeleteSettingVariable(int id)
        {
            LogStart(_logger, controllerName, "DeleteSettingVariable");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Academy);
            if (roleLevel <= RoleLevelEnum.User)
            {
                LogSecurityError(_logger, controllerName, "DeleteSettingVariable");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            var settingVariable = (await _settingVariableRepository.SettingVariableById(id, TheUser.Id));
            if (settingVariable != null)
            {
                await _settingVariableRepository.Delete(settingVariable);
                CommandStatus(CommandStatusEnum.Deleted);
            }
            else
            {
                TempData["CommandStatus"] = "Invalid SettingVariable";
            }
            return RedirectToAction("SettingVariablesList");
        }
    }
}
