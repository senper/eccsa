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
    public class ContactRepository
    {
        private readonly EccsaContext _context;
        private readonly EccsaUserContext _userContext;
        public ContactRepository(EccsaContext context, EccsaUserContext userContext)
        {
            _context = context;
            _userContext = userContext;
        }

        public IQueryable<Contact> Contacts(Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return _context.Contacts.Include(x => x.Stakeholder)
;
        }

        public async Task<List<Contact>> Contacts(Guid memberId, Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return await Contacts(userId).Where(c => c.MemberId == memberId).ToListAsync();
            ;
        }

        public async Task<Contact> ContactById(Guid id, Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return await _context.Contacts.Include(x => x.Stakeholder)
.FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task<ContactPayload> Create(GenericInput<CreateContactInput> input)
        {
            Contact data = new()
            {
                Id = Guid.NewGuid()
            ,
                MemberId = input.PayLoad.MemberId
            ,
                ContactType = input.PayLoad.ContactType
            ,
                Address = input.PayLoad.Address
            ,
                Priority = input.PayLoad.Priority

            };
            if (!Validation.IsValid(data.ContactType, data.Address))
                throw new InvalidCastException("Invalid data");
            await _context.Contacts.AddAsync(data);
            var val = await _context.SaveChangesAsync();
            return new ContactPayload(data);
        }

        public async Task<ContactPayload> Update(GenericInput<UpdateContactInput> data)
        {
            var tmp = await _context.Contacts.FirstOrDefaultAsync(r => r.Id == data.PayLoad.Id);
            if (tmp == null) throw new KeyNotFoundException("Contact not found");
            if (!Validation.IsValid(data.PayLoad.ContactType, data.PayLoad.Address))
                throw new InvalidCastException("Invalid data");
            tmp.MemberId = data.PayLoad.MemberId;
            tmp.ContactType = data.PayLoad.ContactType;
            tmp.Address = data.PayLoad.Address;
            tmp.Priority = data.PayLoad.Priority;

            _context.Contacts.Update(tmp);
            await _context.SaveChangesAsync();
            return new ContactPayload(tmp);

        }

        public async Task Delete(Guid id)
        {
            var tmp = await _context.Contacts.FirstOrDefaultAsync(r => r.Id == id);
            if (tmp == null) return;
            _context.Contacts.Remove(tmp);
            await _context.SaveChangesAsync();
        }
    }
}