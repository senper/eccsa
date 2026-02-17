using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Persistence.DTO;
using ECCSA.BackOffice.Server.Persistence.Context;
using ECCSA.BackOffice.Server.Persistence.DTO.Common;
using Microsoft.EntityFrameworkCore;

namespace ECCSA.BackOffice.Server.Persistence.Repositories.Common
{
    public class SurveySettingRepository
    {
        private readonly EccsaContext _context;

        public SurveySettingRepository(EccsaContext context)
        {
            _context = context;
        }

        public IQueryable<SurveySetting> SurveySettings(Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return _context.SurveySettings.Include(x => x.Questions)
.Include(x => x.Surveys)
.Include(x => x.Choices)
;
        }

        public async Task<List<SurveySetting>> SettingList(ECCSAModuleFlag subSystem)
        {
            return await _context.SurveySettings.Include(x => x.Questions).Include(x => x.Surveys).Include(x => x.Choices).Where(s => s.ResponsibleUnit == subSystem).ToListAsync();
        }

        public async Task<SurveySetting> SettingById(Guid id)
        {
            var settig = await _context.SurveySettings.Include(x => x.Questions)
                                                .Include(x => x.Surveys)
                                                .Include(x => x.Choices)
                                                .FirstOrDefaultAsync(i => i.Id == id);

            foreach (var q in settig.Questions)
            {
                q.Choices = settig.Choices.Where(c => c.QuestionId == q.Id).ToList();


                q.SimpleChoices = q.Choices.Select(s => new SimpleChoice()
                {
                    Id = s.Id,
                    ChoiceText = s.ChoiceText,
                    OrderNo = s.OrderNo,
                    QuestionId = s.QuestionId,
                    QuestionText = s.QuestionText
                }).ToList();

                q.ParentText = "";
                if (q.ParentChoiceId.HasValue)
                {
                    var pchoice = settig.Choices.First(c => c.Id == q.ParentChoiceId.Value);
                    q.ParentText = $"{pchoice.Question.OrderNo} {pchoice.ChoiceText}";
                    q.ParentText = q.ParentText.Length > 0 ? $"[{q.ParentText}]" : "";
                }
            }
            foreach (var c in settig.Choices)
            {
                var qs = settig.Questions.FirstOrDefault(q => q.Id == c.QuestionId);
                c.QuestionText = $"{qs.OrderNo} {qs.Question}";
                c.Referenced = settig.Questions.Any(q => q.ParentChoiceId == c.Id);
            }
            return settig;
        }

        public async Task<List<SurveyQuestion>> Questions(Guid settingId)
        {
            return await _context.SurveyQuestions.Include(x => x.Choices).Where(q => q.SurveySettingId == settingId).ToListAsync();
        }
        public async Task<SurveySettingPayload> Create(GenericInput<CreateSurveySettingInput> input)
        {
            SurveySetting data = new()
            {
                Id = Guid.NewGuid()
            ,
                Title = input.PayLoad.Title
            ,
                Description = input.PayLoad.Description
            ,
                ResponsibleUnit = input.PayLoad.ResponsibleUnit
            ,
                Recurrence = input.PayLoad.Recurrence
            ,
                OpenDays = input.PayLoad.OpenDays
            ,
                PublishThrough = input.PayLoad.PublishThrough
            ,
                CreatedBy = input.UserId
            ,
                CreateStamp = DateTime.UtcNow
            ,
                UpdatedBy = input.UserId
            ,
                UpdateStamp = DateTime.UtcNow

            };
            await _context.SurveySettings.AddAsync(data);
            _ = await _context.SaveChangesAsync();
            return new SurveySettingPayload(data);
        }
        public async Task<SurveySetting> Save(SurveySetting data, Guid UserId)
        {
            if (data.Id == Guid.Empty)
            {
                data.Id = Guid.NewGuid();
                data.CreatedBy = UserId;
                data.CreateStamp = DateTime.UtcNow;
                data.UpdatedBy = UserId;
                data.UpdateStamp = DateTime.UtcNow;
                await _context.SurveySettings.AddAsync(data);
            }
            else
            {
                var tmp = await _context.SurveySettings.FirstOrDefaultAsync(r => r.Id == data.Id);
                if (tmp == null) throw new KeyNotFoundException("Survey setting not found");
                tmp.Title = data.Title;
                tmp.Description = data.Description;
                tmp.Recurrence = data.Recurrence;
                tmp.OpenDays = data.OpenDays;
                tmp.PublishThrough = data.PublishThrough;
                tmp.UpdatedBy = UserId;
                tmp.UpdateStamp = DateTime.UtcNow;
                _context.SurveySettings.Update(tmp);
            }
            await _context.SaveChangesAsync();
            return data;
        }

        public async Task<SurveyQuestion> Save(SurveyQuestion data)
        {
            if (data.Id == Guid.Empty)
            {
                data.Id = Guid.NewGuid();
                data.Cascaded = data.ParentChoiceId.HasValue;
                await _context.SurveyQuestions.AddAsync(data);
            }
            else
            {
                var tmp = await _context.SurveyQuestions.FirstOrDefaultAsync(r => r.Id == data.Id);
                if (tmp == null) throw new KeyNotFoundException("Question not found");
                tmp.Question = data.Question;
                tmp.OrderNo = data.OrderNo;
                tmp.QuestionType = data.QuestionType;
                tmp.Cascaded = data.ParentChoiceId.HasValue;
                tmp.ParentChoiceId = data.ParentChoiceId;
                tmp.MinRange = data.MinRange;
                tmp.MaxRange = data.MaxRange;
                _context.SurveyQuestions.Update(tmp);
            }
            await _context.SaveChangesAsync();
            return data;
        }

        public async Task<SurveyChoice> Save(SurveyChoice data)
        {
            if (data.Id == Guid.Empty)
            {
                data.Id = Guid.NewGuid();
                await _context.SurveyChoices.AddAsync(data);
            }
            else
            {
                var tmp = await _context.SurveyChoices.FirstOrDefaultAsync(r => r.Id == data.Id);
                if (tmp == null) throw new KeyNotFoundException("Question not found");
                tmp.ChoiceText = data.ChoiceText;
                tmp.OrderNo = data.OrderNo;
                _context.SurveyChoices.Update(tmp);
            }
            await _context.SaveChangesAsync();
            return data;
        }

        public async Task Save(Survey model, Guid userId)
        {
            var tmp = await _context.Surveys.FirstOrDefaultAsync(s => s.Id == model.Id && s.SettingId == model.SettingId);
            model.UpdatedBy = userId;
            model.UpdateStamp = DateTime.UtcNow;
            if (tmp == null)
            {
                model.Id = Guid.NewGuid();
                model.AnalysisDescription = String.Empty;
                model.Attachment = String.Empty;

                await _context.Surveys.AddAsync(model);
            }
            else
            {
                model.AnalysisDescription = String.IsNullOrEmpty(model.AnalysisDescription) ? String.Empty : model.AnalysisDescription;
                model.Attachment = String.IsNullOrEmpty(model.Attachment) ? String.Empty : model.Attachment;
                _context.Entry(tmp).CurrentValues.SetValues(model);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<Survey> SaveTelegramPublishDate(Survey data, Guid UserId)
        {
            var tmp = await _context.Surveys.FirstOrDefaultAsync(r => r.Id == data.Id);
            if (tmp == null) throw new KeyNotFoundException("Survey not found");
            tmp.UpdatedBy = UserId;
            tmp.TelegramPublishDate = data.TelegramPublishDate;
            tmp.PollIds = data.PollIds;
            _context.Surveys.Update(tmp);
            await _context.SaveChangesAsync();
            return data;
        }
        public async Task<SurveySettingPayload> Update(GenericInput<UpdateSurveySettingInput> data)
        {
            var tmp = await _context.SurveySettings.FirstOrDefaultAsync(r => r.Id == data.PayLoad.Id);
            if (tmp == null) throw new KeyNotFoundException("Survey Setting not found");
            tmp.Title = data.PayLoad.Title;
            tmp.Description = data.PayLoad.Description;
            tmp.ResponsibleUnit = data.PayLoad.ResponsibleUnit;
            tmp.Recurrence = data.PayLoad.Recurrence;
            tmp.OpenDays = data.PayLoad.OpenDays;
            tmp.PublishThrough = data.PayLoad.PublishThrough;
            tmp.UpdatedBy = data.UserId;
            tmp.UpdateStamp = DateTime.UtcNow;

            _context.SurveySettings.Update(tmp);
            await _context.SaveChangesAsync();
            return new SurveySettingPayload(tmp);

        }

        public async Task DeleteChoice(Guid id)
        {
            var tmp = await _context.SurveyChoices.Include(i => i.Responses).FirstOrDefaultAsync(r => r.Id == id);
            if (tmp == null) throw new KeyNotFoundException("Choice not found");
            if (tmp.Responses.Count > 0)
                return;
            _context.SurveyChoices.Remove(tmp);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteQuestion(Guid id)
        {
            var tmp = await _context.SurveyQuestions.Include(q => q.Choices).FirstOrDefaultAsync(r => r.Id == id);
            if (tmp == null) throw new KeyNotFoundException("Question not found");
            _context.SurveyQuestions.Remove(tmp);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteSetting(Guid id)
        {
            var tmp = await _context.SurveySettings.Include(q => q.Questions).Include(q => q.Surveys).FirstOrDefaultAsync(r => r.Id == id);
            if (tmp == null) throw new KeyNotFoundException("Setting not found");
            if (tmp.Questions.Count > 0 || tmp.Surveys.Count > 0)
            {
                throw new InvalidOperationException("Cannot delete survey setting that has questions and/or surveys");
            }
            _context.SurveySettings.Remove(tmp);
            await _context.SaveChangesAsync();
        }
        public async Task Delete(SurveySetting data)
        {
            var tmp = await _context.SurveySettings.FirstOrDefaultAsync(r => r.Id == data.Id);
            if (tmp == null) throw new KeyNotFoundException("Survey Setting not found");
            _context.SurveySettings.Remove(tmp);
            await _context.SaveChangesAsync();
        }


    }
}