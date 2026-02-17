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
    public class RepresentativeContactRepository 
    {
        private readonly EccsaContext _context;
        private readonly EccsaUserContext _userContext;
        public RepresentativeContactRepository(EccsaContext context, EccsaUserContext userContext)
        {
            _context = context;
            _userContext = userContext;
        }

        public IQueryable<RepresentativeContact> RepresentativeContacts(Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return _context.RepresentativeContacts.Include(x => x.Representative)
;
        }

        public async Task<RepresentativeContact> RepresentativeContactById(Guid id, Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return await _context.RepresentativeContacts.Include(x => x.Representative)
.FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task<RepresentativeContactPayload> Create(GenericInput<CreateRepresentativeContactInput> input)
        {
            RepresentativeContact data = new()
            {
                			Id = Guid.NewGuid()
			, RepresentativeId = input.PayLoad.RepresentativeId
			, ContactType = input.PayLoad.ContactType
			, Address = input.PayLoad.Address
			, Priority = input.PayLoad.Priority

            };
            await _context.RepresentativeContacts.AddAsync(data);
           var val = await _context.SaveChangesAsync();
           return new RepresentativeContactPayload(data);
        }

        public async Task<RepresentativeContactPayload> Update(GenericInput<UpdateRepresentativeContactInput> data)
        {
            var tmp = await  _context.RepresentativeContacts.FirstOrDefaultAsync(r => r.Id == data.PayLoad.Id);
            if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
            						tmp.RepresentativeId = data.PayLoad.RepresentativeId;
			tmp.ContactType = data.PayLoad.ContactType;
			tmp.Address = data.PayLoad.Address;
			tmp.Priority = data.PayLoad.Priority;

            _context.RepresentativeContacts.Update(tmp);
            await _context.SaveChangesAsync();
            return new RepresentativeContactPayload(tmp);

        }

        public async Task Delete(RepresentativeContact data)
        {
            var tmp = await _context.RepresentativeContacts.FirstOrDefaultAsync(r => r.Id == data.Id);
            if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
            _context.RepresentativeContacts.Remove(tmp);
            await _context.SaveChangesAsync();
        }
    }
}