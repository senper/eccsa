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
    public class SurveyResponseRepository 
    {
        private readonly EccsaContext _context;
        private readonly EccsaUserContext _userContext;
        public SurveyResponseRepository(EccsaContext context, EccsaUserContext userContext)
        {
            _context = context;
            _userContext = userContext;
        }

        public IQueryable<SurveyResponse> SurveyResponses(Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return _context.SurveyResponses.Include(x => x.Choices)
.Include(x => x.Respondent)
;
        }

        public async Task<SurveyResponse> SurveyResponseById(Guid id, Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return await _context.SurveyResponses.Include(x => x.Choices)
.Include(x => x.Respondent)
.FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task<SurveyResponsePayload> Create(GenericInput<CreateSurveyResponseInput> input)
        {
            SurveyResponse data = new()
            {
                			Id = Guid.NewGuid()
			, SurveyRespondentId = input.PayLoad.SurveyRespondentId
			, QuestionId = input.PayLoad.QuestionId
			, NumericResponse = input.PayLoad.NumericResponse
			, TextualResponse = input.PayLoad.TextualResponse

            };
            await _context.SurveyResponses.AddAsync(data);
           var val = await _context.SaveChangesAsync();
           return new SurveyResponsePayload(data);
        }

        public async Task<SurveyResponsePayload> Update(GenericInput<UpdateSurveyResponseInput> data)
        {
            var tmp = await  _context.SurveyResponses.FirstOrDefaultAsync(r => r.Id == data.PayLoad.Id);
            if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
            						tmp.SurveyRespondentId = data.PayLoad.SurveyRespondentId;
			tmp.QuestionId = data.PayLoad.QuestionId;
			tmp.NumericResponse = data.PayLoad.NumericResponse;
			tmp.TextualResponse = data.PayLoad.TextualResponse;

            _context.SurveyResponses.Update(tmp);
            await _context.SaveChangesAsync();
            return new SurveyResponsePayload(tmp);

        }

        public async Task Delete(SurveyResponse data)
        {
            var tmp = await _context.SurveyResponses.FirstOrDefaultAsync(r => r.Id == data.Id);
            if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
            _context.SurveyResponses.Remove(tmp);
            await _context.SaveChangesAsync();
        }
    }
}