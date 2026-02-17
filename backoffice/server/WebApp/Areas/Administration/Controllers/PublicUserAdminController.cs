using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Common.Servicing;
using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Notification;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Servicing;
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
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Administration.Controllers
{
    [Area("Administration")]
    public class PublicUserAdminController : BaseController
    {
        private readonly ILogger<PublicUserAdminController> _logger;
        private readonly UserAdministrationRepository _repository;
        private readonly IWebHostEnvironment _hostingEnvironment;
        
        private readonly LegalProfileRepository _legalProfileRepository;
        private readonly ExportAgreementRepository _exportAgreementRepository;
        private readonly UserContactRepository _userContactRepository;

        public PublicUserAdminController(ILogger<PublicUserAdminController> logger,
            UserManager<User> userManager,
            UserAdministrationRepository repository,
             IWebHostEnvironment hostingEnvironment,
            PublicUserRepository userRepository,
            UserContactRepository userContactRepository,
            LegalProfileRepository legalProfileRepository,
            ExportAgreementRepository exportAgreementRepository
            , NotificationRepository notificationRepository) : base(userManager, userRepository, notificationRepository)
        {

            _logger = logger;
            _repository = repository;
            _hostingEnvironment = hostingEnvironment;
            _legalProfileRepository = legalProfileRepository;
            _exportAgreementRepository = exportAgreementRepository;

            _userContactRepository = userContactRepository;
            controllerName = "UserAdministration";
        }
        [HttpGet]
        public async Task<IActionResult> Index(PublicUserFilter filter)
        {
            LogStart(_logger, controllerName, "Index");
            var userName = TheUser.FullName;
            PublicUserAdministrationViewModel model;

            if (!TheUser.AssignedModules.HasFlag(ECCSAModuleFlag.Administration))
            {
                _logger.LogError("UserAdministration",
                                 "Index",
                                 "Insufficient Privilege",
                                 TheUser,
                                 DateTime.UtcNow);
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            var list = await _repository.PublicUsers();
            if(filter == PublicUserFilter.EmailMobileNotConfirmed)
            {
                list = list.Where(u => !u.EmailConfirmed || !u.MobileNoConfirmed).ToList();
            }
            else if (filter == PublicUserFilter.EmailChangeNotConfirmed)
            {
                list = list.Where(u => !u.EmailConfirmed && u.OldEmail != null && u.Email != u.OldEmail).ToList();
            }
            else if (filter == PublicUserFilter.MobileChangeNotConfirmed)
            {
                list = list.Where(u => !u.MobileNoConfirmed && u.OldMobileNo != null && u.MobileNo != u.OldMobileNo).ToList();
            }
            model = new()
            {
                FullName = userName,
                CurrentOperation = "List",
                TheList = list,
                TheModel = new User()
                {
                    Id = Guid.Empty,
                },
                Filter = filter,
                RoleLevel = await GetRoleLevel(ECCSAModuleFlag.Administration)
            };
            TempData["title"] = model.CurrentPage;
            LogEnd(_logger, controllerName, "Index");

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(Guid id)
        {
            LogStart(_logger, controllerName, "Detail");
            var userName = TheUser.FullName;
            PublicUserAdministrationViewModel model;
            if (Guid.Empty == id)
            {
                return NotFound();
            }
            model = new PublicUserAdministrationViewModel
            {
                FullName = userName,
                CurrentOperation = "List",
                TheList = new List<User>(),
                TheModel = await _repository.PublicUser(id),
                ExportAgreements = _exportAgreementRepository.UserExportAgreements(id, (int)StatusEnum.Active),
                LegalProfiles = _legalProfileRepository.UserLegalProfiles(id, (int)StatusEnum.Active),
                UserContacts = _userContactRepository.UserContacts(id),
                UserLoginHistories = await _repository.UserLoginHistory(id)
            };
            if (model.TheModel == null)
            {
                return RedirectToAction("Error", "Home", new { area = "" });
            }
            TempData["title"] = model.CurrentPage;
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> UserExportAgreementDetail(Guid id, Guid userId)
        {
            LogStart(_logger, controllerName, "UserExportAgreementDetail");
            var userName = TheUser.FullName;
            ExportAgreement model;
            if (Guid.Empty == id)
            {
                return NotFound();
            }
            model = await _exportAgreementRepository.UserExportAgreement(userId, id);
            return PartialView(model);
        }
        [HttpPost]
        public async Task<IActionResult> DisableUser(Guid id)
        {
            LogStart(_logger, controllerName, "DisableUser");
            try
            {
                await _repository.DisableUser(id);
            }
            catch (Exception ex)
            {
                LogError(_logger, ex, controllerName, "DisableUser", id);
                TempData["CommandStatus"] = ex.Message;
            }
            LogEnd(_logger, controllerName, "DisableUser");
            return RedirectToAction("Detail", new { id });
        }

        [HttpPost]
        public async Task<IActionResult> EnableUser(Guid id)
        {
            LogStart(_logger, controllerName, "EnableUser");
            try
            {
                await _repository.EnableUser(id);
            }
            catch (Exception ex)
            {
                LogError(_logger, ex, controllerName, "EnableUser", id);
                TempData["CommandStatus"] = ex.Message;
            }
            LogEnd(_logger, controllerName, "EnableUser");
            return RedirectToAction("Detail", new { id });
        }
        [HttpPost]
        public async Task<IActionResult> ResetPasswordChange(ChangePasswordModel model, Guid id, ECCSAModuleFlag module)
        {
            var errorMsg = string.Empty;
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                TempData["CommandStatus"] = "User not found.";
                return RedirectToAction("Detail", new { id });
            }
            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
            if (result.Succeeded)
            {
                _logger.LogInformation("Password reset.", user);
                TempData["CommandStatus"] = "Password reset Successfully";
                return RedirectToAction("Detail", new { id });
            }
            foreach (var error in result.Errors)
            {
                errorMsg += (errorMsg != string.Empty ? ", " : "") + error.Description;
            }

            TempData["CommandStatus"] = errorMsg;
            return RedirectToAction("Detail", new { id });
        }[HttpPost]
        public async Task<IActionResult> ConfirmMobileEmail(Guid id)
        {
            var errorMsg = string.Empty;
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                TempData["CommandStatus"] = "User not found.";
                return RedirectToAction("Detail", new { id });
            }
            user.EmailConfirmed = true;
            user.MobileNoConfirmed = true;
            user.EmailConfirmationCode = null;
            user.MobileConfirmationCode = null;
            user.ConfirmationCodeExpiryDate = null;
            user.OldEmail = null;
            user.OldMobileNo = null; 
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                _logger.LogInformation("Email/Mobile Confirmed.", user);
                TempData["CommandStatus"] = "Email/Mobile Confirmed Successfully";
                return RedirectToAction("Detail", new { id });
            }
            foreach (var error in result.Errors)
            {
                errorMsg += (errorMsg != string.Empty ? ", " : "") + error.Description;
            }

            TempData["CommandStatus"] = errorMsg;
            return RedirectToAction("Detail", new { id });
        }
        [HttpGet]
        public async Task<FileResult> DownLoadFile(Guid id, Guid userId, string fileName, string extention)
        {
            return await DownLoadFile(Path.Combine(UserLegalProfileDocumentsAbsolutePath, Guid2String(userId), $"{Guid2String(id)}{extention}"), fileName);
        }
    }
}
