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
    public class NeedAssessmentRepository 
    {
        private readonly EccsaContext _context;
        private readonly EccsaUserContext _userContext;
        public NeedAssessmentRepository(EccsaContext context, EccsaUserContext userContext)
        {
            _context = context;
            _userContext = userContext;
        }

        public IQueryable<NeedAssessment> NeedAssessments(Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return _context.NeedAssessments.Include(x => x.NeedAssessmentDetails)
;
        }

        public async Task<NeedAssessment> NeedAssessmentById(Guid id, Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return await _context.NeedAssessments.Include(x => x.NeedAssessmentDetails)
.FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task<NeedAssessmentPayload> Create(GenericInput<CreateNeedAssessmentInput> input)
        {
            NeedAssessment data = new()
            {
                			Id = Guid.NewGuid()
			, StartDate = input.PayLoad.StartDate
			, EndDate = input.PayLoad.EndDate
			, Remark = input.PayLoad.Remark
			, Status = input.PayLoad.Status

            };
            await _context.NeedAssessments.AddAsync(data);
           var val = await _context.SaveChangesAsync();
           return new NeedAssessmentPayload(data);
        }

        public async Task<NeedAssessmentPayload> Update(GenericInput<UpdateNeedAssessmentInput> data)
        {
            var tmp = await  _context.NeedAssessments.FirstOrDefaultAsync(r => r.Id == data.PayLoad.Id);
            if (tmp == null) throw new KeyNotFoundException("Assessment not found");
            						tmp.StartDate = data.PayLoad.StartDate;
			tmp.EndDate = data.PayLoad.EndDate;
			tmp.Remark = data.PayLoad.Remark;
			tmp.Status = data.PayLoad.Status;

            _context.NeedAssessments.Update(tmp);
            await _context.SaveChangesAsync();
            return new NeedAssessmentPayload(tmp);

        }

        public async Task Delete(NeedAssessment data)
        {
            var tmp = await _context.NeedAssessments.FirstOrDefaultAsync(r => r.Id == data.Id);
            if (tmp == null) throw new KeyNotFoundException("Assessment not found");
            _context.NeedAssessments.Remove(tmp);
            await _context.SaveChangesAsync();
        }
    }
}