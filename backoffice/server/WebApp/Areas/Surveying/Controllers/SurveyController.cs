using ClosedXML.Excel;
using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common;
using ECCSA.BackOffice.Server.Persistence.Repositories.Security;
using ECCSA.BackOffice.Server.WebApp.Areas.Surveying.Model;
using ECCSA.BackOffice.Server.WebApp.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Notification;
using telegramtest;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Surveying.Controllers
{
    [Area("Surveying")]
    public class SurveyController : BaseController
    {
        private readonly ILogger<SurveyController> _logger;
        private readonly SurveyRepository _repository;
        private readonly ITelegramService _telegramService;

        public SurveyController(ILogger<SurveyController> logger, UserManager<User> userManager, SurveyRepository repository, PublicUserRepository userRepository, ITelegramService telegramService
            , NotificationRepository notificationRepository) : base(userManager, userRepository, notificationRepository)
        {
            _logger = logger;
            _repository = repository;
            controllerName = "Survey";
            _telegramService = telegramService;
        }

        public async Task<IActionResult> Index(Guid id)
        {
            var userName = TheUser.FullName;
            SurveyViewModel model;
            if (Guid.Empty == id)
            {
                return NotFound();
            }
            model = new SurveyViewModel
            {
                FullName = userName,
                CurrentPage = "Survey",
                CurrentOperation = "Detail",
                TheList = new List<Survey>(),
                TheModel = await _repository.SurveyById(id)
            };
            if (model.TheModel == null)
            {
                LogSecurityError(_logger, controllerName, "Index");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            var roleLevel = await GetRoleLevel(model.TheModel.Setting.ResponsibleUnit);
            model.CurrentModule = model.TheModel.Setting.ResponsibleUnit;
            model.RoleLevel = roleLevel;
            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Index");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            TempData["title"] = "Survey";
            return View("Detail", model);
        }

        public async Task<IActionResult> Detail(Guid id)
        {
            var userName = TheUser.FullName;
            SurveyViewModel model;
            
            model = new SurveyViewModel
            {
                FullName = userName,
                CurrentPage = "Survey",
                CurrentOperation = "Detail",
                TheList = new List<Survey>(),
                TheModel = await _repository.SurveyById(id)

            };
            if (model.TheModel == null)
            {
                return RedirectToAction("Error", "Home", new { area = "" });
            }
            var roleLevel = await GetRoleLevel(model.TheModel.Setting.ResponsibleUnit);
            model.CurrentModule = model.TheModel.Setting.ResponsibleUnit;
            model.RoleLevel = roleLevel;
            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Index");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            TempData["title"] = "Survey";

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Save(Survey model)
        {
            try
            {
                await _repository.Save(model, TheUser.Id);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (Exception ex)
            {
                LogError(_logger, ex, controllerName, "Save", model);
                CommandStatus(CommandStatusEnum.UnkownError);
            }
            return RedirectToAction("Detail", new { id = model.Id });
        }

        [HttpPost]
        public async Task<IActionResult> SaveAttachment(AttachmentViewModel model)
        {
            try
            {
                var md = new Attachment()
                {
                    Id = model.Id,
                    SurveyId = model.SurveyId,
                    Title = model.Title,
                    Description = model.Description
                };
                if (md.Id == Guid.Empty)
                    md.Id = Guid.NewGuid();
                if (model.File == null)
                {
                    if (model.Id == Guid.Empty)
                    {
                        CommandStatus("No file to attach");
                        return RedirectToAction("Detail", new { id = model.SurveyId });
                    }
                }
                else
                {
                    var survey = await _repository.SurveyById(model.SurveyId);
                    md.Extension = Path.GetExtension(model.File.FileName);
                    await SaveDocument(md.Id, survey.Setting.ResponsibleUnit, model.File);
                }
                await _repository.Save(md, TheUser.Id);
                CommandStatus(CommandStatusEnum.Saved);

            }
            catch (Exception ex)
            {
                LogError(_logger, ex, controllerName, "SaveAttachment", model);
                CommandStatus(CommandStatusEnum.UnkownError);
            }
            return RedirectToAction("Detail", new { id = model.SurveyId });
        }

        [HttpGet]
        public async Task<FileResult> ExportSurvey(Guid id)
        {
            var survey = await _repository.SurveyById(id);

            var workbook = new XLWorkbook();
            IXLWorksheet sheet = workbook.Worksheets.Add("Sheet 1");
            sheet.Cell(1, 1).Value = "QuestionNo";
            sheet.Cell(1, 2).Value = "Question";
            sheet.Cell(1, 3).Value = "RespondentCode";
            sheet.Cell(1, 4).Value = "Response";
            var i = 2;
            var quesions = survey.Setting.Questions.ToDictionary(x => x.Id);
            foreach (var r in survey.Responses)
            {
                var q = quesions[r.QuestionId];

                var userCode = r.Respondent.UserCode;

                sheet.Cell(i, 1).Value = q.OrderNo;
                sheet.Cell(i, 2).Value = q.Question;
                sheet.Cell(i, 3).Value = r.Respondent.UserCode;
                sheet.Cell(i, 4).Value = r.Response();
                i++;
            }

            using MemoryStream stream = new();
            workbook.SaveAs(stream);
            var content = stream.ToArray();
            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{survey.Title}.xlsx");
        }


        [HttpGet]
        public async Task<FileResult> ExportSurveyVotes(Guid id)
        {
            var survey = await _repository.SurveyById(id);

            var workbook = new XLWorkbook();
            IXLWorksheet sheet = workbook.Worksheets.Add("Sheet 1");
            sheet.Cell(1, 1).Value = "QuestionNo";
            sheet.Cell(1, 2).Value = "Question";
            sheet.Cell(1, 3).Value = "Choice";
            sheet.Cell(1, 4).Value = "Vote";
            var i = 2;
            var quesions = survey.Setting.Questions.ToDictionary(x => x.Id);
            foreach (var r in survey.Votes)
            {
                var q = quesions[r.QuestionId];
                var choice = q.Choices.FirstOrDefault(c => c.Id == r.ChoiceId);
                sheet.Cell(i, 1).Value = q.OrderNo;
                sheet.Cell(i, 2).Value = q.Question;
                sheet.Cell(i, 3).Value = $"{choice?.OrderNo}. {choice?.ChoiceText}";
                sheet.Cell(i, 4).Value = r.Votes;
                i++;
            }

            using MemoryStream stream = new();
            workbook.SaveAs(stream);
            var content = stream.ToArray();
            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{survey.Title}.xlsx");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAttachment(Guid id, Guid surveyId, ECCSAModuleFlag module)
        {
            try
            {
                var att = await _repository.GetAttachment(id);
                await _repository.DeleteAttachment(id);
                CommandStatus(CommandStatusEnum.Deleted);
                var absolutePath = AbsolutePath(module);
                var shortName = Guid2String(id);
                if (att.Extension?.Trim() != string.Empty)
                {
                    System.IO.File.Delete(Path.Combine(absolutePath, shortName + att.Extension));
                }
            }
            catch (Exception ex)
            {
                LogError(_logger, ex, controllerName, "DeleteAttachment", new { id, surveyId });
                CommandStatus(CommandStatusEnum.CannotDelete);
            }
            return RedirectToAction("Detail", new { id = surveyId });
        }
        [HttpPost]
        public async Task<IActionResult> DeleteSurvey(Guid id,
        ECCSAModuleFlag module)
        {
            var survey = await _repository.SurveyById(id);
            try
            {
                await _repository.DeleteSurvey(id);
                CommandStatus(CommandStatusEnum.Deleted);
            }
            catch (Exception ex)
            {
                _logger.LogError("DeleteSurvey", ex, id);
                CommandStatus(CommandStatusEnum.CannotDelete);
            }
            if (survey != null)
                return RedirectToAction("Detail", "Setting", new { id = survey?.SettingId });
            else
                return RedirectToAction("Index", "Setting", new { module });
        }
        [HttpGet]
        public async Task<FileResult> DownLoadFile(Guid id, string type, string fileName, ECCSAModuleFlag module)
        {
            var absolutePath = AbsolutePath(module);
            string extension = Path.GetExtension(Path.GetFileName(fileName));
            return await DownLoadFile(Path.Combine(absolutePath, $"{Guid2String(id)}{extension}"), fileName);
        }

        [HttpPost]
        public async Task<IActionResult> TelegramPublish(Guid id)
        {
            try
            {
                var survey = await _repository.SurveyById(id);
                var polls = await _telegramService.SendSurveyPolls(survey.Setting, survey.GroupId);
                if (polls?.Count > 0)
                {
                    survey.TelegramPublishDate = DateTime.Now;
                    survey.PollIds = polls;
                    await _repository.SaveTelegramPublishDate(survey, TheUser.Id);
                    CommandStatus(CommandStatusEnum.Updated);
                }
                else
                {
                    TempData["CommandStatus"] = "Not able to publish!";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("TelegramPublish", ex, id);
                TempData["CommandStatus"] = ex.Message;
            }
            return RedirectToAction("Detail", new { id });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTelegramResponses(Guid id)
        {
            try
            {
                var polls = await _repository.GetSurveyPolls(id);

                var setting = await _repository.GetSurveySettings(id, TheUser.Id);
                var result = await _telegramService.GetPollResults(setting, id, polls);
                var survey = result.Survey;
                if (survey != null)
                {
                    foreach (var r in survey.Responses)
                    {
                        await _repository.SaveTelegramResponses(r);
                    }

                    await _repository.SaveVotes(id, survey.Votes.ToList(), TheUser.Id);
                    await _telegramService.ConfirmPollUpdates(result.LastPollId);
                }
                TempData["CommandStatus"] = "Responses List updated successfully!";
            }
            catch (Exception ex)
            {
                _logger.LogError("TelegramPublish", ex, id);
                TempData["CommandStatus"] = ex.Message;
            }
            return RedirectToAction("Detail", new { id });
        }
    }
}
