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
    public class SurveyRepository
    {
        private readonly EccsaContext _context;
        public SurveyRepository(EccsaContext context)
        {
            _context = context;
        }

        public Task<List<Survey>> Surveys(Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return _context.Surveys.Include(x => x.Setting)
.Include(x => x.Audiences)
.Include(x => x.Respondents).ToListAsync();
;
        }

        public async Task<List<Survey>> SurveyList(Guid settingId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return await _context.Surveys.Include(x => x.Setting)
                                .Include(x => x.Audiences)
                                .Include(x => x.Respondents)
                                .Include(x => x.Attachments)
                                .Where(s => s.SettingId == settingId).ToListAsync();
            ;
        }

        public async Task<List<Survey>> PublicSurveys(Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            var listQ = _context.Surveys.Include(s => s.Setting).Where(s => 
            s.Setting.Status == StatusEnum.Active && 
            s.Setting.Questions.Count > 0 &&
            s.Setting.PublishThrough == MediaOutletFlag.Website
            && s.StartDate <= DateTime.Today && s.EndDate >= DateTime.Today &&
            (s.Setting.IsPublic || s.Audiences.Any(u => u.UserId == userId)));
            var list = await listQ.ToListAsync();
            var responses = await _context.SurveyResponses.Include(s => s.Respondent).
                ThenInclude(s => s.Survey).
                Where(s => listQ.Any(l => l.Id == s.Respondent.SurveyId) &&
           s.Respondent.UserId == userId).ToListAsync();
            var qustions = await _context.SurveyQuestions.Where(s => listQ.Any(l => l.SettingId == s.SurveySettingId)).ToListAsync();
            foreach (var survey in list)
            {
                survey.Setting = new SurveySetting
                {
                    QuestionsCount = qustions.Count(q => q.SurveySettingId == survey.SettingId),
                    ResponsesCount = responses.Count(r => r.Respondent.SurveyId == survey.Id),
                    OpenDays = survey.Setting.OpenDays,
                    Id = survey.Setting.Id,
                };
            }
            return list;
        }

        public async Task<List<Survey>> UserNotFinishedSurveys(Guid userId)
        {
            var list = await PublicSurveys(userId);
            return list.Where(s => s.Setting.QuestionsCount > s.Setting.ResponsesCount).Select(s => new Survey
            {
                Id = s.Id,
                Title = s.Title
            }).ToList();
        }

        public async Task<Survey> SurveyById(Guid id)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            var survey = await _context.Surveys.Include(x => x.Setting)
                                .Include(x => x.Audiences)
                                .Include(x => x.Respondents)
                                .Include(x => x.Votes)
                                .Include(x => x.Responses).ThenInclude(x => x.Choices)
                                .Include(x => x.Attachments)
                                .FirstOrDefaultAsync(i => i.Id == id);
            if (survey == null)
                return survey;

            var setting = await _context.SurveySettings
                            .Include(x => x.Questions)
                            .ThenInclude(y => y.Choices)
                            .FirstOrDefaultAsync(s => s.Id == survey.SettingId);
            survey.Setting = setting;

            return survey;
        }

        public async Task<List<string>> GetSurveyPolls(Guid id)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            var survey = await _context.Surveys
                                .FirstOrDefaultAsync(i => i.Id == id);

            return survey.PollIds;
        }
        public async Task<List<SurveyQuestion>> GetSurveyQuestions(Guid id, Guid respondentId)
        {
            var list = await _context.SurveyQuestions.Include(x => x.Setting).Where(s => s.SurveySettingId == id).ToListAsync();
            var respones = await _context.SurveyResponses.Where(s => list.Any(l => l.Id == s.QuestionId) &&
            s.SurveyRespondentId == respondentId).ToListAsync();
            list.ForEach(s => s.Response = respones.FirstOrDefault(r => r.QuestionId == s.Id));
            return list;
        }
        public async Task<SurveySetting> GetSurveySettings(Guid id, Guid respondentId)
        {
            var survey = await _context.Surveys.FirstOrDefaultAsync(s => s.Id == id);
            var settings = await _context.SurveySettings.Include(s => s.Questions).ThenInclude(s => s.Choices)//.Include(s => s.Surveys)
                .FirstOrDefaultAsync(s => s.Id == survey.SettingId);
            var respones = await _context.SurveyResponses.Include(s => s.Choices).
                Where(s => s.Respondent.SurveyId == id &&
            s.Respondent.UserId == respondentId).ToListAsync();
            settings.SelectedSurvey = new Survey { Id = survey.Id, Title = survey.Title, Description = survey.Description, AnalysisDescription = survey.AnalysisDescription };
            foreach (var q in settings.Questions)
            {
                if (q.ParentChoiceId.HasValue)
                {
                    var pchoice = settings.Choices.FirstOrDefault(c => c.Id == q.ParentChoiceId.Value);
                    q.ParentText = $"{pchoice?.Question.OrderNo} {pchoice?.ChoiceText}";
                    q.ParentText = q.ParentText.Length > 0 ? $"[{q.ParentText}]" : "";
                    q.ParentQuestionId = pchoice?.QuestionId;
                }
                var response = respones.FirstOrDefault(r => r.QuestionId == q.Id) ??
                    new SurveyResponse { Choices = new List<SelectedChoice>() };
                q.Response = new SurveyResponse
                {
                    Id = response.Id,
                    Choices = response.Choices.Select(s => new SelectedChoice
                    {
                        ChoiceId = s.ChoiceId,
                        Rank = s.Rank,
                        ResponseId = s.ResponseId,
                    }).ToList(),
                    NumericResponse = response.NumericResponse,
                    QuestionId = response.QuestionId,
                    SurveyRespondentId = response.SurveyRespondentId,
                    TextualResponse = response.TextualResponse,
                };
                q.SimpleChoices = q.Choices.Select(c => new SimpleChoice
                {
                    ChoiceText = c.ChoiceText,
                    Id = c.Id,
                    OrderNo = c.OrderNo,
                    QuestionId = c.QuestionId,
                    QuestionText = c.QuestionText,
                    Referenced = c.Referenced,
                    SurveySettingId = c.SurveySettingId
                }).ToList();
            }
            return settings;
        }
        public async Task<SurveyRespondent> GetSurveyRespondant(Guid id, Guid respondentId)
        {
            var respondant = await _context.SurveyRespondents.FirstOrDefaultAsync(r => r.UserId == respondentId &&
            r.SurveyId == id);
            var survey = await _context.Surveys.FirstOrDefaultAsync(s => s.Id == id);
            if (respondant == null)
            {
                respondant = new SurveyRespondent
                {
                    Id = Guid.NewGuid(),
                    CreatedBy = respondentId,
                    CreateStamp = DateTime.Now,
                    EndDate = DateTime.Now,
                    Remark = "",
                    UserId = respondentId,
                    StartDate = DateTime.Now,
                    SurveyId = survey.Id,
                    UpdatedBy = respondentId,
                    UpdateStamp = DateTime.Now,
                };
                _context.SurveyRespondents.Add(respondant);
                await _context.SaveChangesAsync();
            }
            return respondant;
        }

        public async Task DeleteAttachment(Guid id1)
        {
            var tmp = await _context.Attachments.FirstOrDefaultAsync(r => r.Id == id1);
            if (tmp == null)
            {
                throw new KeyNotFoundException("Attachment Not found");
            }
            _context.Attachments.Remove(tmp);
            await _context.SaveChangesAsync();
        }
        public async Task<Attachment> GetAttachment(Guid id)
        {
            var tmp = await _context.Attachments.FirstOrDefaultAsync(r => r.Id == id);
            if (tmp == null)
            {
                throw new KeyNotFoundException("Attachment Not found");
            }
            return tmp;
        }
        public async Task SaveResponses(SurveyResponse response)
        {
            if (!_context.SurveyResponses.Any(r => r.Id == response.Id))
            {
                _context.SurveyResponses.Add(response);
            }
            else
            {
                var temp = _context.SurveyResponses.FirstOrDefault(r => r.Id == response.Id);
                temp.Choices = response.Choices.ToList();
                _context.Entry(temp).CurrentValues.SetValues(response);
            }
            await _context.SaveChangesAsync();
        }
        public async Task SaveTelegramResponses(SurveyResponse response)
        {
            if (!_context.SurveyResponses.Any(r => r.Id == response.Id || 
            (r.QuestionId == response.QuestionId && r.SurveyId == response.SurveyId && r.RespondentName == response.RespondentName)))
            {
                _context.SurveyResponses.Add(response);
            }
            else
            {
                var temp = _context.SurveyResponses.Include(s => s.Choices).FirstOrDefault(r => r.Id == response.Id ||
                (r.QuestionId == response.QuestionId && r.SurveyId == response.SurveyId && r.RespondentName == response.RespondentName));
                response.Id = temp.Id;
                _context.SelectedChoices.RemoveRange(temp.Choices);
                foreach(var sc in response.Choices)
                {
                    sc.ResponseId = temp.Id;
                }
                temp.Choices = response.Choices.ToList();
                _context.Entry(temp).CurrentValues.SetValues(response);
            }
            await _context.SaveChangesAsync();
        }
        public async Task<List<LookupValue>> GetPrivateSurveySettings(ECCSAModuleFlag module)
        {
            return await _context.SurveySettings
                .Where(s => !s.IsPublic && s.Status == StatusEnum.Active &&
                s.ResponsibleUnit.HasFlag(module)).Select(x => new LookupValue()
                {
                    Id = x.Id,
                    Name = x.Title
                }).ToListAsync();
        }

        public async Task Save(Survey survey, Guid userId)
        {
            var tmp = await _context.Surveys.FirstOrDefaultAsync(s => s.Id == survey.Id && s.SettingId == survey.SettingId);
            survey.UpdatedBy = userId;
            survey.UpdateStamp = DateTime.UtcNow;
            if (tmp == null)
            {
                survey.Id = survey.Id == Guid.Empty ? Guid.NewGuid() : survey.Id;
                survey.CreatedBy = userId;
                survey.CreateStamp = DateTime.UtcNow;
                await _context.Surveys.AddAsync(survey);
                foreach (var aud in survey.Audiences)
                {
                    _context.SurveyAudiences.Add(aud);
                }
            }
            else
            {
                _context.Entry(tmp).CurrentValues.SetValues(survey);
            }

            await _context.SaveChangesAsync();
        }

        public async Task Save(Attachment attachment, Guid userId)
        {
            var tmp = await _context.Attachments.FirstOrDefaultAsync(s => s.Id == attachment.Id && s.SurveyId == attachment.SurveyId);
            attachment.UpdatedBy = userId;
            attachment.UpdateStamp = DateTime.UtcNow;
            if (tmp == null)
            {
                //attachment.Id = Guid.NewGuid();
                await _context.Attachments.AddAsync(attachment);
            }
            else
            {
                _context.Entry(tmp).CurrentValues.SetValues(attachment);
            }
            await _context.SaveChangesAsync();
        }
        public async Task SaveVotes(Guid id, List<SurveyVote> newVotes, Guid userId)
        {
            var votes = await _context.SurveyVotes.Where(s => s.SurveyId == id).ToListAsync();
            foreach (var vote in newVotes)
            {
                var oldVote = votes.FirstOrDefault(v => v.QuestionId == vote.QuestionId && v.ChoiceId == vote.ChoiceId);
                if (oldVote == null)
                {
                    _context.SurveyVotes.Add(vote);
                }
                else
                {
                    oldVote.Votes = vote.Votes;
                    _context.SurveyVotes.Update(oldVote);
                }
            }

            await _context.SaveChangesAsync();
        }
        public async Task<SurveyPayload> Create(GenericInput<CreateSurveyInput> input)
        {
            Survey data = new()
            {
                Id = Guid.NewGuid()
            ,
                SettingId = input.PayLoad.SettingId
            ,
                Title = input.PayLoad.Title
            ,
                Description = input.PayLoad.Description
            ,
                StartDate = input.PayLoad.StartDate
            ,
                EndDate = input.PayLoad.EndDate
            ,
                AnalysisDescription = input.PayLoad.AnalysisDescription
            ,
                Attachment = input.PayLoad.Attachment
            ,
                CreatedBy = input.UserId
            ,
                CreateStamp = DateTime.UtcNow
            ,
                UpdatedBy = input.UserId
            ,
                UpdateStamp = DateTime.UtcNow

            };
            await _context.Surveys.AddAsync(data);
            _ = await _context.SaveChangesAsync();
            return new SurveyPayload(data);
        }

        public async Task<SurveyPayload> Update(GenericInput<UpdateSurveyInput> data)
        {
            var tmp = await _context.Surveys.FirstOrDefaultAsync(r => r.Id == data.PayLoad.Id);
            if (tmp == null) throw new KeyNotFoundException("Survey not found");
            tmp.SettingId = data.PayLoad.SettingId;
            tmp.Title = data.PayLoad.Title;
            tmp.Description = data.PayLoad.Description;
            tmp.StartDate = data.PayLoad.StartDate;
            tmp.EndDate = data.PayLoad.EndDate;
            tmp.AnalysisDescription = data.PayLoad.AnalysisDescription;
            tmp.Attachment = data.PayLoad.Attachment;
            tmp.UpdatedBy = data.UserId;
            tmp.UpdateStamp = DateTime.UtcNow;

            _context.Surveys.Update(tmp);
            await _context.SaveChangesAsync();
            return new SurveyPayload(tmp);

        }
        public async Task DeleteSurvey(Guid id)
        {
            var tmp = await _context.Surveys.Include(q => q.Responses).Include(q => q.Attachments).FirstOrDefaultAsync(r => r.Id == id);
            if (tmp == null) throw new KeyNotFoundException("Survey not found");
            if (tmp.Responses.Count > 0 || tmp.Attachments.Count > 0)
            {
                throw new InvalidOperationException("Cannot delete survey that has response and/or attachments");
            }
            _context.Surveys.Remove(tmp);
            if (_context.Trainings.Any(c => c.SurveyId == tmp.Id))
            {
                var trainings = _context.Trainings.Where(c => c.SurveyId == tmp.Id).ToList();
                foreach (var t in trainings)
                {
                    t.SurveyId = null;
                    _context.Trainings.Update(t);
                }
            }
            await _context.SaveChangesAsync();
        }
        public async Task Delete(Survey data)
        {
            var tmp = await _context.Surveys.FirstOrDefaultAsync(r => r.Id == data.Id);
            if (tmp == null) throw new KeyNotFoundException("Survey not found");
            _context.Surveys.Remove(tmp);
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
    }
}