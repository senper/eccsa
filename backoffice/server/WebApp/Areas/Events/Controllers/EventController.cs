using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Common.EventManagement;
using ECCSA.BackOffice.Server.Model.Common.Servicing;
using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Events;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Notification;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Servicing;
using ECCSA.BackOffice.Server.Persistence.Repositories.Security;
using ECCSA.BackOffice.Server.WebApp.Areas.Events.Model;
using ECCSA.BackOffice.Server.WebApp.Controllers;
using ECCSA.BackOffice.Server.WebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
namespace ECCSA.BackOffice.Server.WebApp.Areas.Events.Controllers
{
    [Area("Events")]
    public class EventController : BaseController
    {
        private readonly ILogger<EventController> _logger;
        private readonly EventRepository _repository;
        private readonly EventTypeRepository _eventTypeRepository;
        private readonly ServiceTypeRepository _settingRepository;
        public EventController(ILogger<EventController> logger,
            UserManager<User> userManager,
            EventRepository repository,
            ServiceTypeRepository settingRepository,
            EventTypeRepository eventTypeRepository,
            PublicUserRepository userRepository, NotificationRepository notificationRepository) : base(userManager, userRepository, notificationRepository)
        {
            
            _logger = logger;
            _repository = repository;
            _settingRepository = settingRepository;
            _eventTypeRepository = eventTypeRepository;
            controllerName = "Event";
        }

        private string GetCurrentPage(ECCSAModuleFlag module)
        {
            return module switch
            {
                ECCSAModuleFlag.Academy => "Chamber Academy Events",
                ECCSAModuleFlag.Arbitration => "Arbitration Events",
                ECCSAModuleFlag.Communication => "Communication Events",
                ECCSAModuleFlag.Investment => "Trade Promotion Events",
                ECCSAModuleFlag.Membership => "Membership Events",
                ECCSAModuleFlag.Reporting => "Reporting Events",
                ECCSAModuleFlag.Research => "Research & Advocacy Events",
                ECCSAModuleFlag.TradePromotion => "Trade Facilitation Events",
                _ => string.Empty,
            };
        }
        public async Task<IActionResult> Index(ECCSAModuleFlag module, EventStatus searchStatus)
        {
            LogStart(_logger, controllerName, "Index");
            var roleLevel = await GetRoleLevel(module);
            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Index");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            var userName = TheUser.FullName;
            EventViewModel model;
            if (!TheUser.AssignedModules.HasFlag(module))
            {
                model = new EventViewModel
                {
                    FullName = userName,
                    CurrentPage = GetCurrentPage(module),
                    CurrentOperation = "List",
                    EventTypes = new List<EventType>(),
                    TheList = new List<Event>(),
                    TheModel = new Event()
                    {
                        Id = Guid.Empty,

                    }

                };
                return View(model);
            }

            model = new EventViewModel
            {
                FullName = userName,
                CurrentPage = GetCurrentPage(module),
                CurrentOperation = "List",
                TheModel = new Event()
                {
                    Id = Guid.Empty,
                }

            };
            model.EventTypes = await _eventTypeRepository.EventTypes();
            if (searchStatus == EventStatus.NotSet)
            {
                model.TheList = await _repository.Events(module);
            }
            else
            {
                model.TheList = await _repository.Events(module, searchStatus);
            }
            model.CurrentModule = module;
            model.Status = searchStatus;
            model.ChangeActive(MenuItemEnum.Events);
            TempData["title"] = GetCurrentPage(module);
            model.RoleLevel = roleLevel;
            LogEnd(_logger, controllerName, "Index");
            return View(model);
        }

        public async Task<IActionResult> Edit(Guid id, ECCSAModuleFlag module)
        {
            var userName = TheUser.FullName;
            EventViewModel model;
            model = new EventViewModel
            {
                FullName = userName,
                CurrentPage = GetCurrentPage(module),
                CurrentOperation = "Detail",
                TheList = new List<Event>(),


            };
            if (Guid.Empty == id)
            {
                model.TheModel = new Event()
                {
                    Id = Guid.Empty,

                };
            }
            else
            {
                model.TheModel = await _repository.EventById(id);
            }
            model.ChangeActive(MenuItemEnum.Events);
            model.CurrentModule = module;
            TempData["title"] = GetCurrentPage(module);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveServiceType(EventViewModel vmodel)
        {
            try
            {
                var eventDetail = await _repository.DetailById(vmodel.DetailId);

                var model = ServiceType.FromEvent(eventDetail, vmodel.RateBasis, vmodel.MemberServiceCharge, vmodel.NonMemberServiceCharge, TheUser.Id);
                await _settingRepository.SaveEventServiceType(model);
                vmodel.EventId = eventDetail.EventId;
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (Exception ex)
            {
                _logger.LogError("SaveSetting", ex, vmodel);
                CommandStatus(CommandStatusEnum.UnkownError);
            }
            return RedirectToAction("Detail", new { id = vmodel.EventId });
        }

        public async Task<IActionResult> Save(EventViewModel vmodel)
        {

            Event request = new()
            {
                Id = vmodel.Id == Guid.Empty ? Guid.NewGuid() : vmodel.Id,
                EventTypeId = vmodel.EventTypeId,
                EventOwner = vmodel.EventOwner,
                Title = vmodel.Title,
                Description = vmodel.Description,
                StartDate = vmodel.StartDate,
                EndDate = vmodel.EndDate,
                Status = vmodel.Status
            };
            try
            {
                if (vmodel.Photo != null && vmodel.Photo.Length > 0)
                {
                    string extension = Path.GetExtension(Path.GetFileName(vmodel.Photo.FileName));
                    if(extension != ".jpg")
                    {
                        CommandStatus("Only jpg image is allowed.");
                        return RedirectToAction("Index", new { module = vmodel.EventOwner });
                    }
                    await SaveDocument(request.Id, vmodel.EventOwner, vmodel.Photo);
                }
                await _repository.Save(request, TheUser.Id);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("Save Event", ex, request);
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Save Event", ex, request);
                CommandStatus(CommandStatusEnum.UnkownError);
            }
            if (request.Id == Guid.Empty)
                return RedirectToAction("Index", new { module = vmodel.EventOwner });
            else
                return RedirectToAction("Detail", new { id = request.Id });
        }

        public async Task<IActionResult> SaveDetail(EventDetail detail)
        {
            var ev = await _repository.EventById(detail.EventId);
            _logger.LogInformation($"Save Event Detail {detail}");
            LogStart(_logger, controllerName, "Save");
            var roleLevel = await GetRoleLevel(ev.EventOwner);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "SaveDetail");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("Detail", new { id = ev.Id });
            }
            try
            {
                await _repository.Save(detail, TheUser.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Save Event Detail");
            }
            return RedirectToAction("Detail", new { Id = detail.EventId });
        }
        public async Task<IActionResult> Detail(Guid id)
        {
            LogStart(_logger, controllerName, "Detail");
            var userName = TheUser.FullName;
            EventViewModel model;
            if (Guid.Empty == id)
            {
                return NotFound();
            }
            model = new EventViewModel
            {
                FullName = userName,
                CurrentOperation = "Detail",
                TheList = new List<Event>(),
                TheModel = await _repository.EventById(id)

            };
            if (model.TheModel == null)
            {
                CommandStatus("Event not found");
                return RedirectToAction("Index", new { module = TheUser.DefaultModule });
            }
            model.CurrentModule = model.TheModel.EventOwner;
            
            var roleLevel = await GetRoleLevel(model.CurrentModule);
            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Index");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            if (model.TheModel == null)
            {
                return RedirectToAction("Error", "Home", new { area = "" });
            }
            model.ChangeActive(MenuItemEnum.Events);
            model.CurrentPage = GetCurrentPage(model.CurrentModule);
            TempData["title"] = model.CurrentPage;
            model.RoleLevel = roleLevel;
            LogEnd(_logger, controllerName, "Detail");
            return View(model);
        }




        [HttpGet]
        public async Task<FileResult> DownLoadFile(Guid id, ECCSAModuleFlag module, string title)
        {
            var fileName = Guid2String(id);
            var filePath = AbsolutePath(module);
            var extension = GetExtension(filePath, fileName);

            fileName += extension;
            title += extension;
            return await DownLoadFile(filePath + "\\" + fileName, title);
        }



    }
}
