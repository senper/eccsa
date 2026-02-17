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
    public class NeedAssessmentDetailRepository 
    {
        private readonly EccsaContext _context;
        private readonly EccsaUserContext _userContext;
        public NeedAssessmentDetailRepository(EccsaContext context, EccsaUserContext userContext)
        {
            _context = context;
            _userContext = userContext;
        }

        public IQueryable<NeedAssessmentDetail> NeedAssessmentDetails(Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return _context.NeedAssessmentDetails.Include(x => x.Category)
.Include(x => x.NeedAssessment)
.Include(x => x.Member)
;
        }

        public async Task<NeedAssessmentDetail> NeedAssessmentDetailById(Guid id, Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return await _context.NeedAssessmentDetails.Include(x => x.Category)
.Include(x => x.NeedAssessment)
.Include(x => x.Member)
.FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task<NeedAssessmentDetailPayload> Create(GenericInput<CreateNeedAssessmentDetailInput> input)
        {
            NeedAssessmentDetail data = new()
            {
                			Id = Guid.NewGuid()
			, CategoryId = input.PayLoad.CategoryId
			, MemberId = input.PayLoad.MemberId
			, AssessmentId = input.PayLoad.AssessmentId
			, NeedRating = input.PayLoad.NeedRating
			, BestBefore = input.PayLoad.BestBefore
			, Remark = input.PayLoad.Remark
			, Status = input.PayLoad.Status

            };
            await _context.NeedAssessmentDetails.AddAsync(data);
           var val = await _context.SaveChangesAsync();
           return new NeedAssessmentDetailPayload(data);
        }

        public async Task<NeedAssessmentDetailPayload> Update(GenericInput<UpdateNeedAssessmentDetailInput> data)
        {
            var tmp = await  _context.NeedAssessmentDetails.FirstOrDefaultAsync(r => r.Id == data.PayLoad.Id);
            if (tmp == null) throw new KeyNotFoundException("Assessment not found");
            						tmp.CategoryId = data.PayLoad.CategoryId;
			tmp.MemberId = data.PayLoad.MemberId;
			tmp.AssessmentId = data.PayLoad.AssessmentId;
			tmp.NeedRating = data.PayLoad.NeedRating;
			tmp.BestBefore = data.PayLoad.BestBefore;
			tmp.Remark = data.PayLoad.Remark;
			tmp.Status = data.PayLoad.Status;

            _context.NeedAssessmentDetails.Update(tmp);
            await _context.SaveChangesAsync();
            return new NeedAssessmentDetailPayload(tmp);

        }

        public async Task Delete(NeedAssessmentDetail data)
        {
            var tmp = await _context.NeedAssessmentDetails.FirstOrDefaultAsync(r => r.Id == data.Id);
            if (tmp == null) throw new KeyNotFoundException("Assessment not found");
            _context.NeedAssessmentDetails.Remove(tmp);
            await _context.SaveChangesAsync();
        }
    }
}