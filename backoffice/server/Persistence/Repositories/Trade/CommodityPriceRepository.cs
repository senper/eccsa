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
    public class CommodityPriceRepository 
    {
        private readonly EccsaContext _context;
        private readonly EccsaUserContext _userContext;
        public CommodityPriceRepository(EccsaContext context, EccsaUserContext userContext)
        {
            _context = context;
            _userContext = userContext;
        }

        public IQueryable<CommodityPrice> CommodityPrices(Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return _context.CommodityPrices.Include(x => x.CommodityType)
.Include(x => x.PriceUnitRate)
;
        }

        public async Task<CommodityPrice> CommodityPriceById(Guid id, Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return await _context.CommodityPrices.Include(x => x.CommodityType)
.Include(x => x.PriceUnitRate)
.FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task<CommodityPricePayload> Create(GenericInput<CreateCommodityPriceInput> input)
        {
            CommodityPrice data = new()
            {
                			Id = Guid.NewGuid()
			, MarketDate = input.PayLoad.MarketDate
			, CommodityTypeId = input.PayLoad.CommodityTypeId
			, PriceUnitRateId = input.PayLoad.PriceUnitRateId
			, PriceValue = input.PayLoad.PriceValue

            };
            await _context.CommodityPrices.AddAsync(data);
           var val = await _context.SaveChangesAsync();
           return new CommodityPricePayload(data);
        }

        public async Task<CommodityPricePayload> Update(GenericInput<UpdateCommodityPriceInput> data)
        {
            var tmp = await  _context.CommodityPrices.FirstOrDefaultAsync(r => r.Id == data.PayLoad.Id);
            if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
            						tmp.MarketDate = data.PayLoad.MarketDate;
			tmp.CommodityTypeId = data.PayLoad.CommodityTypeId;
			tmp.PriceUnitRateId = data.PayLoad.PriceUnitRateId;
			tmp.PriceValue = data.PayLoad.PriceValue;

            _context.CommodityPrices.Update(tmp);
            await _context.SaveChangesAsync();
            return new CommodityPricePayload(tmp);

        }

        public async Task Delete(CommodityPrice data)
        {
            var tmp = await _context.CommodityPrices.FirstOrDefaultAsync(r => r.Id == data.Id);
            if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
            _context.CommodityPrices.Remove(tmp);
            await _context.SaveChangesAsync();
        }
    }
}