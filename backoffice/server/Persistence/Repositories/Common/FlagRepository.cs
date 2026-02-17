using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Persistence.DTO;
using ECCSA.BackOffice.Server.Persistence.Context;
using ECCSA.BackOffice.Server.Persistence.DTO.Common;
using Microsoft.EntityFrameworkCore;

namespace ECCSA.BackOffice.Server.Persistence.Repositories.Common
{
    public class FlagRepository 
    {
        private readonly EccsaContext _context;
        private readonly EccsaUserContext _userContext;
        public FlagRepository(EccsaContext context, EccsaUserContext userContext)
        {
            _context = context;
            _userContext = userContext;
        }

        public IQueryable<Flag> Flags(Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return _context.Flags.Include(x => x.FlagType)
;
        }

        public async Task<Flag> FlagById(int id, Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return await _context.Flags.Include(x => x.FlagType)
.FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task<FlagPayload> Create(GenericInput<CreateFlagInput> input)
        {
            Flag data = new()
            {
            FlagTypeId = input.PayLoad.FlagTypeId
			, Name = input.PayLoad.Name
			, NameLocal = input.PayLoad.NameLocal

            };
            await _context.Flags.AddAsync(data);
           var val = await _context.SaveChangesAsync();
           return new FlagPayload(data);
        }

        public async Task<FlagPayload> Update(GenericInput<UpdateFlagInput> data)
        {
            var tmp = await  _context.Flags.FirstOrDefaultAsync(r => r.Id == data.PayLoad.Id);
            if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
            						tmp.FlagTypeId = data.PayLoad.FlagTypeId;
			tmp.Name = data.PayLoad.Name;
			tmp.NameLocal = data.PayLoad.NameLocal;

            _context.Flags.Update(tmp);
            await _context.SaveChangesAsync();
            return new FlagPayload(tmp);

        }

        public async Task Delete(Flag data)
        {
            var tmp = await _context.Flags.FirstOrDefaultAsync(r => r.Id == data.Id);
            if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
            _context.Flags.Remove(tmp);
            await _context.SaveChangesAsync();
        }
    }
}