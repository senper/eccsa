using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Persistence.DTO;
using ECCSA.BackOffice.Server.Model.Membership;
using ECCSA.BackOffice.Server.Persistence.Context;
using ECCSA.BackOffice.Server.Persistence.DTO.Membership;
using Microsoft.EntityFrameworkCore;

namespace ECCSA.BackOffice.Server.Persistence.Repositories.Membership
{
    public class StakeholderRepository 
    {
        private readonly EccsaContext _context;
        private readonly EccsaUserContext _userContext;
        public StakeholderRepository(EccsaContext context, EccsaUserContext userContext)
        {
            _context = context;
            _userContext = userContext;
        }

        public IQueryable<Stakeholder> Stakeholders(Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return _context.Stakeholders.Include(x => x.Contacts)
.Include(x => x.Representatives)
.Include(x => x.Addresses)
.Include(x => x.MembershipEnrollments)
;
        }

        public async Task<Stakeholder> StakeholderById(Guid id, Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return await _context.Stakeholders.Include(x => x.Contacts)
.Include(x => x.Representatives)
.Include(x => x.Addresses)
.Include(x => x.MembershipEnrollments)
.FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task<StakeholderPayload> Create(GenericInput<CreateStakeholderInput> input)
        {
            Stakeholder data = new()
            {
                			Id = Guid.NewGuid()
			, StakeholderType = input.PayLoad.StakeholderType
			, Status = input.PayLoad.Status

            };
            await _context.Stakeholders.AddAsync(data);
           var val = await _context.SaveChangesAsync();
           return new StakeholderPayload(data);
        }

        public async Task<StakeholderPayload> Update(GenericInput<UpdateStakeholderInput> data)
        {
            var tmp = await  _context.Stakeholders.FirstOrDefaultAsync(r => r.Id == data.PayLoad.Id);
            if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
            						tmp.StakeholderType = data.PayLoad.StakeholderType;
			tmp.Status = data.PayLoad.Status;

            _context.Stakeholders.Update(tmp);
            await _context.SaveChangesAsync();
            return new StakeholderPayload(tmp);

        }

        public async Task Delete(Stakeholder data)
        {
            var tmp = await _context.Stakeholders.FirstOrDefaultAsync(r => r.Id == data.Id);
            if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
            _context.Stakeholders.Remove(tmp);
            await _context.SaveChangesAsync();
        }
    }
}