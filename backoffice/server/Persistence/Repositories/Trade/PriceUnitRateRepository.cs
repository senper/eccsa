using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Persistence.DTO;
using ECCSA.BackOffice.Server.Model.Trade;
using ECCSA.BackOffice.Server.Persistence.Context;
using ECCSA.BackOffice.Server.Persistence.DTO.Trade;
using Microsoft.EntityFrameworkCore;

namespace ECCSA.BackOffice.Server.Persistence.Repositories.Trade
{
    public class PriceUnitRateRepository 
    {
        private readonly EccsaContext _context;
        private readonly EccsaUserContext _userContext;
        public PriceUnitRateRepository(EccsaContext context, EccsaUserContext userContext)
        {
            _context = context;
            _userContext = userContext;
        }

        public IQueryable<PriceUnitRate> PriceUnitRates(Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return _context.PriceUnitRates.Include(x => x.CommodityPrices)
;
        }

        public async Task<PriceUnitRate> PriceUnitRateById(Guid id, Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return await _context.PriceUnitRates.Include(x => x.CommodityPrices)
.FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task<PriceUnitRatePayload> Create(GenericInput<CreatePriceUnitRateInput> input)
        {
            PriceUnitRate data = new()
            {
                			Id = Guid.NewGuid()
			, Name = input.PayLoad.Name
			, NameLocal = input.PayLoad.NameLocal

            };
            await _context.PriceUnitRates.AddAsync(data);
           var val = await _context.SaveChangesAsync();
           return new PriceUnitRatePayload(data);
        }

        public async Task<PriceUnitRatePayload> Update(GenericInput<UpdatePriceUnitRateInput> data)
        {
            var tmp = await  _context.PriceUnitRates.FirstOrDefaultAsync(r => r.Id == data.PayLoad.Id);
            if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
            						tmp.Name = data.PayLoad.Name;
			tmp.NameLocal = data.PayLoad.NameLocal;

            _context.PriceUnitRates.Update(tmp);
            await _context.SaveChangesAsync();
            return new PriceUnitRatePayload(tmp);

        }

        public async Task Delete(PriceUnitRate data)
        {
            var tmp = await _context.PriceUnitRates.FirstOrDefaultAsync(r => r.Id == data.Id);
            if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
            _context.PriceUnitRates.Remove(tmp);
            await _context.SaveChangesAsync();
        }
    }
}