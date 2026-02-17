using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Common.Servicing;
using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Servicing;
using ECCSA.BackOffice.Server.Persistence.Repositories.Security;
using ECCSA.BackOffice.Server.WebApp.Areas.Service.Model;
using ECCSA.BackOffice.Server.WebApp.Controllers;
using ECCSA.BackOffice.Server.WebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Notification;
using ECCSA.BackOffice.Server.Persistence.Repositories.UserAdministration;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Service.Controllers
{
    [Area("Service")]
    public class ServiceTypeController : BaseController
    {
        private readonly ILogger<ServiceTypeController> _logger;
        private readonly ServiceTypeRepository _settingRepository;
        private readonly UserAdministrationRepository _userAdminRepository;
        public ServiceTypeController(ILogger<ServiceTypeController> logger
            , UserManager<User> userManager
            , ServiceTypeRepository settingRepository
            , PublicUserRepository userRepository
            , UserAdministrationRepository userAdminRepository
            , NotificationRepository notificationRepository) : base(userManager, userRepository, notificationRepository)
        {

            _logger = logger;
            _settingRepository = settingRepository;
            _userAdminRepository = userAdminRepository;
            controllerName = "ServiceType";
        }

        private static string GetCurrentPage(ECCSAModuleFlag module)
        {
            return module switch
            {
                ECCSAModuleFlag.Academy => "Chamber Academy Services",
                ECCSAModuleFlag.Arbitration => "Arbitration Services",
                ECCSAModuleFlag.Communication => "Communication Services",
                ECCSAModuleFlag.Investment => "Trade Promotion Services",
                ECCSAModuleFlag.Membership => "Membership Services",
                ECCSAModuleFlag.Reporting => "Reporting Services",
                ECCSAModuleFlag.Research => "Research & Advocacy Services",
                ECCSAModuleFlag.TradePromotion => "Trade Facilitation Services",
                _ => string.Empty,
            };
        }
        public async Task<IActionResult> Index(ECCSAModuleFlag module)
        {
            LogStart(_logger, controllerName, "Index");
            var userName = TheUser.FullName;
            var roleLevel = await GetRoleLevel(module);

            ServiceTypeViewModel model;
            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Index");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            model = new ServiceTypeViewModel
            {
                FullName = userName,
                CurrentPage = GetCurrentPage(module),
                CurrentOperation = "List",
                TheList = await _settingRepository.ServiceTypes(module),
                RoleLevel = roleLevel,
                TheModel = new ServiceType()
                {
                    ServiceOwner = module,
                    Id = Guid.Empty,
                    Status = StatusEnum.Active,
                }

            };

            model.ChangeActive(MenuItemEnum.ServiceType);
            model.CurrentModule = module;
            TempData["title"] = GetCurrentPage(module);
            LogEnd(_logger, controllerName, "Index");
            return View(model);
        }


        public async Task<IActionResult> Detail(Guid id)
        {
            LogStart(_logger, controllerName, "Detail");
            var userName = TheUser.FullName;
            ServiceTypeViewModel model;
            if (Guid.Empty == id)
            {
                return NotFound();
            }


            model = new ServiceTypeViewModel
            {
                FullName = userName,
                CurrentPage = "Investment",
                CurrentOperation = "List",
                TheList = new List<ServiceType>(),
                TheModel = await _settingRepository.ServiceTypeById(id),


            };


            if (model.TheModel == null)
            {
                return RedirectToAction("Error", "Home", new { area = "" });
            }
            model.CurrentModule = model.TheModel.ServiceOwner;
            var roleLevel = await GetRoleLevel(model.TheModel.ServiceOwner);
            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Index");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            model.Users = (await _userAdminRepository.BackOfficeUsers()).Where(u => u.AssignedModules.HasFlag(model.CurrentModule)).ToList();
            model.ReportTemplates = GetReportTemplates();
            model.HasServiceRequest = model.TheModel.ServiceRequests.Any();
            model.LookupTypes = await _settingRepository.LookupTypes(model.CurrentModule);
            model.RoleLevel = roleLevel;
            model.Groups = model.TheModel.ServiceRequirements
                .Where(r => r.RequirementType == RequirementTypeEnum.Header)
                .OrderBy(r => r.OrderNo).ToList();

            model.ChangeActive(MenuItemEnum.ServiceType);

            model.CurrentPage = GetCurrentPage(model.CurrentModule);
            TempData["title"] = model.CurrentPage;
            LogEnd(_logger, controllerName, "Detail");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveServiceType(ServiceType model)
        {
            _logger.LogInformation("SaveServiceType", "Started", DateTime.UtcNow);
            var roleLevel = await GetRoleLevel(model.ServiceOwner);
            if (roleLevel != RoleLevelEnum.Admin)
            {
                _logger.LogError("SaveServiceType", "Insufficient Privilege");
                CommandStatus("Insufficient Privilege");
                return RedirectToAction("Index", new { module = model.ServiceOwner });
            }
            try
            {
                await _settingRepository.Save(model, TheUser.Id);
                CommandStatus(CommandStatusEnum.Saved);
                _logger.LogInformation("SaveServiceType", "End", DateTime.UtcNow);
            }
            catch (Exception ex)
            {
                _logger.LogError("SaveServiceType", ex, model);
                CommandStatus(CommandStatusEnum.UnkownError);
            }
            if (model.Id == Guid.Empty)
            {
                return RedirectToAction("Index", new { module = model.ServiceOwner });
            }
            else
            {
                return RedirectToAction("Detail", new { id = model.Id });
            }
        }



        [HttpPost]
        public async Task<IActionResult> SaveRequirement(ServiceRequirement model)
        {
            _logger.LogInformation("SaveRequirement", "Started", DateTime.UtcNow);
            try
            {
                var serviceType = await _settingRepository.ServiceTypeById(model.ServiceTypeId);
                var roleLevel = await GetRoleLevel(serviceType.ServiceOwner);
                if (roleLevel != RoleLevelEnum.Admin)
                {
                    _logger.LogError("SaveRequirement", "Insufficient Privilege");
                    CommandStatus("Insufficient Privilege");
                    return RedirectToAction("Detail", new { id = model.ServiceTypeId });
                }
                await _settingRepository.Save(model, TheUser.Id);
                CommandStatus(CommandStatusEnum.Saved);
                _logger.LogInformation("SaveRequirement", "End", DateTime.UtcNow);
            }
            catch (Exception ex)
            {
                _logger.LogError("SaveRequirement", ex, model);
                CommandStatus(CommandStatusEnum.UnkownError);
            }
            return RedirectToAction("Detail", new { id = model.ServiceTypeId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteContact(Guid id, Guid userId)
        {
            try
            {
                var serviceType = await _settingRepository.ServiceTypeById(id);
                var worker = serviceType.WorkerList.Find(w => w.UserId == userId);
                var roleLevel = await GetRoleLevel(serviceType.ServiceOwner);
                if (roleLevel != RoleLevelEnum.Admin)
                {
                    _logger.LogError("SaveRequirementHeader", "Insufficient Privilege");
                    CommandStatus("Insufficient Privilege");
                    return RedirectToAction("Detail", new { id });
                }

                if (worker != null)
                {
                    serviceType.WorkerList.Remove(worker);
                    await _settingRepository.SaveWorker(serviceType);
                }
                //  await _settingRepository.SaveServiceRequirementHeader(model.Id, model.OrderNo, model.Name, model.ServiceTypeId , TheUser.Id);
                CommandStatus(CommandStatusEnum.Saved);

            }
            catch (Exception ex)
            {
                _logger.LogError("SaveRequirementHeader", ex, new { id, userId });
                CommandStatus(CommandStatusEnum.UnkownError);
            }
            return RedirectToAction("Detail", new { id });
        }
        [HttpPost]
        public async Task<IActionResult> AddContact(Guid id, Guid userId)
        {
            try
            {
                var serviceType = await _settingRepository.ServiceTypeById(id);
                var worker = serviceType.WorkerList.Find(w => w.UserId == userId);
                var roleLevel = await GetRoleLevel(serviceType.ServiceOwner);
                if (roleLevel != RoleLevelEnum.Admin)
                {
                    _logger.LogError("SaveRequirementHeader", "Insufficient Privilege");
                    CommandStatus("Insufficient Privilege");
                    return RedirectToAction("Detail", new { id });
                }
                if (worker == null)
                {
                    serviceType.WorkerList.Add(new ServiceTypeWorker { UserId = userId });
                    await _settingRepository.SaveWorker(serviceType);
                }
                //  await _settingRepository.SaveServiceRequirementHeader(model.Id, model.OrderNo, model.Name, model.ServiceTypeId , TheUser.Id);
                CommandStatus(CommandStatusEnum.Saved);

            }
            catch (Exception ex)
            {
                _logger.LogError("SaveRequirementHeader", ex, new { id, userId });
                CommandStatus(CommandStatusEnum.UnkownError);
            }
            return RedirectToAction("Detail", new { id });
        }

        [HttpPost]
        public async Task<IActionResult> SaveRequirementHeader(RequirementHeaderViewModel model)
        {
            try
            {
                var serviceType = await _settingRepository.ServiceTypeById(model.ServiceTypeId);
                var roleLevel = await GetRoleLevel(serviceType.ServiceOwner);
                if (roleLevel != RoleLevelEnum.Admin)
                {
                    _logger.LogError("SaveRequirementHeader", "Insufficient Privilege");
                    CommandStatus("Insufficient Privilege");
                    return RedirectToAction("Detail", new { id = model.ServiceTypeId });
                }
                //  await _settingRepository.SaveServiceRequirementHeader(model.Id, model.OrderNo, model.Name, model.ServiceTypeId , TheUser.Id);
                CommandStatus(CommandStatusEnum.Saved);

            }
            catch (Exception ex)
            {
                _logger.LogError("SaveRequirementHeader", ex, model);
                CommandStatus(CommandStatusEnum.UnkownError);
            }
            return RedirectToAction("Detail", new { id = model.ServiceTypeId });
        }

        [HttpPost]
        public async Task<IActionResult> SaveRequirementInvoice(RequirementInvoiceViewModel model)
        {
            _logger.LogInformation("SaveRequirementInvoice", "Started", DateTime.UtcNow);
            try
            {
                var serviceType = await _settingRepository.ServiceTypeById(model.ServiceTypeId);
                var roleLevel = await GetRoleLevel(serviceType.ServiceOwner);
                if (roleLevel != RoleLevelEnum.Admin)
                {
                    _logger.LogError("SaveRequirementInvoice", "Insufficient Privilege");
                    CommandStatus("Insufficient Privilege");
                    return RedirectToAction("Detail", new { id = model.ServiceTypeId });
                }

                await _settingRepository.SaveServiceRequirementInvoice(model.Id, model.OrderNo, model.Name,
                    model.NameLocal, model.Description, model.DescriptionLocal, model.ServiceTypeId, model.IsForOpponent, TheUser.Id);
                CommandStatus(CommandStatusEnum.Saved);
                _logger.LogInformation("SaveRequirementInvoice", "End", DateTime.UtcNow);
            }
            catch (Exception ex)
            {
                _logger.LogError("SaveRequirementInvoice", ex, model);
                CommandStatus(CommandStatusEnum.UnkownError);
            }
            return RedirectToAction("Detail", new { id = model.ServiceTypeId });
        }


        [HttpPost]
        public async Task<IActionResult> DeleteRequirement(Guid id, Guid serviceTypeId)
        {
            try
            {
                var serviceType = await _settingRepository.ServiceTypeById(serviceTypeId);
                var roleLevel = await GetRoleLevel(serviceType.ServiceOwner);
                if (roleLevel != RoleLevelEnum.Admin)
                {
                    _logger.LogError("DeleteRequirement", "Insufficient Privilege");
                    CommandStatus("Insufficient Privilege");
                    return RedirectToAction("Detail", new { id = serviceTypeId });
                }
                await _settingRepository.DeleteRequirement(id, serviceTypeId, TheUser.Id);
                CommandStatus(CommandStatusEnum.Deleted);
            }
            catch (Exception ex)
            {
                _logger.LogError("DeleteRequirement", ex, id);
                CommandStatus(CommandStatusEnum.CannotDelete);
            }
            return RedirectToAction("Detail", new { id = serviceTypeId });
        }


        [HttpPost]
        public async Task<IActionResult> DeleteServiceType(Guid id,
        ECCSAModuleFlag module)
        {
            try
            {
                var roleLevel = await GetRoleLevel(module);
                if (roleLevel != RoleLevelEnum.Admin)
                {
                    _logger.LogError("DeleteServiceType", "Insufficient Privilege");
                    CommandStatus("Insufficient Privilege");
                    return RedirectToAction("Index", new { module });
                }
                await _settingRepository.DeleteServiceType(id, TheUser.Id, module);
                CommandStatus(CommandStatusEnum.Deleted);
            }
            catch (Exception ex)
            {
                _logger.LogError("DeleteServiceType", ex, id);
                CommandStatus(CommandStatusEnum.CannotDelete);
            }
            return RedirectToAction("Index", new { module });
        }
    }
}
