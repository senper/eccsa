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
    public class ArbitrationRelatedPartyRepository 
    {
        private readonly EccsaContext _context;
        private readonly EccsaUserContext _userContext;
        public ArbitrationRelatedPartyRepository(EccsaContext context, EccsaUserContext userContext)
        {
            _context = context;
            _userContext = userContext;
        }

        public IQueryable<ArbitrationRelatedParty> ArbitrationRelatedParties(Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return _context.ArbitrationRelatedParties.Include(x => x.ArbitrationService)
;
        }

        public async Task<ArbitrationRelatedParty> ArbitrationRelatedPartyById(Guid id, Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return await _context.ArbitrationRelatedParties.Include(x => x.ArbitrationService)
.FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task<ArbitrationRelatedPartyPayload> Create(GenericInput<CreateArbitrationRelatedPartyInput> input)
        {
            ArbitrationRelatedParty data = new()
            {
                			Id = Guid.NewGuid()
			, ServiceId = input.PayLoad.ServiceId
			, OrderNo = input.PayLoad.OrderNo
			, Name = input.PayLoad.Name
			, TINumber = input.PayLoad.TINumber
			, Address = input.PayLoad.Address

            };
            await _context.ArbitrationRelatedParties.AddAsync(data);
           var val = await _context.SaveChangesAsync();
           return new ArbitrationRelatedPartyPayload(data);
        }

        public async Task<ArbitrationRelatedPartyPayload> Update(GenericInput<UpdateArbitrationRelatedPartyInput> data)
        {
            var tmp = await  _context.ArbitrationRelatedParties.FirstOrDefaultAsync(r => r.Id == data.PayLoad.Id);
            if (tmp == null) throw new KeyNotFoundException("Related Party not found");
            						tmp.ServiceId = data.PayLoad.ServiceId;
			tmp.OrderNo = data.PayLoad.OrderNo;
			tmp.Name = data.PayLoad.Name;
			tmp.TINumber = data.PayLoad.TINumber;
			tmp.Address = data.PayLoad.Address;

            _context.ArbitrationRelatedParties.Update(tmp);
            await _context.SaveChangesAsync();
            return new ArbitrationRelatedPartyPayload(tmp);

        }

        public async Task Delete(ArbitrationRelatedParty data)
        {
            var tmp = await _context.ArbitrationRelatedParties.FirstOrDefaultAsync(r => r.Id == data.Id);
            if (tmp == null) throw new KeyNotFoundException("Related Party not found");
            _context.ArbitrationRelatedParties.Remove(tmp);
            await _context.SaveChangesAsync();
        }
    }
}