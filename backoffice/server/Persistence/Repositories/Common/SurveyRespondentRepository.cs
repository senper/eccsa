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
	public class SurveyRespondentRepository 
	{
		private readonly EccsaContext _context;
		private readonly EccsaUserContext _userContext;
		public SurveyRespondentRepository(EccsaContext context, EccsaUserContext userContext)
		{
			_context = context;
			_userContext = userContext;
		}

		public IQueryable<SurveyRespondent> SurveyRespondents(Guid userId)
		{
			// var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
			return _context.SurveyRespondents.Include(x => x.Responses)
.Include(x => x.Survey)
;
		}

		public async Task<SurveyRespondent> SurveyRespondentById(Guid id, Guid userId)
		{
			// var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
			return await _context.SurveyRespondents.Include(x => x.Responses)
.Include(x => x.Survey)
.FirstOrDefaultAsync(i => i.Id == id);
		}
		public async Task<SurveyRespondentPayload> Create(GenericInput<CreateSurveyRespondentInput> input)
		{
			SurveyRespondent data = new()
			{
							Id = Guid.NewGuid()
			, SurveyId = input.PayLoad.SurveyId
			, UserId = input.PayLoad.RespondentStakeholderId
			, StartDate = input.PayLoad.StartDate
			, EndDate = input.PayLoad.EndDate
			, Remark = input.PayLoad.Remark
			, CreatedBy = input.UserId
			, CreateStamp = DateTime.UtcNow
			, UpdatedBy = input.UserId
			, UpdateStamp = DateTime.UtcNow

			};
			await _context.SurveyRespondents.AddAsync(data);
		   var val = await _context.SaveChangesAsync();
		   return new SurveyRespondentPayload(data);
		}

		public async Task<SurveyRespondentPayload> Update(GenericInput<UpdateSurveyRespondentInput> data)
		{
			var tmp = await  _context.SurveyRespondents.FirstOrDefaultAsync(r => r.Id == data.PayLoad.Id);
			if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
									tmp.SurveyId = data.PayLoad.SurveyId;
			tmp.UserId = data.PayLoad.RespondentStakeholderId;
			tmp.StartDate = data.PayLoad.StartDate;
			tmp.EndDate = data.PayLoad.EndDate;
			tmp.Remark = data.PayLoad.Remark;
									tmp.UpdatedBy = data.UserId;
			tmp.UpdateStamp = DateTime.UtcNow;

			_context.SurveyRespondents.Update(tmp);
			await _context.SaveChangesAsync();
			return new SurveyRespondentPayload(tmp);

		}

		public async Task Delete(SurveyRespondent data)
		{
			var tmp = await _context.SurveyRespondents.FirstOrDefaultAsync(r => r.Id == data.Id);
			if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
			_context.SurveyRespondents.Remove(tmp);
			await _context.SaveChangesAsync();
		}
	}
}