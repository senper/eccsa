using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Common.EventManagement;
using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Events;
using ECCSA.BackOffice.Server.Persistence.Repositories.Security;
using ECCSA.BackOffice.Server.WebApp.Areas.Events.Model;
using ECCSA.BackOffice.Server.WebApp.Controllers;
using ECCSA.BackOffice.Server.WebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Notification;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Events.Controllers
{
    [Area("Events")]
    public class EventTypeController : BaseController
    {
        private readonly ILogger<EventTypeController> _logger;
        private readonly EventTypeRepository _repository;
        public EventTypeController(ILogger<EventTypeController> logger,
            UserManager<User> userManager,
            EventTypeRepository repository,
            PublicUserRepository userRepository
            , NotificationRepository notificationRepository) : base(userManager, userRepository, notificationRepository)
        {

            _logger = logger;
            _repository = repository;
        }

        private string GetCurrentPage(ECCSAModuleFlag module)
        {
            return module switch
            {
                ECCSAModuleFlag.Academy => "Chamber Academy Event Types",
                ECCSAModuleFlag.Arbitration => "Arbitration Event Types",
                ECCSAModuleFlag.Communication => "Communication Event Types",
                ECCSAModuleFlag.Investment => "Trade Promotion Event Types",
                ECCSAModuleFlag.Membership => "Membership Event Types",
                ECCSAModuleFlag.Reporting => "Reporting Event Types",
                ECCSAModuleFlag.Research => "Research & Advocacy Event Types",
                ECCSAModuleFlag.TradePromotion => "Trade Facilitation Event Types",
                _ => string.Empty,
            };
        }
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation($"Event Types ");
            var userName = TheUser.FullName;
            EventTypeViewModel model;
            if (!TheUser.AssignedModules.HasFlag(ECCSAModuleFlag.Administration))
            {
                model = new EventTypeViewModel
                {
                    FullName = userName,
                    CurrentPage = "Event Type",
                    CurrentOperation = "List",
                    TheList = new List<EventType>(),
                    TheModel = new EventType()
                    {
                        EventTypeId = Guid.Empty,
                        Status = StatusEnum.Active,

                    }

                };
                return View(model);
            }
            model = new EventTypeViewModel
            {
                FullName = userName,
                CurrentPage = "Event Type",
                CurrentOperation = "List",
                TheList = await _repository.AllEventTypes(),
                CurrentModule = ECCSAModuleFlag.Administration,
                TheModel = new EventType()
                {
                    EventTypeId = Guid.Empty,
                    Status = StatusEnum.Active,
                }

            };

            model.ChangeActive(MenuItemEnum.EventType);
           
            TempData["title"] = "Event Type";
            return View(model);
        }


        public async Task<IActionResult> Detail(Guid id)
        {
            _logger.LogInformation($"DetailEventType {id}");
            var userName = TheUser.FullName;
            EventTypeViewModel model;
            if (Guid.Empty == id)
            {
                return NotFound();
            }
            model = new EventTypeViewModel
            {
                FullName = userName,
                CurrentOperation = "List",
                TheList = new List<EventType>(),
                TheModel = await _repository.EventTypeById(id),
                CurrentModule = ECCSAModuleFlag.Administration,
            };

            if (model.TheModel == null)
            {
                return RedirectToAction("Error", "Home", new { area = "" });
            }
            model.ChangeActive(MenuItemEnum.EventType);
            model.CurrentPage = "Event Type";
            TempData["title"] = model.CurrentPage;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveEventType(EventType model)
        {
            _logger.LogInformation($"SaveEventType {model}");
            try
            {
                await _repository.Save(model, TheUser.Id);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (Exception ex)
            {
                _logger.LogError("SaveEventType", ex, model);
                CommandStatus(CommandStatusEnum.UnkownError);
            }
            if (model.EventTypeId == Guid.Empty)
            {
                return RedirectToAction("Index");
            }else
            {
                return RedirectToAction("Detail", new {id= model.EventTypeId });
            }
        }



    
        [HttpPost]
        public async Task<IActionResult> DeleteEventType(Guid id)
        {
            _logger.LogInformation($"DeleteEventType {id}");
            try
            {
                await _repository.DeleteEventType (id);
                CommandStatus(CommandStatusEnum.Deleted);
            }
            catch (Exception ex)
            {
                _logger.LogError("DeleteEventType", ex, id);
                CommandStatus(CommandStatusEnum.CannotDelete);
            }
            return RedirectToAction("Index");
        }
    }
}
