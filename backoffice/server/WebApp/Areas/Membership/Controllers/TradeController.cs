
using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Membership;
using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories.Membership;
using ECCSA.BackOffice.Server.Persistence.Repositories.Security;
using ECCSA.BackOffice.Server.WebApp.Areas.Membership.Model;
using ECCSA.BackOffice.Server.WebApp.Controllers;
using ECCSA.BackOffice.Server.WebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Notification;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Membership.Controllers
{
    [Area("Membership")]
    public class TradeController : BaseController
    {
        private readonly ILogger<TradeController> _logger;
        private readonly BusinessInformationRepository _businessInformationRepository;
        public TradeController(ILogger<TradeController> logger,
            UserManager<User> userManager,
        BusinessInformationRepository businessInformationRepository,
        PublicUserRepository userRepository
            , NotificationRepository notificationRepository) : base(userManager, userRepository, notificationRepository)
        {
            _businessInformationRepository = businessInformationRepository;
            _logger = logger;
            controllerName = "Trade";
        }
       

        public async Task<IActionResult> Index(DateTime? endDateAfter, ECCSAModuleFlag module)
        {
            LogStart(_logger, controllerName, "Index");
            var roleLevel = await GetRoleLevel(module);
            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Index");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            endDateAfter ??= DateTime.Today.AddYears(-1);
            var userName = TheUser.FullName;
            var model = new TradingViewModel
            {
                FullName = userName,
                CurrentPage = "Business Information",
                CurrentOperation = "List",
                EndDateAfter = endDateAfter.Value,
                Controller = controllerName,
                TheList = await _businessInformationRepository.BusinessInformationList(endDateAfter.Value)
            };
            
            model.ChangeActive(MenuItemEnum.MembershipTrade);
            TempData["title"] = "Business Information";
            model.RoleLevel = roleLevel;
            model.CurrentModule = module;
            LogEnd(_logger, controllerName, "Index");
            return View(model);
        }
        public async Task<IActionResult> Detail(Guid id, DateTime endDateAfter, ECCSAModuleFlag module)
        {
            _logger.LogInformation("Meeting Detail Started");
            var model = new TradingViewModel();
            model.Controller = controllerName;
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Membership);

            if (roleLevel <= RoleLevelEnum.None && !TheUser.IsHeadOfficeUser)
            {
                LogSecurityError(_logger, controllerName, "Index");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            try
            {
                model.TheModel = await _businessInformationRepository.BusinessInformation(id);
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Business Information Exception {id}");
            }
            if (model.TheModel == null)
            {
                return RedirectToAction("Error", "Home", new { area = "" });
            }
            // model.TheModel.MediaType2Bool();
            model.CurrentPage = "Business Information";
            model.CurrentOperation = "Detail";
            model.RoleLevel = roleLevel;
            model.CurrentModule = module;
            model.ActiveTabIndex = 0;
            model.EndDateAfter = endDateAfter;
            model.ChangeActive(MenuItemEnum.MembershipTrade);
            return View(model);
        }

        private async Task<String> SaveDocument(Guid id, IFormFile file)
        {
            var absolutePath = OtherDocumentAbsolutePath;
            var shortName = Guid2String(id);
            string fileName = file.FileName;
            string extension = Path.GetExtension(Path.GetFileName(fileName));
            var newFileName = String.Concat(shortName, extension);
            await SaveFile(absolutePath, newFileName, file, shortName);

            return extension;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(BusinessInformation model, IFormFile photo, ECCSAModuleFlag module, DateTime endDateAfter)
        {
            LogStart(_logger, controllerName, "Save");
            var roleLevel = await GetRoleLevel(module);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "Save");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("Index");
            }
            try
            {
                if (photo != null && photo.Length > 0)
                {
                    model.Id = model.Id == Guid.Empty ? Guid.NewGuid() : model.Id;
                    model.Extension = Path.GetExtension(Path.GetFileName(photo.FileName));
                    await SaveDocument(model.Id, ECCSAModuleFlag.Membership, photo);
                }
                else
                {
                    model.Extension = String.Empty;
                }
                await _businessInformationRepository.Save(TheUser.Id, model);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Business Information Save");
                CommandStatus(CommandStatusEnum.UnkownError);
            }
            if(model.Id == Guid.Empty)
            {
                return RedirectToAction("Index", new { module, endDateAfter });
            }
            return RedirectToAction("Detail", new { id = model.Id, module, endDateAfter });
        }
        [HttpPost]
        public async Task<IActionResult> Delete(Guid id, ECCSAModuleFlag module, DateTime endDateAfter)
        {
            var roleLevel = await GetRoleLevel(module);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "Save");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("Index", new { module });
            }
            try
            {
                var bInfo = await _businessInformationRepository.BusinessInformation(id);
                await _businessInformationRepository.Delete(TheUser.Id, id);
                if (!string.IsNullOrEmpty(bInfo.Extension))
                {
                    var fname = Guid2String(id) + bInfo.Extension;
                    var path = Path.Combine(AbsolutePath(module), fname);
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                }
                CommandStatus(CommandStatusEnum.Deleted);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Business Information Deleted");
                CommandStatus(CommandStatusEnum.CannotDelete);
            }

            return RedirectToAction("Index", new { module, endDateAfter });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemovePhoto(Guid id, ECCSAModuleFlag module, DateTime endDateAfter)
        {
            if (id == Guid.Empty)
            {
                return RedirectToAction("Index");
            }

            LogStart(_logger, controllerName, "RemovePhoto");
            var roleLevel = await GetRoleLevel(module);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "RemovePhoto");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("Detail", new { id, endDateAfter, module });
            }
            try
            {
                var data = await _businessInformationRepository.BusinessInformation(id);
                if (!string.IsNullOrEmpty(data.Extension))
                {
                    var fname = Guid2String(id) + data.Extension;
                    var path = Path.Combine(AbsolutePath(ECCSAModuleFlag.Membership), fname);
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                }
                data.Extension = "";
                await _businessInformationRepository.Save(TheUser.Id, data, true);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("RemovePhoto", ex, id);
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("RemovePhoto", ex, id);
                CommandStatus(CommandStatusEnum.UnkownError);
            }
            return RedirectToAction("Detail", new { id, endDateAfter, module });
        }
    }
}
