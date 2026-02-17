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
    public class LegalDocumentRepository
    {
        private readonly EccsaContext _context;
        private readonly EccsaUserContext _userContext;
        public LegalDocumentRepository(EccsaContext context, EccsaUserContext userContext)
        {
            _context = context;
            _userContext = userContext;
        }

        public IQueryable<LegalDocument> LegalDocuments(Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return _context.LegalDocuments.Include(x => x.Individual)
.Include(x => x.Requirement)
;
        }

        public async Task<LegalDocument> LegalDocumentById(Guid id, Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return await _context.LegalDocuments.Include(x => x.Individual)
.Include(x => x.Requirement)
.FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task<LegalDocumentPayload> Create(GenericInput<CreateLegalDocumentInput> input)
        {
            LegalDocument data = new()
            {
                Id = Guid.NewGuid()
            ,
                IndividualId = input.PayLoad.IndividualId

            };
            await _context.LegalDocuments.AddAsync(data);
             await _context.SaveChangesAsync();
            return new LegalDocumentPayload(data);
        }

        

       

        public async Task Delete(LegalDocument data)
        {
            var tmp = await _context.LegalDocuments.FirstOrDefaultAsync(r => r.Id == data.Id);
            if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
            _context.LegalDocuments.Remove(tmp);
            await _context.SaveChangesAsync();
        }
    }
}