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
    public class MemberRepository 
    {
        private readonly EccsaContext _context;
        private readonly EccsaUserContext _userContext;
        public MemberRepository(EccsaContext context, EccsaUserContext userContext)
        {
            _context = context;
            _userContext = userContext;
        }

        public IQueryable<Member> Members(Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return _context.Members.Include(x => x.NeedAssessmentDetails)
;
        }

        public async Task<Member> MemberById(Guid id, Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return await _context.Members.Include(x => x.NeedAssessmentDetails)
.FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task<MemberPayload> Create(GenericInput<CreateMemberInput> input)
        {
            Member data = new()
            {
                			Id = Guid.NewGuid()
			, NameEng = input.PayLoad.NameEng
			, NameLocal = input.PayLoad.NameLocal
			, MemberType = input.PayLoad.MemberType
			, MembershipId = input.PayLoad.MembershipId
			, AddressType = input.PayLoad.AddressType
			, LocationId = input.PayLoad.LocationId
			, AddressLine = input.PayLoad.AddressLine
			, ContactType = input.PayLoad.ContactType
			, Contact = input.PayLoad.Contact

            };
            await _context.Members.AddAsync(data);
           var val = await _context.SaveChangesAsync();
           return new MemberPayload(data);
        }

        public async Task<MemberPayload> Update(GenericInput<UpdateMemberInput> data)
        {
            var tmp = await  _context.Members.FirstOrDefaultAsync(r => r.Id == data.PayLoad.Id);
            if (tmp == null) throw new KeyNotFoundException("Member not found");
            						tmp.NameEng = data.PayLoad.NameEng;
			tmp.NameLocal = data.PayLoad.NameLocal;
			tmp.MemberType = data.PayLoad.MemberType;
			tmp.MembershipId = data.PayLoad.MembershipId;
			tmp.AddressType = data.PayLoad.AddressType;
			tmp.LocationId = data.PayLoad.LocationId;
			tmp.AddressLine = data.PayLoad.AddressLine;
			tmp.ContactType = data.PayLoad.ContactType;
			tmp.Contact = data.PayLoad.Contact;

            _context.Members.Update(tmp);
            await _context.SaveChangesAsync();
            return new MemberPayload(tmp);

        }

        public async Task Delete(Member data)
        {
            var tmp = await _context.Members.FirstOrDefaultAsync(r => r.Id == data.Id);
            if (tmp == null) throw new KeyNotFoundException("Member not found");
            _context.Members.Remove(tmp);
            await _context.SaveChangesAsync();
        }
    }
}