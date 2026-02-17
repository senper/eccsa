using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Model.Investment;
using ECCSA.BackOffice.Server.Persistence.Repositories.Investment;
using ECCSA.BackOffice.Server.WebApp.Controllers;
using Microsoft.AspNetCore.Identity;
using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Notification;
using ECCSA.BackOffice.Server.WebApp.Areas.Investment.Model;
using ECCSA.BackOffice.Server.WebApp.Models;
using ECCSA.BackOffice.Server.Persistence.Repositories.Security;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Investment.Controllers
{
    [Area("Investment")]
    public class IndexController : BaseController
    {
        private readonly ILogger<IndexController> _logger;
        private readonly InvestmentRepository _repository;

        public IndexController(ILogger<IndexController> logger,
            UserManager<User> userManager,
            InvestmentRepository repository,
            PublicUserRepository userRepository, NotificationRepository notificationRepository) : base(userManager, userRepository, notificationRepository)
        {
            _logger = logger;
            _repository = repository;
        }
        public async Task<IActionResult> Index(string searchTerm)
        {
            var userName = TheUser.FullName;
            BusinessViewModel model;
            if (!TheUser.AssignedModules.HasFlag(Server.Model.Common.ECCSAModuleFlag.Investment))
            {
                model = new BusinessViewModel
                {
                    FullName = userName,
                    CurrentPage = "Business",
                    CurrentOperation = "List",
                    TheList = new List<Business>(),
                    TheModel = new Business()
                    {
                        Id = Guid.Empty,

                    }

                };
                return View(model);
            }
            model = new BusinessViewModel
            {
                FullName = userName,
                CurrentPage = "Business",
                CurrentOperation = "List",
                TheList = string.IsNullOrEmpty(searchTerm)? await _repository.Businesses(): await _repository.Search(searchTerm),
                TheModel = new Business()
                {
                    Id = Guid.Empty,
                }

            };

            model.ChangeActive(MenuItemEnum.InvestmentBusiness);
            model.CurrentModule = Server.Model.Common.ECCSAModuleFlag.Investment;
            TempData["title"] = "Business Interests";
            return View(model);
        }


        public async Task<IActionResult> Detail(Guid id)
        {
            var userName = TheUser.FullName;
            BusinessViewModel model;
            if (Guid.Empty == id)
            {
                return NotFound();
            }
            model = new BusinessViewModel
            {
                FullName = userName,
                CurrentPage = "Trade Promotion",
                CurrentOperation = "List",
                TheList = new List<Business>(),
                TheModel = await _repository.Business(id)

            };

            if (model.TheModel == null)
            {
                return RedirectToAction("Error", "Home", new { area = "" });
            }
            model.ChangeActive(MenuItemEnum.InvestmentBusiness);
            model.CurrentModule = Server.Model.Common.ECCSAModuleFlag.Investment;
            model.CurrentPage = "Business Interest";
            TempData["title"] = "Business Interest";
            return View(model);
        }

        
        [HttpPost]
        public async Task<IActionResult> Save(Business model)
        {
            try
            {
                await _repository.Save(model, TheUser.Id);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (Exception ex)
            {
                _logger.LogError("Save", ex, model);
                CommandStatus(CommandStatusEnum.UnkownError);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _repository.DeleteBusiness(id);
                CommandStatus(CommandStatusEnum.Deleted);
            }
            catch (Exception ex)
            {
                _logger.LogError("Save", ex, id);
                CommandStatus(CommandStatusEnum.UnkownError);
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> ConfirmPhone(Guid id, string code)
        {
            try
            {
                await _repository.ConfirmPhone(id,code, TheUser.Id);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (Exception ex)
            {
                _logger.LogError("ConfirmPhone", ex, new {id, code });
                CommandStatus(CommandStatusEnum.UnkownError);
            }
            return RedirectToAction("Detail", new { id});
        }

    }
}
