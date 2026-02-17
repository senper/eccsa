using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Notification;
using ECCSA.BackOffice.Server.Model.Common.Notification;
using ECCSA.BackOffice.Server.WebApp.Areas.Notification.Model;
using ECCSA.BackOffice.Server.WebApp.Controllers;
using ECCSA.BackOffice.Server.WebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Persistence.Repositories.Security;
using System.Linq;
using ECCSA.BackOffice.Server.Model.Membership;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Notification.Controllers
{
    [Area("Notification")]
    public class SMSController : BaseController
    {
        private readonly ILogger<SMSController> _logger;
        readonly NotificationRepository _repository;
        private readonly YesNoRepository _lookupRepository;
        public SMSController(ILogger<SMSController> logger, UserManager<User> userManager, NotificationRepository repository, YesNoRepository lookupRepository, PublicUserRepository userRepository, NotificationRepository notificationRepository) : base(userManager, userRepository, notificationRepository)
        {

            _logger = logger;
            _repository = repository;
            _lookupRepository = lookupRepository;
            controllerName = "SMS";
        }

        private static string GetCurrentPage(ECCSAModuleFlag module)
        {
            return module switch
            {
                ECCSAModuleFlag.Academy => "Chamber Academy SMS",
                ECCSAModuleFlag.Administration => "Administration",
                ECCSAModuleFlag.Arbitration => "Arbitration SMS",
                ECCSAModuleFlag.Communication => "Communication SMS",
                ECCSAModuleFlag.Investment => "Trade Promotion SMS",
                ECCSAModuleFlag.Membership => "Membership SMS",
                ECCSAModuleFlag.Reporting => "Reporting SMS",
                ECCSAModuleFlag.Research => "Research & Advocacy SMS",
                ECCSAModuleFlag.TradePromotion => "Trade Facilitation SMS",
                _ => string.Empty,
            };
        }
        public async Task<IActionResult> Index(ECCSAModuleFlag module, MessageStatus searchStatus)
        {
            LogStart(_logger, controllerName, "Index");
            var roleLevel = await GetRoleLevel(module);
            if(roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Index");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            var userName = TheUser.FullName;
            SMSViewModel model;
            if (!TheUser.AssignedModules.HasFlag(module))
            {
                model = new SMSViewModel
                {
                    FullName = userName,
                    CurrentPage = GetCurrentPage(module),
                    CurrentOperation = "List",
                    TheList = new List<SMSMessage>(),
                    TheModel = new SMSMessage()
                    {
                        Id = Guid.Empty,

                    }

                };
                return View(model);
            }
            
            model = new SMSViewModel
            {
                FullName = userName,
                CurrentPage = GetCurrentPage(module),
                CurrentOperation = "List",
                TheModel = new SMSMessage()
                {
                    Id = Guid.Empty,
                }

            };
            model.TheList = await _repository.SMSMessages(module, searchStatus);
            model.CurrentModule = module;
            model.Status = searchStatus;
            model.ChangeActive(MenuItemEnum.SMS);
            model.RoleLevel = roleLevel;
            LogEnd(_logger, controllerName, "Index");
            TempData["title"] = GetCurrentPage(module);
            return View(model);
        }
        public async Task<IActionResult> Save(SMSViewModel vmodel)
        {
            SMSMessage request = new()
            {
                Id = vmodel.Id == Guid.Empty? Guid.NewGuid():vmodel.Id,
                Message = vmodel.Message,
                Status = vmodel.Status,
               Recipients = new List<MessageDelivery>()
            };
            try
            {
     
                await _repository.Save(request, TheUser.Id);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("Save SMS", ex, request);
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Save SMS", ex, request);
                CommandStatus(CommandStatusEnum.UnkownError);
            }
            return RedirectToAction("Index",new { module = vmodel.Owner  });
        }        
        public async Task<IActionResult> Detail(Guid id
            , Guid sectorId
            , StakeholderTypeEnum stakeholderType = StakeholderTypeEnum.Association)
        {
            LogStart(_logger, controllerName, "Index");
            var userName = TheUser.FullName;
            SMSViewModel model;
            if (Guid.Empty == id)
            {
                return NotFound();
            }
            model = new SMSViewModel
            {
                FullName = userName,
                CurrentOperation = "Detail",
                TheList = new List<SMSMessage>(),
                TheModel = await _repository.SMSMessageById(id),
                StakeholderType = stakeholderType,
                SectorId = sectorId
            };
           
            var roleLevel = await GetRoleLevel(model.TheModel.Owner);
            if(roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Detail");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            var lst = await _lookupRepository.LookupTypeById(Persistence.Repositories.Constants.SectorLookupTypeId);
            model.Sectors = lst.LookupValues.ToList();
            model.Sectors.Insert(0, new LookupValue
            {
                Id = Guid.Empty,
                Name = String.Empty,
                Value = Guid.Empty.ToString()
            });
            model.RecipientsSource = await _repository.Recipients(null, sectorId, stakeholderType);
            model.CurrentModule = model.TheModel.Owner;
            model.ChangeActive(MenuItemEnum.Events);
            model.CurrentPage = GetCurrentPage(model.CurrentModule);
            TempData["title"] = model.CurrentPage;
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ChangeStatus(Guid id, MessageStatus status)
        {
            try
            {
                await _repository.ChangeSMSStatus(id, status, TheUser.Id);
                CommandStatus("Successfully Status Saved");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("Status change", ex, id);
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Status change", ex, id);
                CommandStatus(CommandStatusEnum.UnkownError);
            }
            return RedirectToAction("Detail", new { id });
        }
        public async Task<IActionResult> AddRecipient(Guid messageId, string mobile, string label)
        {

            _logger.LogInformation($"Add Recipient {mobile}");
            try
            {
                await _repository.AddRecipient(messageId, mobile, label, CcGroup.To, TheUser.Id);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("Save SMS", ex, mobile);
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Save SMS");
            }
            return RedirectToAction("Detail", new { Id = messageId });
        }
        public async Task<IActionResult> RemoveRecipient(Guid messageId, string mobile)
        {

            _logger.LogInformation($"Remove Recipient {mobile}");
            try
            {
                await _repository.RemoveRecipient(messageId, mobile, TheUser.Id);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("Remove SMS Recipient", ex, mobile);
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Remove SMS Recipient");
            }
            return RedirectToAction("Detail", new { Id = messageId });
        }
        public async Task<IActionResult> Delete(Guid id, ECCSAModuleFlag module)
        {

            SMSMessage request = new()
            {
                Id = id,
                CreatedBy = TheUser.Id

            };
            try
            {

                await _repository.Delete(request);
                CommandStatus(CommandStatusEnum.Deleted);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("Delete SMS", ex, request);
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Delete SMS", ex, request);
                CommandStatus(CommandStatusEnum.UnkownError);
            }
            return RedirectToAction("Index", new { module });
        }
        [HttpPost]
        public async Task<IActionResult> AddRecipents(Guid id, string selectedList, StakeholderTypeEnum stakeholderType, Guid sectorId)
        {
            var msg = await _repository.SMSMessageById(id);
            var list = selectedList?.Split(';');
            var idList = ConvertToGuidList(list);
            var recipientsSources = await _repository.Recipients(msg.Owner, Guid.Empty, stakeholderType);
            var selected = recipientsSources.Where(s => idList.Any(i => i == s.Id) && !msg.Recipients.Any(r => r.Address == s.Email)).ToList();
            if (!selected.Any())
            {
                CommandStatus("Nothing New Selected to add.");
                return RedirectToAction("Detail", new { id, stakeholderType });
            }
            //
            msg.Recipients.AddRange(selected.Select(s => new MessageDelivery
            {
                Address = s.MobileNo,
                Status = DeliveryStatus.NotSet,
                Label = s.Name
            }));
            try
            {
                await _repository.Save(msg, TheUser.Id);
                CommandStatus("Successfully Recipients Added");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("Recipient Added", ex, msg);
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Recipient Added", ex, msg);
                CommandStatus(CommandStatusEnum.UnkownError);
            }
            return RedirectToAction("Detail", new { id, stakeholderType, sectorId });
        }
        [HttpPost]
        public async Task<IActionResult> RemoveRecipents(Guid id, string selectedList, StakeholderTypeEnum stakeholderType, Guid sectorId)
        {
            var msg = await _repository.SMSMessageById(id);
            var list = selectedList?.Split(';');
            if (list == null || !list.Any())
            {
                CommandStatus("Nothing Selected to remove.");
                return RedirectToAction("Detail", new { id, stakeholderType });
            }
            //
            msg.Recipients.RemoveAll(r => list.Contains(r.Address));
            try
            {
                await _repository.Save(msg, TheUser.Id);
                CommandStatus("Successfully Recipient Removed");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("Recipient Remove", ex, msg);
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Recipient Remove", ex, msg);
                CommandStatus(CommandStatusEnum.UnkownError);
            }
            return RedirectToAction("Detail", new { id, stakeholderType, sectorId });
        }

        private static List<Guid> ConvertToGuidList(string[] list)
        {
            if (list == null) return new List<Guid>();
            var idList = new List<Guid>();
            if (list != null)
            {
                foreach (var l in list)
                {
                    if (Guid.TryParse(l, out Guid nid))
                    {
                        idList.Add(nid);
                    }
                }
            }

            return idList;
        }
        [HttpPost]
        public async Task<IActionResult> SendSMS(Guid id)
        {
            var msg = await _repository.SMSMessageById(id);
            try
            {
                try
                {
                    
                }
                catch (Exception ex)
                {
                    _logger.LogError("SendSMS", ex);
                    msg.Recipients.ForEach(r => r.Status = DeliveryStatus.NotDelivered);
                }
                await _repository.Save(msg, TheUser.Id);
                CommandStatus("Successfully SMS Sent");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("Send SMS", ex, msg);
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Send SMS", ex, msg);
                CommandStatus(CommandStatusEnum.UnkownError);
            }
            return RedirectToAction("Detail", new { id });
        }
    }
}
