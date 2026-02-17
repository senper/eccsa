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
    public class AddressRepository 
    {
        private readonly EccsaContext _context;
        private readonly EccsaUserContext _userContext;
        public AddressRepository(EccsaContext context, EccsaUserContext userContext)
        {
            _context = context;
            _userContext = userContext;
        }

        public IQueryable<Address> Addresses(Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return _context.Addresses.Include(x => x.Stakeholder)
;
        }

        public async Task<Address> AddressById(Guid id, Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return await _context.Addresses.Include(x => x.Stakeholder)
.FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task<AddressPayload> Create(GenericInput<CreateAddressInput> input)
        {
            Address data = new()
            {
                			Id = Guid.NewGuid()
			, MemberId = input.PayLoad.MemberId
			, AddressType = input.PayLoad.AddressType
			, LocationId = input.PayLoad.LocationId
			, AddressLine = input.PayLoad.AddressLine

            };
            await _context.Addresses.AddAsync(data);
           var val = await _context.SaveChangesAsync();
           return new AddressPayload(data);
        }

        public async Task<AddressPayload> Update(GenericInput<UpdateAddressInput> data)
        {
            var tmp = await  _context.Addresses.FirstOrDefaultAsync(r => r.Id == data.PayLoad.Id);
            if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
            						tmp.MemberId = data.PayLoad.MemberId;
			tmp.AddressType = data.PayLoad.AddressType;
			tmp.LocationId = data.PayLoad.LocationId;
			tmp.AddressLine = data.PayLoad.AddressLine;

            _context.Addresses.Update(tmp);
            await _context.SaveChangesAsync();
            return new AddressPayload(tmp);

        }

        public async Task Delete(Address data)
        {
            var tmp = await _context.Addresses.FirstOrDefaultAsync(r => r.Id == data.Id);
            if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
            _context.Addresses.Remove(tmp);
            await _context.SaveChangesAsync();
        }
    }
}