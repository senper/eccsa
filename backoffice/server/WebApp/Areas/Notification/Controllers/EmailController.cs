using ClosedXML;
using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Common.Notification;
using ECCSA.BackOffice.Server.Model.Membership;
using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Notification;
using ECCSA.BackOffice.Server.Persistence.Repositories.Security;
using ECCSA.BackOffice.Server.WebApp.Areas.Notification.Model;
using ECCSA.BackOffice.Server.WebApp.Controllers;
using ECCSA.BackOffice.Server.WebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
namespace ECCSA.BackOffice.Server.WebApp.Areas.Notification.Controllers
{
    [Area("Notification")]
    public class EmailController : BaseController
    {
        private readonly ILogger<EmailController> _logger;
        private readonly NotificationRepository _repository;
        private readonly YesNoRepository _lookupRepository;
        public EmailController(ILogger<EmailController> logger, UserManager<User> userManager, NotificationRepository repository, YesNoRepository lookupRepository, PublicUserRepository userRepository, NotificationRepository notificationRepository) : base(userManager, userRepository, notificationRepository)
        {

            _logger = logger;
            _repository = repository;
            _lookupRepository = lookupRepository;
            controllerName = "Email";
        }

        private static string GetCurrentPage(ECCSAModuleFlag module) => module switch
        {
            ECCSAModuleFlag.Academy => "Chamber Academy Notifications",
            ECCSAModuleFlag.Arbitration => "Arbitration Notifications",
            ECCSAModuleFlag.Communication => "Communication Notifications",
            ECCSAModuleFlag.Investment => "Trade Promotion Notifications",
            ECCSAModuleFlag.Membership => "Membership Notifications",
            ECCSAModuleFlag.Reporting => "Reporting Notifications",
            ECCSAModuleFlag.Research => "Research & Advocacy Notifications",
            ECCSAModuleFlag.TradePromotion => "Trade Facilitation Notifications",
            ECCSAModuleFlag.Administration => "Administration",
            _ => string.Empty,
        };
        [HttpGet]
        public async Task<IActionResult> Index(ECCSAModuleFlag module, MessageStatus searchStatus)
        {
            var userName = TheUser.FullName;
            EmailMessageViewModel model;
            LogStart(_logger, controllerName, "Index");
            var roleLevel = await GetRoleLevel(module);
            if(roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Index");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            model = new EmailMessageViewModel
            {
                FullName = userName,
                CurrentPage = GetCurrentPage(module),
                CurrentOperation = "List",
                TheModel = new EmailMessage()
                {
                    Id = Guid.Empty,
                }
            };
            model.TheList = await _repository.EmailMessages(module, searchStatus);
            model.Templates = await _repository.EmailTemplates(module);
            model.CurrentModule = module;
            model.Status = searchStatus;
            model.ChangeActive(MenuItemEnum.EmailMessage);
            TempData["title"] = GetCurrentPage(module);
            model.RoleLevel = roleLevel;
            LogEnd(_logger, controllerName, "Index");
            return View(model);
        }
        public async Task<IActionResult> Delete(Guid id, ECCSAModuleFlag module)
        {

            EmailMessage request = new ()
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
                _logger.LogError("Delete Email", ex, request);
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Delete Email", ex, request);
                CommandStatus(CommandStatusEnum.UnkownError);
            }
            return RedirectToAction("Index", new { module });
        }
        [HttpPost]
        public async Task<IActionResult> Save(EmailMessageViewModel vmodel)
        {
            LogStart(_logger, controllerName, "Save");
            var roleLevel = await GetRoleLevel(vmodel.Owner);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "Save");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("Detail", new { id = vmodel.Id });
            }
            var request = await _repository.EmailMessageById(vmodel.Id) ??
             new EmailMessage()
             {
                 Id = vmodel.Id == Guid.Empty ? Guid.NewGuid() : vmodel.Id,
                 Owner = vmodel.Owner,
                 Subject = vmodel.Subject,
                 Body = vmodel.Body ?? "",
                 Recipients = vmodel.Recipients,
                 Status = vmodel.Status,
                 TemplateId = vmodel.TemplateId,
                 Attachment = ""
             };
            
            request.Subject = vmodel.Subject;
            try
            {
                if (vmodel.Attachment != null && vmodel.Attachment.Length > 0)
                {
                    await SaveDocument(request.Id, vmodel.Owner, vmodel.Attachment);
                }
                await _repository.Save(request, TheUser.Id);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("Save Email", ex, request);
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Save Email", ex, request);
                CommandStatus(CommandStatusEnum.UnkownError);
            }
            if (vmodel.Id == Guid.Empty)
                return RedirectToAction("Index", new { module = vmodel.Owner });
            else
                return RedirectToAction("Detail", new { id = vmodel.Id });

        }
        [HttpPost]
        public async Task<IActionResult> ChangeStatus(Guid id, MessageStatus status)
        {
            try
            {
                await _repository.ChangeStatus(id, status, TheUser.Id);
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
        [HttpPost]
        public async Task<IActionResult> CopyEmail(Guid id)
        {
            var redirectId = id;
            try
            {
                redirectId = await _repository.CopyEmail(id, TheUser.Id);
                CommandStatus("Successfully Copied");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("Copy email", ex, id);
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Copy email", ex, id);
                CommandStatus(CommandStatusEnum.UnkownError);
            }
            return RedirectToAction("Detail", new { id = redirectId });
        }
        [HttpPost]
        public async Task<IActionResult> AddRecipient(Guid messageId, MessageDelivery email)
        {

            _logger.LogInformation($"Add Recipient {email}");
            try
            {
                await _repository.AddRecipient(messageId, email.Address, email.Label, email.CCGroup, TheUser.Id);
                CommandStatus("Successfully Recipient Added");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("Save Email", ex, email);
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Save Recipient Detail");
            }
            return RedirectToAction("Detail", new { Id = messageId });
        }
        [HttpPost]
        public async Task<IActionResult> RemoveRecipient(Guid messageId, string email)
        {

            _logger.LogInformation($"Remove Recipient {email}");
            try
            {
                await _repository.RemoveRecipient(messageId, email, TheUser.Id);
                CommandStatus("Successfully Recipient Removed");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("Remove Email Recipient", ex, email);
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Remove Email Recipient");
            }
            return RedirectToAction("Detail", new { Id = messageId });
        }
        [HttpGet]
        public async Task<IActionResult> Detail(Guid id
            , Guid sectorId
            , StakeholderTypeEnum stakeholderType = StakeholderTypeEnum.Association
            )
        {
            var userName = TheUser.FullName;
            EmailMessageViewModel model;
            if (Guid.Empty == id)
            {
                return NotFound();
            }
            model = new EmailMessageViewModel
            {
                FullName = userName,
                CurrentOperation = "Detail",
                TheList = new List<EmailMessage>(),
                TheModel = await _repository.EmailMessageById(id),
                StakeholderType = stakeholderType,
                SectorId = sectorId
            };
            if (model.TheModel == null)
            {
                return RedirectToAction("Error", "Home", new { area = "" });
            }
            var lst = await _lookupRepository.LookupTypeById(Persistence.Repositories.Constants.SectorLookupTypeId);
            model.Sectors = lst.LookupValues.ToList();
            model.Sectors.Insert(0, new LookupValue
            {
                Id = Guid.Empty,
                Name = String.Empty,
                Value = Guid.Empty.ToString()
            });
            model.CurrentModule = model.TheModel.Owner;
            model.RecipientsSource = await _repository.Recipients(null,sectorId, stakeholderType);
            model.ChangeActive(MenuItemEnum.EmailMessage);
            model.CurrentPage = GetCurrentPage(model.CurrentModule);
            //try
            //{
            //    var request = new MailService.WebApi.Models.MailRequest
            //    {
            //        Attachments = new List<IFormFile>(),
            //        Body = model.TheModel.Body,
            //        Subject = model.TheModel.Subject,
            //        ToEmail = new List<string>()
            //    };
            //    //await _mailService.GetEmailAsync(request);
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError("GetEmailAsync", ex);
                
            //}
            TempData["title"] = model.CurrentPage;
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> GenerateBody(Guid id, IFormCollection formCollection)
        {
            var msg = await _repository.EmailMessageById(id);
            //
            string FilePath = Path.Combine(FileRoot, "EmailMessageTemplates", $"{msg.TemplateId}.html");
            StreamReader str = new (FilePath);
            string mailbody = str.ReadToEnd();
            str.Close();
            //
            foreach (var pl in msg.Template.PlaceHolders)
            {
                var value = pl.Name;
                if (!string.IsNullOrEmpty(formCollection[pl.Signature]))
                    value = formCollection[pl.Signature].ToString();
                mailbody = mailbody.Replace(pl.Signature, value);
            }
            msg.Body = mailbody;
            try
            {
                await _repository.Save(msg, TheUser.Id);
                CommandStatus("Successfully Body Generated");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("Body Generate", ex, msg);
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Body Generate", ex, msg);
                CommandStatus(CommandStatusEnum.UnkownError);
            }
            return RedirectToAction("Detail", new { id });
        }
        [HttpPost]
        public async Task<IActionResult> SetBody(Guid id, string body)
        {
            var msg = await _repository.EmailMessageById(id);
            //
            msg.Body = body;
            try
            {
                await _repository.Save(msg, TheUser.Id);
                CommandStatus("Successfully Body saved");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("Body save", ex, msg);
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Body save", ex, msg);
                CommandStatus(CommandStatusEnum.UnkownError);
            }
            return RedirectToAction("Detail", new { id });
        }
        [HttpPost]
        public async Task<IActionResult> AddRecipent(Guid id, string address, string label, StakeholderTypeEnum stakeholderType)
        {
            var msg = await _repository.EmailMessageById(id);
            //
            if (!msg.Recipients.Any(s => s.Address == address))
            {
                msg.Recipients.Add(new MessageDelivery
                {
                    Address = address,
                    Status = DeliveryStatus.NotSet,
                    Label = label
                });
                try
                {
                    await _repository.Save(msg, TheUser.Id);
                    CommandStatus("Successfully Recipient Added");
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
            }
            return RedirectToAction("Detail", new { id, stakeholderType });
        }
        [HttpPost]
        public async Task<IActionResult> AddRecipents(Guid id, string selectedList, StakeholderTypeEnum stakeholderType,Guid sectorId)
        {
            var msg = await _repository.EmailMessageById(id);
            var list = selectedList?.Split(';');
            var idList = ConvertToGuidList(list);
            var recipientsSources = await _repository.Recipients(null,Guid.Empty, stakeholderType);
            var selected = recipientsSources.Where(s => idList.Any(i => i == s.Id) && !msg.Recipients.Any(r => r.Address == s.Email)).ToList();
            if (!selected.Any())
            {
                CommandStatus("Nothing New Selected to add.");
                return RedirectToAction("Detail", new { id, stakeholderType });
            }
            //
            msg.Recipients.AddRange(selected.Select(s => new MessageDelivery
            {
                Address = s.Email,
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
            var msg = await _repository.EmailMessageById(id);
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
        public async Task<IActionResult> RemoveRecipent(Guid id, string address, string label, StakeholderTypeEnum stakeholderType)
        {
            var msg = await _repository.EmailMessageById(id);
            //
            var removed = msg.Recipients.FirstOrDefault(s => s.Address == address);
            if (removed != null)
            {
                msg.Recipients.Remove(removed);
                try
                {
                    await _repository.Save(msg, TheUser.Id);
                    CommandStatus("Successfully Recipient Removed");
                }
                catch (InvalidOperationException ex)
                {
                    _logger.LogError("Recipient Removed", ex, msg);
                    CommandStatus(ex.Message);
                }
                catch (Exception ex)
                {
                    _logger.LogError("Recipient Removed", ex, msg);
                    CommandStatus(CommandStatusEnum.UnkownError);
                }
            }
            return RedirectToAction("Detail", new { id, stakeholderType });
        }
        //[HttpPost]
        //public async Task<IActionResult> SendEmail(Guid id)
        //{
        //    var msg = await _repository.EmailMessageById(id);
        //    IFormFile file = null;
        //    if (!string.IsNullOrEmpty(msg.Attachment))
        //    {
        //        string path = Path.Combine(EmalAttachmentAbsolutePath, Guid2String(id).ToString() + msg.Attachment);
        //        string fileName = "Attachment" + msg.Attachment;
        //        var fullName = path;
        //        if (!System.IO.File.Exists(fullName))
        //        {
        //            if (fullName.EndsWith("\\"))
        //                fullName = string.Concat(path, fileName);
        //            else
        //                fullName = string.Concat(path, "\\", fileName);
        //        }
        //        MemoryStream memoryStream = new();
        //        try
        //        {
        //            using var stream = new FileStream(fullName, FileMode.Open, FileAccess.Read);
        //            await stream.CopyToAsync(memoryStream);
        //            file = new FormFile(memoryStream, 0, memoryStream.Length, fileName, fileName);
        //        }
        //        catch (Exception ex)
        //        {
        //            _logger.LogError($"SendEmail Attachment get file path {path} error msg {ex.Message}", ex);
        //            file = null;
        //        }
        //    }
        //    //
        //    try
        //    {
        //        var request = new MailService.WebApi.Models.MailRequest
        //        {
        //            Attachments = file != null ? new List<IFormFile> { file } : new List<IFormFile>(),
        //            Body = msg.Body,
        //            Subject = msg.Subject,
        //            ToEmail = new List<string>()
        //        };
        //        foreach (var address in msg.Recipients)
        //        {
        //            request.ToEmail.Add(address.Address);
        //            address.Status = DeliveryStatus.Delivered;
        //        }
        //        try
        //        {
        //            await _mailService.SendEmailAsync(request);
        //        }
        //        catch (Exception ex)
        //        {
        //            _logger.LogError("SendEmail", ex);
        //            msg.Recipients.ForEach(r => r.Status = DeliveryStatus.NotDelivered);
        //        }
        //        await _repository.Save(msg, TheUser.Id);
        //        CommandStatus("Successfully Email Sent");
        //    }
        //    catch (InvalidOperationException ex)
        //    {
        //        _logger.LogError("Send Email", ex, msg);
        //        CommandStatus(ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError("Send Email", ex, msg);
        //        CommandStatus(CommandStatusEnum.UnkownError);
        //    }
        //    return RedirectToAction("Detail", new { id });
        //}
        [HttpPost]
        public async Task<IActionResult> SaveAttachment(Guid id, IFormFile attachmentFile)
        {
            var msg = await _repository.EmailMessageById(id);
            //
            try
            {
                if (attachmentFile != null)
                {
                    string extension = Path.GetExtension(Path.GetFileName(attachmentFile.FileName));
                    await SaveFile(EmalAttachmentAbsolutePath, $"{Guid2String(id)}{extension}", attachmentFile);
                    CommandStatus(CommandStatusEnum.Saved);
                    if (attachmentFile != null)
                    {
                        msg.Attachment = Path.GetExtension(Path.GetFileName(attachmentFile.FileName));
                    }
                    else
                    {
                        msg.Attachment = "";
                    }
                    await _repository.Save(msg, TheUser.Id);
                    CommandStatus("Attachment Successfully Saved");
                }
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("Body save", ex, msg);
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Body save", ex, msg);
                CommandStatus(CommandStatusEnum.UnkownError);
            }
            return RedirectToAction("Detail", new { id });
        }
        [HttpGet]
        public async Task<FileResult> DownLoadFile(Guid id, string type, string fileName)
        {
            var filePath = EmalAttachmentAbsolutePath;
            string extension = Path.GetExtension(Path.GetFileName(fileName));
            return await DownLoadFile(Path.Combine(filePath, $"{Guid2String(id)}{extension}") , fileName);
        }
    }
}
