using ECCSA.BackOffice.Server.Model.BusinessDirectory;
using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories.BusinessDirectory;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common;
using ECCSA.BackOffice.Server.Persistence.Repositories.Security;
using ECCSA.BackOffice.Server.WebApp.Areas.BusinessDirectory.Model;
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

namespace ECCSA.BackOffice.Server.WebApp.Areas.BusinessDirectory.Controllers
{
    [Area("BusinessDirectory")]
    public class BusinessInterestController : BaseController
    {
        private readonly ILogger<BusinessInterestController> _logger;
        private readonly BusinessInterestRepository _repository;
        private readonly BusinessRepository _businessrepository;
        private readonly YesNoRepository _lookupRepository;
        public BusinessInterestController(
            UserManager<User> userManager
            , ILogger<BusinessInterestController> logger
            , BusinessInterestRepository repository
            , YesNoRepository lookuprepository
            , BusinessRepository businessrepository
            , PublicUserRepository userRepository, NotificationRepository notificationRepository) : base(userManager, userRepository, notificationRepository)
        {
            _logger = logger;
            _repository = repository;
            _lookupRepository = lookuprepository;
            _businessrepository = businessrepository;
            controllerName = "BusinessInterest";
        }
        [HttpGet]
        public async Task<IActionResult> Index(Guid? sectorId = null, ECCSAModuleFlag ownerModule = ECCSAModuleFlag.Membership)
        {
            var model = new BusinessInterestViewModel();
            LogStart(_logger, controllerName, "Index");
            
            var roleLevel = await GetRoleLevel(ownerModule);
            if (roleLevel <= RoleLevelEnum.None )
            {
                LogSecurityError(_logger, controllerName, "Index");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            try
            {
                model.TheList = await _repository.BusinessInterestSources(sectorId);
                model.Sectors = await _repository.RootSectorsWithSources();
            }
            catch (Exception ex)
            {
                model.TheList = new List<BusinessInterestSource>();
                _logger.LogError(ex, "Business Interest Exception");
            }
            model.CurrentPage = "Business Interest Sources";
            model.CurrentOperation = "Index";
            model.ChangeActive(MenuItemEnum.BusinessInterestSource);
            model.CurrentModule = ownerModule;
            model.RoleLevel = roleLevel;
            model.SectorId = sectorId;
            LogEnd(_logger, controllerName, "Index");
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> SaveSource(BusinessInterestSource theModel, ECCSAModuleFlag ownerModule)
        {
            var id = theModel.Id;
            LogStart(_logger, controllerName, "SaveSource");
            var roleLevel = await GetRoleLevel(ownerModule);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "SaveSource");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("Index", new { ownerModule });
            }
            try
            {
                await _repository.SaveSource(theModel);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("SaveSource", ex, theModel);
                CommandStatus(ex.Message);
                return RedirectToAction("Index", new { ownerModule });
            }
            catch (Exception ex)
            {
                _logger.LogError("SaveSource", ex, theModel);
                CommandStatus(CommandStatusEnum.UnkownError);
                return RedirectToAction("Index", new { ownerModule });
            }
            if (id == Guid.Empty)
                return RedirectToAction("Index", new {ownerModule});
            else
                return RedirectToAction("SourceDetail", new { id = theModel.Id, ownerModule });
        }

        [HttpGet]
        public async Task<IActionResult> SourceDetail(Guid id, int activeTabIndex = 0, Guid? sectorId = null, ECCSAModuleFlag ownerModule = ECCSAModuleFlag.Membership)
        {
            LogStart(_logger, controllerName, "SourceDetail");
            var roleLevel = await GetRoleLevel(ownerModule);
            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "PhotoAlbumDetail");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            var model = new BusinessInterestViewModel();
            try
            {
                model.TheModel = await _repository.BusinessInterestSource(id);
                model.CurrentModule = ECCSAModuleFlag.Membership;
                model.BusinessTypes = (await _lookupRepository.LookupTypeById(new Guid("00000000-0000-0000-0000-00000000002e")))
                    .LookupValues.ToList();
                model.Products = await _repository.BusinessInterestProducts();
                if (sectorId.HasValue)
                {
                    model.Sectors = await _businessrepository.ChildSectors(sectorId.Value);
                }
                else
                {
                    model.Sectors = await _businessrepository.RootSectors();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Photo Album Detail Exception {id}");
            }

            model.CurrentPage = "Business Interest Sources";
            model.CurrentOperation = "Detail";
            model.ChangeActive(MenuItemEnum.BusinessInterestSource);
            model.RoleLevel = roleLevel;
            model.SectorId = sectorId;
            model.ActiveTabIndex = activeTabIndex;
            model.CurrentModule = ownerModule;
            LogEnd(_logger, controllerName, "SourceDetail");
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteSource(Guid id, ECCSAModuleFlag ownerModule)
        {
            LogStart(_logger, controllerName, "DeleteSource");
            var roleLevel = await GetRoleLevel(ownerModule);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "DeleteSource");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("Index");
            }
            try
            {
                await _repository.DeleteSource(id);
                CommandStatus(CommandStatusEnum.Deleted);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("DeleteSource", ex, id);
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("DeleteSource", ex, id);
                CommandStatus(CommandStatusEnum.UnkownError);
            }

            return RedirectToAction("Index", new { ownerModule });
        }

        [HttpPost]
        public async Task<IActionResult> AddSourceSector(Guid id, Guid sectorId, Guid parentSectorId, ECCSAModuleFlag ownerModule)
        {
            LogStart(_logger, controllerName, "AddSourceSector");
            var roleLevel = await GetRoleLevel(ownerModule);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "AddSourceSector");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("Index");
            }
            try
            {
                await _repository.AddSourceSector(id, sectorId);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("AddSourceSector", ex, sectorId);
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("AddSourceSector", ex, sectorId);
                CommandStatus(CommandStatusEnum.UnkownError);
            }
            return RedirectToAction("SourceDetail", new { id, sectorId = parentSectorId, ownerModule });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveSourceSector(Guid id, Guid sourceId, Guid parentSectorId, ECCSAModuleFlag ownerModule)
        {
            LogStart(_logger, controllerName, "RemoveSourceSector");
            var roleLevel = await GetRoleLevel(ownerModule);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "RemoveSourceSector");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("SourceDetail", new { id = sourceId, sectorId = parentSectorId });
            }
            try
            {
                await _repository.RemoveSourceSector(id);
                CommandStatus(CommandStatusEnum.Deleted);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("RemoveSourceSector", ex, id);
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("RemoveSourceSector", ex, id);
                CommandStatus(CommandStatusEnum.UnkownError);
            }

            return RedirectToAction("SourceDetail", new { id = sourceId, sectorId = parentSectorId, ownerModule });
        }

        [HttpPost]
        public async Task<IActionResult> SaveSourceProductType(BusinessInterestSourceDetail model, Guid parentSectorId, ECCSAModuleFlag ownerModule)
        {
            LogStart(_logger, controllerName, "SaveSourceProductType");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Membership);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "SaveSourceProductType");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("SourceDetail", new { id = model.BusinessInterestSourceId, activeTabIndex = 1, sectorId = parentSectorId, ownerModule });
            }
            try
            {
                await _repository.SaveSourceProductType(model);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("SaveSourceProductType", ex, model);
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("SaveSourceProductType", ex, model);
                CommandStatus(CommandStatusEnum.UnkownError);
            }
            return RedirectToAction("SourceDetail", new { id = model.BusinessInterestSourceId, activeTabIndex = 1, sectorId = parentSectorId, ownerModule });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSourceProductType(Guid id, Guid sourceId, Guid parentSectorId, ECCSAModuleFlag ownerModule)
        {
            LogStart(_logger, controllerName, "DeleteSourceProductType");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Membership);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "DeleteSourceProductType");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("SourceDetail", new { id = sourceId, activeTabIndex = 1, sectorId = parentSectorId });
            }
            try
            {
                await _repository.DeleteSourceProductType(id);
                CommandStatus(CommandStatusEnum.Deleted);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("DeleteSourceProductType", ex, id);
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("DeleteSourceProductType", ex, id);
                CommandStatus(CommandStatusEnum.UnkownError);
            }

            return RedirectToAction("SourceDetail", new { id = sourceId, activeTabIndex = 1, sectorId = parentSectorId , ownerModule });
        }

        [HttpGet]
        public async Task<IActionResult> BusinessInterestProducts(ECCSAModuleFlag ownerModule)
        {
            var model = new BusinessInterestProductViewModel();
            LogStart(_logger, controllerName, "BusinessInterestProducts");
            var roleLevel = await GetRoleLevel(ownerModule);
            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "BusinessInterestProducts");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            try
            {
                model.TheList = await _repository.BusinessInterestProducts();
            }
            catch (Exception ex)
            {
                model.TheList = new List<BusinessInterestProduct>();
                _logger.LogError(ex, "Business Interest Exception");
            }
            model.CurrentPage = "Business Interest Products";
            model.CurrentOperation = "List";
            model.RoleLevel = roleLevel;
            model.ChangeActive(MenuItemEnum.BusinessInterestProduct);
            model.CurrentModule =  ownerModule;
            model.RoleLevel = roleLevel;
            LogEnd(_logger, controllerName, "BusinessInterestProducts");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteBusinessInterestProduct(Guid id, ECCSAModuleFlag ownerModule)
        {
            LogStart(_logger, controllerName, "DeleteSourceProductType");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Membership);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "DeleteSourceProductType");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("BusinessInterestProducts");
            }
            try
            {
                await _repository.DeleteBusinessInterestProduct(id);
                CommandStatus(CommandStatusEnum.Deleted);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("DeleteBusinessInterestProduct", ex, id);
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("DeleteBusinessInterestProduct", ex, id);
                CommandStatus(CommandStatusEnum.UnkownError);
            }

            return RedirectToAction("BusinessInterestProducts", new {ownerModule});
        }
        [HttpPost]
        public async Task<IActionResult> SaveBusinessInterestProduct(BusinessInterestProduct model, ECCSAModuleFlag ownerModule)
        {
            LogStart(_logger, controllerName, "SaveBusinessInterestProduct");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Membership);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "SaveBusinessInterestProduct");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("BusinessInterestProducts", new {ownerModule});
            }
            try
            {
                await _repository.SaveBusinessInterestProduct(model);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("SaveBusinessInterestProduct", ex, model);
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("SaveBusinessInterestProduct", ex, model);
                CommandStatus(CommandStatusEnum.UnkownError);
            }
                return RedirectToAction("BusinessInterestProducts", new {ownerModule});
        }
    }
}
