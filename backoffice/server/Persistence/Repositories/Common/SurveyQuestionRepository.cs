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
    public class SurveyQuestionRepository 
    {
        private readonly EccsaContext _context;
        private readonly EccsaUserContext _userContext;
        public SurveyQuestionRepository(EccsaContext context, EccsaUserContext userContext)
        {
            _context = context;
            _userContext = userContext;
        }

        public IQueryable<SurveyQuestion> SurveyQuestions(Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return _context.SurveyQuestions.Include(x => x.Choices)
.Include(x => x.Setting)
;
        }

        public async Task<SurveyQuestion> SurveyQuestionById(Guid id, Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return await _context.SurveyQuestions.Include(x => x.Choices)
.Include(x => x.Setting)
.FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task<SurveyQuestionPayload> Create(GenericInput<CreateSurveyQuestionInput> input)
        {
            SurveyQuestion data = new()
            {
                			Id = Guid.NewGuid()
			, SurveySettingId = input.PayLoad.SurveySettingId
			, OrderNo = input.PayLoad.OrderNo
			, Question = input.PayLoad.Question
			, QuestionType = input.PayLoad.QuestionType
			, MinRange = input.PayLoad.MinRange
			, MaxRange = input.PayLoad.MaxRange
			, Cascaded = input.PayLoad.Cascaded
			, ParentChoiceId = input.PayLoad.ParentChoiceId

            };
            await _context.SurveyQuestions.AddAsync(data);
           var val = await _context.SaveChangesAsync();
           return new SurveyQuestionPayload(data);
        }

        public async Task<SurveyQuestionPayload> Update(GenericInput<UpdateSurveyQuestionInput> data)
        {
            var tmp = await  _context.SurveyQuestions.FirstOrDefaultAsync(r => r.Id == data.PayLoad.Id);
            if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
            						tmp.SurveySettingId = data.PayLoad.SurveySettingId;
			tmp.OrderNo = data.PayLoad.OrderNo;
			tmp.Question = data.PayLoad.Question;
			tmp.QuestionType = data.PayLoad.QuestionType;
			tmp.MinRange = data.PayLoad.MinRange;
			tmp.MaxRange = data.PayLoad.MaxRange;
			tmp.Cascaded = data.PayLoad.Cascaded;
			tmp.ParentChoiceId = data.PayLoad.ParentChoiceId;

            _context.SurveyQuestions.Update(tmp);
            await _context.SaveChangesAsync();
            return new SurveyQuestionPayload(tmp);

        }

        public async Task Delete(SurveyQuestion data)
        {
            var tmp = await _context.SurveyQuestions.FirstOrDefaultAsync(r => r.Id == data.Id);
            if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
            _context.SurveyQuestions.Remove(tmp);
            await _context.SaveChangesAsync();
        }
    }
}