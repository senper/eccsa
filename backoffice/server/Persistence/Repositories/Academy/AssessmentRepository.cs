using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Persistence.DTO;
using ECCSA.BackOffice.Server.Model.Academy;
using ECCSA.BackOffice.Server.Persistence.Context;
using ECCSA.BackOffice.Server.Persistence.DTO.Academy;
using Microsoft.EntityFrameworkCore;

namespace ECCSA.BackOffice.Server.Persistence.Repositories.Academy
{
    public class AssessmentRepository 
    {
        private readonly EccsaContext _context;
        private readonly EccsaUserContext _userContext;
        public AssessmentRepository(EccsaContext context, EccsaUserContext userContext)
        {
            _context = context;
            _userContext = userContext;
        }

        public IQueryable<Assessment> Assessments(Guid userId, Guid registrationId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return _context.Assessments.Include(x => x.Training)
.Include(x => x.Registration).Where(a => a.RegistrationId == registrationId)
;
        }

        public async Task<Assessment> AssessmentById(Guid id, Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return await _context.Assessments.Include(x => x.Training)
.Include(x => x.Registration)
.FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task<AssessmentPayload> Create(GenericInput<CreateAssessmentInput> input)
        {
            Assessment data = new()
            {
                			Id = Guid.NewGuid()
			, TrainingId = input.PayLoad.TrainingId
			, RegistrationId = input.PayLoad.RegistrationId
			, Date = input.PayLoad.Date
			, ResultScore = input.PayLoad.ResultScore
			, ResultPass = input.PayLoad.ResultPass
			, CreatedBy = input.UserId
			, CreateStamp = DateTime.UtcNow
			, UpdatedBy = input.UserId
			, UpdateStamp = DateTime.UtcNow

            };
            await _context.Assessments.AddAsync(data);
           var val = await _context.SaveChangesAsync();
           return new AssessmentPayload(data);
        }

        public async Task<AssessmentPayload> Update(GenericInput<UpdateAssessmentInput> data)
        {
            var tmp = await  _context.Assessments.FirstOrDefaultAsync(r => r.Id == data.PayLoad.Id);
            if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
            						tmp.TrainingId = data.PayLoad.TrainingId;
			tmp.RegistrationId = data.PayLoad.RegistrationId;
			tmp.Date = data.PayLoad.Date;
			tmp.ResultScore = data.PayLoad.ResultScore;
			tmp.ResultPass = data.PayLoad.ResultPass;
									tmp.UpdatedBy = data.UserId;
			tmp.UpdateStamp = DateTime.UtcNow;

            _context.Assessments.Update(tmp);
            await _context.SaveChangesAsync();
            return new AssessmentPayload(tmp);

        }

        public async Task Delete(Assessment data)
        {
            var tmp = await _context.Assessments.FirstOrDefaultAsync(r => r.Id == data.Id);
            if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
            _context.Assessments.Remove(tmp);
            await _context.SaveChangesAsync();
        }
    }
}