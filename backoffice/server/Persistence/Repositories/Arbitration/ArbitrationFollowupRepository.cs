using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Persistence.DTO;
using ECCSA.BackOffice.Server.Model.Arbitration;
using ECCSA.BackOffice.Server.Persistence.Context;
using ECCSA.BackOffice.Server.Persistence.DTO.Arbitration;
using Microsoft.EntityFrameworkCore;

namespace ECCSA.BackOffice.Server.Persistence.Repositories.Arbitration
{
    public class ArbitrationFollowupRepository 
    {
        private readonly EccsaContext _context;
        private readonly EccsaUserContext _userContext;
        public ArbitrationFollowupRepository(EccsaContext context, EccsaUserContext userContext)
        {
            _context = context;
            _userContext = userContext;
        }

        public IQueryable<ArbitrationFollowup> ArbitrationFollowups(Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return _context.ArbitrationFollowups.Include(x => x.ArbitrationService)
.Include(x => x.TaskType)
.Include(x => x.FollowupResult)
;
        }

        public async Task<ArbitrationFollowup> ArbitrationFollowupById(Guid id, Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return await _context.ArbitrationFollowups.Include(x => x.ArbitrationService)
.Include(x => x.TaskType)
.Include(x => x.FollowupResult)
.FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task<ArbitrationFollowupPayload> Create(GenericInput<CreateArbitrationFollowupInput> input)
        {
            ArbitrationFollowup data = new()
            {
                			Id = Guid.NewGuid()
			, ServiceId = input.PayLoad.ServiceId
			, TaskTypeId = input.PayLoad.TaskTypeId
			, PlannedDate = input.PayLoad.PlannedDate
			, FollowupResultId = input.PayLoad.FollowupResultId
			, Description = input.PayLoad.Description
			, Remark = input.PayLoad.Remark
			, CreatedBy = input.UserId
			, CreateStamp = DateTime.UtcNow
			, UpdatedBy = input.UserId
			, UpdateStamp = DateTime.UtcNow

            };
            await _context.ArbitrationFollowups.AddAsync(data);
           var val = await _context.SaveChangesAsync();
           return new ArbitrationFollowupPayload(data);
        }

        public async Task<ArbitrationFollowupPayload> Update(GenericInput<UpdateArbitrationFollowupInput> data)
        {
            var tmp = await  _context.ArbitrationFollowups.FirstOrDefaultAsync(r => r.Id == data.PayLoad.Id);
            if (tmp == null) throw new KeyNotFoundException("Follow-up not found");
            						tmp.ServiceId = data.PayLoad.ServiceId;
			tmp.TaskTypeId = data.PayLoad.TaskTypeId;
			tmp.PlannedDate = data.PayLoad.PlannedDate;
			tmp.FollowupResultId = data.PayLoad.FollowupResultId;
			tmp.Description = data.PayLoad.Description;
			tmp.Remark = data.PayLoad.Remark;
									tmp.UpdatedBy = data.UserId;
			tmp.UpdateStamp = DateTime.UtcNow;

            _context.ArbitrationFollowups.Update(tmp);
            await _context.SaveChangesAsync();
            return new ArbitrationFollowupPayload(tmp);

        }

        public async Task Delete(ArbitrationFollowup data)
        {
            var tmp = await _context.ArbitrationFollowups.FirstOrDefaultAsync(r => r.Id == data.Id);
            if (tmp == null) throw new KeyNotFoundException("Follow-up not found");
            _context.ArbitrationFollowups.Remove(tmp);
            await _context.SaveChangesAsync();
        }
    }
}