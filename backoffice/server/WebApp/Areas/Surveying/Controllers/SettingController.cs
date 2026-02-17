using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common;
using ECCSA.BackOffice.Server.Persistence.Repositories.Security;
using ECCSA.BackOffice.Server.WebApp.Areas.Surveying.Model;
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
using telegramtest;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Surveying.Controllers
{
    [Area("Surveying")]
    public class SettingController : BaseController
    {
        private readonly ILogger<SettingController> _logger;
        private readonly SurveySettingRepository _settingRepository;
        private readonly ITelegramService _telegramService;
        public SettingController(ILogger<SettingController> logger,
            UserManager<User> userManager,
            SurveySettingRepository settingRepository,
            PublicUserRepository userRepository,
            ITelegramService telegramService
            , NotificationRepository notificationRepository) : base(userManager, userRepository, notificationRepository)
        {

            _logger = logger;
            _settingRepository = settingRepository;
            _telegramService = telegramService;
        }


        public async Task<IActionResult> Index(ECCSAModuleFlag module)
        { var userName = TheUser.FullName;
            SurveySettingViewModel model;
            if (!TheUser.AssignedModules.HasFlag(module))
            {
                model = new SurveySettingViewModel
                {
                    FullName = userName,
                    CurrentPage = "Survey Setting",
                    CurrentOperation = "List",
                    CurrentModule = module,
                    TheList = new List<SurveySetting>(),
                    TheModel = new SurveySetting()
                    {
                        ResponsibleUnit = module,
                        PublishThrough = MediaOutletFlag.NotSet,
                        Id = Guid.Empty,
                        Status = StatusEnum.Active,
                        Recurrence = FrequencyEnum.None,
                        Title = string.Empty,
                        Description = string.Empty
                    }

                };
                return View(model);
            }
            model = new SurveySettingViewModel
            {
                FullName = userName,
                CurrentPage = "Survey Setting",
                CurrentOperation = "List",
                CurrentModule = module,
                TheList = await _settingRepository.SettingList(module),
                TheModel = new SurveySetting()
                {
                    ResponsibleUnit = module,
                    PublishThrough = MediaOutletFlag.NotSet,
                    Id = Guid.Empty,
                    Status = StatusEnum.Active,
                    Recurrence = FrequencyEnum.None,
                    Title = string.Empty,
                    Description = string.Empty
                }

            };

            model.ChangeActive(MenuItemEnum.SurveySetting);
            TempData["title"] = "Survey Setting";
            return View(model);
        }


        public async Task<IActionResult> Detail(Guid id, int activeTabIndex = 0)
        {
            var userName = TheUser.FullName;
            SurveySettingViewModel model;
            if (Guid.Empty == id)
            {
                return NotFound();
            }
            model = new SurveySettingViewModel
            {
                FullName = userName,
                CurrentPage = "Survey Setting",
                CurrentOperation = "List",
                TheList = new List<SurveySetting>(),
                TheModel = await _settingRepository.SettingById(id)

            };
            if (model.TheModel == null)
            {
                return RedirectToAction("Error", "Home", new { area = "" });
            }
            model.CurrentModule = model.TheModel.ResponsibleUnit;
            model.ChoiceJson = System.Text.Json.JsonSerializer.Serialize(model.TheModel.Questions
                .Where(q => q.QuestionType == QuestionType.SingleChoice ||
                            q.QuestionType == QuestionType.MultipleChoice).OrderBy(c => c.OrderNo));
            model.ChangeActive(MenuItemEnum.SurveySetting);
            model.ActiveTabIndex = activeTabIndex;
            TempData["title"] = "Survey Setting";
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveSetting(SurveySetting model)
        {
            try
            {
                await _settingRepository.Save(model, TheUser.Id);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (Exception ex)
            {
                _logger.LogError("SaveSetting", ex, model);
                CommandStatus(CommandStatusEnum.UnkownError);
            }
            return RedirectToAction("Index", new { module = model.ResponsibleUnit });
        }

        [HttpPost]
        public async Task<IActionResult> SaveSurvey(Survey model)
        {
            try
            {
                await _settingRepository.Save(model, TheUser.Id);
                var setting = await _settingRepository.SettingById(model.SettingId);
                    var msg = "Data Saved Successfully !!";
                if (setting.PublishThrough == MediaOutletFlag.Telegram)
                {
                    var polls = await _telegramService.SendSurveyPolls(setting, model.GroupId);
                    if (polls?.Count > 0)
                    {
                        model.TelegramPublishDate = DateTime.Now;
                        model.PollIds = polls;
                        await _settingRepository.SaveTelegramPublishDate(model, TheUser.Id);
                    }
                    else
                    {
                        msg += " BUT:: Not able to publish!";
                    }
                }
                CommandStatus(msg);
            }
            catch (Exception ex)
            {
                _logger.LogError("SaveSurvey", ex, model);
                CommandStatus(CommandStatusEnum.UnkownError);
            }
            return RedirectToAction("Detail", new { id = model.SettingId, activeTabIndex = 1 });
        }

        [HttpPost]
        public async Task<IActionResult> SaveQuestion(SurveyQuestion model)
        {
            try
            {
                if (!model.Cascaded)
                    model.ParentChoiceId = null;
                await _settingRepository.Save(model);
                CommandStatus(CommandStatusEnum.Saved);

            }
            catch (Exception ex)
            {
                _logger.LogError("SaveQuestion", ex, model);
                CommandStatus(CommandStatusEnum.UnkownError);
            }
            return RedirectToAction("Detail", new { id = model.SurveySettingId });
        }

        [HttpPost]
        public async Task<IActionResult> SaveChoice(SurveyChoice model)
        {
            try
            {
                await _settingRepository.Save(model);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (Exception ex)
            {
                _logger.LogError("SaveQuestion", ex, model);
                CommandStatus(CommandStatusEnum.UnkownError);
            }
            return RedirectToAction("Detail", new { id = model.SurveySettingId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteChoice(Guid id, Guid settingId)
        {
            try
            {
                await _settingRepository.DeleteChoice(id);
                CommandStatus(CommandStatusEnum.Deleted);
            }
            catch (Exception ex)
            {
                _logger.LogError("DeleteChoice", ex, id);
                CommandStatus(CommandStatusEnum.CannotDelete);
            }
            return RedirectToAction("Detail", new { id = settingId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteQuestion(Guid id, Guid settingId)
        {
            try
            {
                await _settingRepository.DeleteQuestion(id);
                CommandStatus(CommandStatusEnum.Deleted);
            }
            catch (Exception ex)
            {
                _logger.LogError("DeleteQuestion", ex, id);
                CommandStatus(CommandStatusEnum.CannotDelete);
            }
            return RedirectToAction("Detail", new { id = settingId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSetting(Guid id,
        ECCSAModuleFlag module)
        {
            try
            {
                await _settingRepository.DeleteSetting(id);
                CommandStatus(CommandStatusEnum.Deleted);
            }
            catch (Exception ex)
            {
                _logger.LogError("DeleteSetting", ex, id);
                CommandStatus(CommandStatusEnum.CannotDelete);
            }
            return RedirectToAction("Index", new { module });
        }

    }
}
