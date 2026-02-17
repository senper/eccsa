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
    public class ArbitrationFollowupResultRepository 
    {
        private readonly EccsaContext _context;
        private readonly EccsaUserContext _userContext;
        public ArbitrationFollowupResultRepository(EccsaContext context, EccsaUserContext userContext)
        {
            _context = context;
            _userContext = userContext;
        }

        public IQueryable<ArbitrationFollowupResult> ArbitrationFollowupResults(Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return _context.ArbitrationFollowupResults.Include(x => x.Services)
;
        }

        public async Task<ArbitrationFollowupResult> ArbitrationFollowupResultById(Guid id, Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return await _context.ArbitrationFollowupResults.Include(x => x.Services)
.FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task<ArbitrationFollowupResultPayload> Create(GenericInput<CreateArbitrationFollowupResultInput> input)
        {
            ArbitrationFollowupResult data = new()
            {
                			Id = Guid.NewGuid()
			, NameEng = input.PayLoad.NameEng
			, NameLocal = input.PayLoad.NameLocal
			, DescriptionEng = input.PayLoad.DescriptionEng
			, DescriptionLocal = input.PayLoad.DescriptionLocal

            };
            await _context.ArbitrationFollowupResults.AddAsync(data);
           var val = await _context.SaveChangesAsync();
           return new ArbitrationFollowupResultPayload(data);
        }

        public async Task<ArbitrationFollowupResultPayload> Update(GenericInput<UpdateArbitrationFollowupResultInput> data)
        {
            var tmp = await  _context.ArbitrationFollowupResults.FirstOrDefaultAsync(r => r.Id == data.PayLoad.Id);
            if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
            						tmp.NameEng = data.PayLoad.NameEng;
			tmp.NameLocal = data.PayLoad.NameLocal;
			tmp.DescriptionEng = data.PayLoad.DescriptionEng;
			tmp.DescriptionLocal = data.PayLoad.DescriptionLocal;

            _context.ArbitrationFollowupResults.Update(tmp);
            await _context.SaveChangesAsync();
            return new ArbitrationFollowupResultPayload(tmp);

        }

        public async Task Delete(ArbitrationFollowupResult data)
        {
            var tmp = await _context.ArbitrationFollowupResults.FirstOrDefaultAsync(r => r.Id == data.Id);
            if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
            _context.ArbitrationFollowupResults.Remove(tmp);
            await _context.SaveChangesAsync();
        }
    }
}