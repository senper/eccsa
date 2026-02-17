using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Common.Notification;
using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Notification;
using ECCSA.BackOffice.Server.Persistence.Repositories.Security;
using ECCSA.BackOffice.Server.WebApp.Areas.Notification.Model;
using ECCSA.BackOffice.Server.WebApp.Controllers;
using ECCSA.BackOffice.Server.WebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Notification.Controllers
{
    [Area("Notification")]
    public class EmailTemplateController : BaseController
    {
        private readonly ILogger<EmailTemplateController> _logger;
        readonly NotificationRepository _repository;
        public EmailTemplateController(ILogger<EmailTemplateController> logger, UserManager<User> userManager, NotificationRepository repository, PublicUserRepository userRepository, NotificationRepository notificationRepository) : base(userManager, userRepository, notificationRepository)
        {

            _logger = logger;
            _repository = repository;
            controllerName = "EmailTemplate";
        }

        public async Task<IActionResult> Index(ECCSAModuleFlag module)
        {
            LogStart(_logger, controllerName, "Index");
            var roleLevel = await GetRoleLevel(module);
            if(roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Index");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            var userName = TheUser.FullName;
            EmailTemplateViewModel model;
            if (!TheUser.AssignedModules.HasFlag(ECCSAModuleFlag.Administration))
            {
                model = new EmailTemplateViewModel
                {
                    FullName = userName,
                    CurrentPage = "Email Template",
                    CurrentOperation = "List",
                    TheList = new List<EmailTemplate>(),
                    TheModel = new EmailTemplate()
                    {
                        Id = Guid.Empty,
                    }

                };
                return View(model);
            }
            model = new EmailTemplateViewModel
            {
                FullName = userName,
                CurrentPage = "Email Template",
                CurrentOperation = "List",
                TheList = await _repository.EmailTemplates(module),
                TheModel = new EmailTemplate()
                {
                    Id = Guid.Empty,
                }

            };

            model.ChangeActive(MenuItemEnum.EmailTemplate);
           model.RoleLevel = roleLevel;
            LogEnd(_logger, controllerName, "Index");
            TempData["title"] = "Email Template";
            return View(model);
        }


        public async Task<IActionResult> Detail(Guid id)
        {
            _logger.LogInformation($"DetailEventType {id}");
            var userName = TheUser.FullName;
            EmailTemplateViewModel model;
            if (Guid.Empty == id)
            {
                return NotFound();
            }
            model = new EmailTemplateViewModel
            {
                FullName = userName,
                CurrentOperation = "List",
                TheList = new List<EmailTemplate>(),
                TheModel = await _repository.EmailTemplateById(id)

            };


            model.ChangeActive(MenuItemEnum.EmailTemplate);
            model.CurrentPage = "Email Template";
            TempData["title"] = model.CurrentPage;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Save(EmailTemplateViewModel model)
        {
            _logger.LogInformation($"SaveEmailTemplate {model}");
            try
            {
                await _repository.Save(model.TheModel, TheUser.Id);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (Exception ex)
            {
                _logger.LogError("SaveEmailTemplate", ex, model);
                CommandStatus(CommandStatusEnum.UnkownError);
            }
            return RedirectToAction("Index");
        }



    
        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            _logger.LogInformation($"Delete {id}");
            try
            {
                await _repository.Delete (new EmailTemplate() { Id = id, CreatedBy = TheUser.Id});
                CommandStatus(CommandStatusEnum.Deleted);
            }
            catch (Exception ex)
            {
                _logger.LogError("Delete", ex, id);
                CommandStatus(CommandStatusEnum.CannotDelete);
            }
            return RedirectToAction("Index");
        }
    }
}
