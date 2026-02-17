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
    public class SurveyChoiceRepository 
    {
        private readonly EccsaContext _context;
        private readonly EccsaUserContext _userContext;
        public SurveyChoiceRepository(EccsaContext context, EccsaUserContext userContext)
        {
            _context = context;
            _userContext = userContext;
        }

        public IQueryable<SurveyChoice> SurveyChoices(Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return _context.SurveyChoices.Include(x => x.Question)
.Include(x => x.SurveySetting)
.Include(x => x.Responses)
;
        }

        public async Task<SurveyChoice> SurveyChoiceById(Guid id, Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return await _context.SurveyChoices.Include(x => x.Question)
.Include(x => x.SurveySetting)
.Include(x => x.Responses)
.FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task<SurveyChoicePayload> Create(GenericInput<CreateSurveyChoiceInput> input)
        {
            SurveyChoice data = new()
            {
                			Id = Guid.NewGuid()
			, SurveySettingId = input.PayLoad.SurveySettingId
			, OrderNo = input.PayLoad.OrderNo
			, QuestionId = input.PayLoad.QuestionId
			, ChoiceText = input.PayLoad.ChoiceText

            };
            await _context.SurveyChoices.AddAsync(data);
           var val = await _context.SaveChangesAsync();
           return new SurveyChoicePayload(data);
        }

        public async Task<SurveyChoicePayload> Update(GenericInput<UpdateSurveyChoiceInput> data)
        {
            var tmp = await  _context.SurveyChoices.FirstOrDefaultAsync(r => r.Id == data.PayLoad.Id);
            if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
            						tmp.SurveySettingId = data.PayLoad.SurveySettingId;
			tmp.OrderNo = data.PayLoad.OrderNo;
			tmp.QuestionId = data.PayLoad.QuestionId;
			tmp.ChoiceText = data.PayLoad.ChoiceText;

            _context.SurveyChoices.Update(tmp);
            await _context.SaveChangesAsync();
            return new SurveyChoicePayload(tmp);

        }

        public async Task Delete(SurveyChoice data)
        {
            var tmp = await _context.SurveyChoices.FirstOrDefaultAsync(r => r.Id == data.Id);
            if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
            _context.SurveyChoices.Remove(tmp);
            await _context.SaveChangesAsync();
        }
    }
}