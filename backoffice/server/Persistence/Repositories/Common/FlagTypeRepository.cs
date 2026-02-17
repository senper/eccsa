using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Persistence.Context;
using ECCSA.BackOffice.Server.Persistence.DTO.Common;
using Microsoft.EntityFrameworkCore;

namespace ECCSA.BackOffice.Server.Persistence.Repositories.Common
{
    public class FlagTypeRepository 
    {
        private readonly EccsaContext _context;
        public FlagTypeRepository(EccsaContext context)
        {
            _context = context;
        }

        public IQueryable<FlagType> FlagTypes()
        {

            return _context.FlagTypes.Include(x => x.Flags)
;
        }

        public async Task<FlagType> FlagTypeById(int id)
        {
            return await _context.FlagTypes.Include(x => x.Flags)
.FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task<FlagTypePayload> Create(CreateFlagTypeInput input)
        {
            FlagType data = new()
            {
                						 Name = input.Name
			, NameLocal = input.NameLocal

            };
            await _context.FlagTypes.AddAsync(data);
           await _context.SaveChangesAsync();
           return new FlagTypePayload(data);
        }

        public async Task<FlagTypePayload> Update(UpdateFlagTypeInput data)
        {
            var tmp = await  _context.FlagTypes.FirstOrDefaultAsync(r => r.Id == data.Id);
            if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
            						tmp.Name = data.Name;
			tmp.NameLocal = data.NameLocal;

            _context.FlagTypes.Update(tmp);
            await _context.SaveChangesAsync();
            return new FlagTypePayload(tmp);

        }

        public async void Delete(FlagType data)
        {
            var tmp = await _context.FlagTypes.FirstOrDefaultAsync(r => r.Id == data.Id);
            if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
            _context.FlagTypes.Remove(tmp);
            await _context.SaveChangesAsync();
        }
    }
}