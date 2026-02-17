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
    public class LegalDocumentTypeRepository 
    {
        private readonly EccsaContext _context;
        private readonly EccsaUserContext _userContext;
        public LegalDocumentTypeRepository(EccsaContext context, EccsaUserContext userContext)
        {
            _context = context;
            _userContext = userContext;
        }

        public IQueryable<LegalDocumentType> LegalDocumentTypes(Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return _context.LegalDocumentTypes//.Include(x => x.LegalDocuments)
//.Include(x => x.ApplicantDocuments)
;
        }

        public async Task<LegalDocumentType> LegalDocumentTypeById(int id, Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return await _context.LegalDocumentTypes//.Include(x => x.LegalDocuments)
//.Include(x => x.ApplicantDocuments)
.FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task<LegalDocumentTypePayload> Create(GenericInput<CreateLegalDocumentTypeInput> input)
        {
            LegalDocumentType data = new()
            {
            NameEng = input.PayLoad.NameEng
			, NameLocal = input.PayLoad.NameLocal
			, HasExpiry = input.PayLoad.HasExpiry
			, AllowMultiple = input.PayLoad.AllowMultiple

            };
            await _context.LegalDocumentTypes.AddAsync(data);
           var val = await _context.SaveChangesAsync();
           return new LegalDocumentTypePayload(data);
        }

        public async Task<LegalDocumentTypePayload> Update(GenericInput<UpdateLegalDocumentTypeInput> data)
        {
            var tmp = await  _context.LegalDocumentTypes.FirstOrDefaultAsync(r => r.Id == data.PayLoad.Id);
            if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
            						tmp.NameEng = data.PayLoad.NameEng;
			tmp.NameLocal = data.PayLoad.NameLocal;
			tmp.HasExpiry = data.PayLoad.HasExpiry;
			tmp.AllowMultiple = data.PayLoad.AllowMultiple;

            _context.LegalDocumentTypes.Update(tmp);
            await _context.SaveChangesAsync();
            return new LegalDocumentTypePayload(tmp);

        }

        public async Task Delete(LegalDocumentType data)
        {
            var tmp = await _context.LegalDocumentTypes.FirstOrDefaultAsync(r => r.Id == data.Id);
            if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
            _context.LegalDocumentTypes.Remove(tmp);
            await _context.SaveChangesAsync();
        }
    }
}