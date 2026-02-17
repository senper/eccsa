using DocumentFormat.OpenXml.Office2010.Excel;
using ECCSA.BackOffice.Server.Model.Academy;
using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Communication;
using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Persistence.DTO;
using ECCSA.BackOffice.Server.Persistence.DTO.Academy;
using ECCSA.BackOffice.Server.Persistence.Repositories.Academy;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Notification;
using ECCSA.BackOffice.Server.Persistence.Repositories.Security;
using ECCSA.BackOffice.Server.WebApp.Areas.Academy.Model;
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
using ResourceType = ECCSA.BackOffice.Server.Model.Academy.ResourceType;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Academy.Controllers
{
    [Area("Academy")]
    public class TopicController : BaseController
    {
        private readonly ILogger<TopicController> _logger;
        private readonly string ResouceAttachmentsFolder = Path.Combine(
                                        Directory.GetCurrentDirectory(),
                                        "wwwroot", "Academy",
                                        "ResouceAttachments");

        private readonly TopicRepository _topicRepository;
        private readonly CategoryRepository _categoryRepository;
        private readonly LearningResourceRepository _resourceRepository;
        private readonly TopicSettingRepository _topicSettingRepository;
        private readonly SettingVariableRepository _settingRepository;
        public TopicController(ILogger<TopicController> logger,
            UserManager<User> userManager,
            TopicRepository topicRepository,
            LearningResourceRepository resourceRepository,
            TopicSettingRepository topicSettingRepository,
        SettingVariableRepository settingRepository,
            CategoryRepository categoryRepository,
            PublicUserRepository userRepository
            , NotificationRepository notificationRepository) : base(userManager, userRepository, notificationRepository)
        {
            _logger = logger;
            _topicRepository = topicRepository;
            _categoryRepository = categoryRepository;
            _topicSettingRepository = topicSettingRepository;
            _resourceRepository = resourceRepository;
            _settingRepository = settingRepository;
            controllerName = "Topic";
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userName = TheUser.FullName;
            LogStart(_logger, controllerName, "Index");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Academy);
            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Index");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            var model = new TopicViewModel
            {
                FullName = userName,
                CurrentPage = "Topic",
                CurrentOperation = "List",
                TheList = _topicRepository.Topics(TheUser.Id).ToList(),

            };
            model.ChangeActive(MenuItemEnum.AcademyTopic);
            model.RoleLevel = roleLevel;
            LogEnd(_logger, controllerName, "Index");
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Detail(Guid id, int activeTabIndex = 0)
        {
            var userName = TheUser.FullName;
            LogStart(_logger, controllerName, "Detail");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Academy);
            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Detail");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            var model = new TopicViewModel
            {
                FullName = userName,
                CurrentPage = "Topic",
                CurrentOperation = "Detail",
                TheList = new List<Topic>(),
                TheModel = await _topicRepository.TopicById(id, TheUser.Id),

            };
            if (model.TheModel == null)
            {
                return RedirectToAction("Error", "Home", new { area = "" });
            }
            model.ChangeActive(MenuItemEnum.AcademyTopic);
            model.RoleLevel = roleLevel;
            model.ActiveTabIndex = activeTabIndex;
            LogEnd(_logger, controllerName, "Index");
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var userName = TheUser.FullName;

            var model = new TopicViewModel
            {
                FullName = userName,
                CurrentPage = "Topics",
                TheList = new List<Topic>(),
                CategoryLookup = _categoryRepository.Categories(TheUser.Id).Where(c =>
                c.ChildrenCategories != null && c.ChildrenCategories.Count <= 0).Select(i => new CategoryLookup
                {
                    Id = i.Id,
                    Name = $"{i.Code}-{i.NameEng}",
                    Parent = i.ParentCategory != null ?
                    $"{i.ParentCategory.Code}-{i.ParentCategory.NameEng} ( {i.ParentCategory.NameLocal} )" : "",
                }).ToList(),
                TheModel = (await _topicRepository.TopicById(id, TheUser.Id)) ?? new Topic(),
            };
            model.ChangeActive(MenuItemEnum.AcademyTopic);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CreateTopic(Topic theModel)
        {
            LogStart(_logger, controllerName, "CreateTopic");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Academy);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "CreateTopic");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("Index");
            }
            try
            {

                await _topicRepository.Save(theModel, TheUser.Id);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unknown Error");
                TempData["CommandStatus"] = ex.Message;
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> UpdateTopic(Topic theModel)
        {
            LogStart(_logger, controllerName, "UpdateTopic");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Academy);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "UpdateTopic");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("Detail", new { id = theModel.Id });
            }
            try
            {
                await _topicRepository.Save(theModel, TheUser.Id);
                CommandStatus(CommandStatusEnum.Updated);
            }
            catch (Exception ex)
            {
                TempData["CommandStatus"] = ex.Message;
            }
            return RedirectToAction("Detail", new { id = theModel.Id });
        }
        [HttpPost]
        public async Task<IActionResult> DeleteTopic(Guid id)
        {
            LogStart(_logger, controllerName, "DeleteTopic");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Academy);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "UpdateTopic");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("Index");
            }
            var topic = (await _topicRepository.TopicById(id, TheUser.Id));
            if (topic != null)
            {
                await _topicRepository.Delete(topic);
                CommandStatus(CommandStatusEnum.Deleted);
            }
            else
            {
                TempData["CommandStatus"] = "Invalid Topic";
            }
            return RedirectToAction("Index");
        }

        ///
        public async Task<IActionResult> EditResource(Guid id, Guid topicId)
        {
            var userName = TheUser.FullName;

            var model = new ResourceViewModel
            {
                FullName = userName,
                CurrentPage = "Topics",
                TheList = new List<LearningResource>(),
                TheModel = (await _resourceRepository.LearningResourceById(id, TheUser.Id)) ?? new LearningResource { TopicId = topicId },
            };
            model.ChangeActive(MenuItemEnum.AcademyTopic);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CreateResource(CreateLearningResourceInput theModel, IFormFile attachment)
        {
            LogStart(_logger, controllerName, "CreateResource");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Academy);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "CreateResource");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("Detail", new { id = theModel.TopicId });
            }
            try
            {
                if (theModel.ResourceType == ResourceType.Attachment && attachment != null)
                {
                    CheckFileExtension(attachment.FileName ?? "");
                }
                var payload = await _resourceRepository.Create(new GenericInput<CreateLearningResourceInput>(theModel, TheUser.Id));
                if (theModel.ResourceType == ResourceType.Attachment && attachment != null)
                {
                    await SaveFile(ResouceAttachmentsFolder, payload.PayLoad.Id.ToString(), attachment);
                }
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (Exception ex)
            {
                TempData["CommandStatus"] = ex.Message;
            }
            return RedirectToAction("Detail", new { id = theModel.TopicId });
        }
        [HttpPost]
        public async Task<IActionResult> UpdateResource(UpdateLearningResourceInput theModel, IFormFile attachment)
        {
            LogStart(_logger, controllerName, "UpdateResource");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Academy);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "UpdateResource");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("Detail", new { id = theModel.TopicId });
            }
            try
            {
                if (theModel.ResourceType == ResourceType.Attachment && attachment != null)
                {
                    await SaveFile(ResouceAttachmentsFolder, theModel.Id.ToString(), attachment);
                }
                else if (theModel.ResourceType != ResourceType.Attachment)
                {
                    System.IO.File.Delete(ResouceAttachmentsFolder + "\\" + theModel.Id.ToString());
                }
                var payload = await _resourceRepository.Update(new GenericInput<UpdateLearningResourceInput>(theModel, TheUser.Id));
                CommandStatus(CommandStatusEnum.Updated);
            }
            catch (Exception ex)
            {
                TempData["CommandStatus"] = ex.Message;
            }
            return RedirectToAction("Detail", new { id = theModel.TopicId });
        }
        [HttpGet]
        public async Task<FileResult> DownLoadFile(Guid id, /*string type,*/ string fileName)
        {
            return await DownLoadFile(ResouceAttachmentsFolder + "\\" + id.ToString(), fileName);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteResource(Guid id)
        {
            LogStart(_logger, controllerName, "DeleteResource");
            var resource = (await _resourceRepository.LearningResourceById(id, TheUser.Id));

            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Academy);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "DeleteResource");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("Detail", new { id = resource.TopicId });
            }
            if (resource != null)
            {
                await _resourceRepository.Delete(resource);
                if (resource.ResourceType == ResourceType.Attachment)
                {
                    System.IO.File.Delete(ResouceAttachmentsFolder + "\\" + id.ToString());
                }
                CommandStatus(CommandStatusEnum.Deleted);
            }
            else
            {
                TempData["CommandStatus"] = "Invalid Resource";
            }
            return RedirectToAction("Detail", new { id = resource.TopicId });
        }
        ///
        ///
        public async Task<IActionResult> EditSetting(Guid id, Guid topicId)
        {
            var userName = TheUser.FullName;

            var model = new SettingViewModel
            {
                FullName = userName,
                CurrentPage = "Topics",
                TheList = new List<TopicSetting>(),
                SettingsLookup = _settingRepository.SettingVariables(TheUser.Id).ToList(),
                TheModel = (await _topicSettingRepository.TopicSettingById(id, TheUser.Id)) ?? new TopicSetting { TopicId = topicId },
            };
            model.ChangeActive(MenuItemEnum.AcademyTopic);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CreateSetting(CreateTopicSettingInput theModel)
        {
            try
            {
                var payload = await _topicSettingRepository.Create(new GenericInput<CreateTopicSettingInput>(theModel, TheUser.Id));
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (Exception ex)
            {
                TempData["CommandStatus"] = ex.Message;
            }
            return RedirectToAction("Detail", new { id = theModel.TopicId, activeTabIndex = 1 });
        }
        [HttpPost]
        public async Task<IActionResult> UpdateSetting(UpdateTopicSettingInput theModel)
        {
            try
            {
                var payload = await _topicSettingRepository.Update(new GenericInput<UpdateTopicSettingInput>(theModel, TheUser.Id));
                CommandStatus(CommandStatusEnum.Updated);
            }
            catch (Exception ex)
            {
                TempData["CommandStatus"] = ex.Message;
            }
            return RedirectToAction("Detail", new { id = theModel.TopicId, activeTabIndex = 1 });
        }
        [HttpPost]
        public async Task<IActionResult> DeleteSetting(Guid id)
        {
            var resource = (await _topicSettingRepository.TopicSettingById(id, TheUser.Id));
            if (resource != null)
            {
                await _topicSettingRepository.Delete(resource);
                CommandStatus(CommandStatusEnum.Deleted);
            }
            else
            {
                TempData["CommandStatus"] = "Invalid Setting";
            }
            return RedirectToAction("Detail", new { id = resource.TopicId, activeTabIndex = 1 });
        }
        ///
    }
}
