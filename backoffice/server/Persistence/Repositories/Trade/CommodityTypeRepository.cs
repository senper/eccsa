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
    public class CommodityTypeRepository 
    {
        private readonly EccsaContext _context;
        private readonly EccsaUserContext _userContext;
        public CommodityTypeRepository(EccsaContext context, EccsaUserContext userContext)
        {
            _context = context;
            _userContext = userContext;
        }

        public IQueryable<CommodityType> CommodityTypes(Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return _context.CommodityTypes.Include(x => x.CommodityPrices)
;
        }

        public async Task<CommodityType> CommodityTypeById(Guid id, Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return await _context.CommodityTypes.Include(x => x.CommodityPrices)
.FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task<CommodityTypePayload> Create(GenericInput<CreateCommodityTypeInput> input)
        {
            CommodityType data = new()
            {
                			Id = Guid.NewGuid()
			, Name = input.PayLoad.Name
			, NameLocal = input.PayLoad.NameLocal

            };
            await _context.CommodityTypes.AddAsync(data);
           var val = await _context.SaveChangesAsync();
           return new CommodityTypePayload(data);
        }

        public async Task<CommodityTypePayload> Update(GenericInput<UpdateCommodityTypeInput> data)
        {
            var tmp = await  _context.CommodityTypes.FirstOrDefaultAsync(r => r.Id == data.PayLoad.Id);
            if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
            						tmp.Name = data.PayLoad.Name;
			tmp.NameLocal = data.PayLoad.NameLocal;

            _context.CommodityTypes.Update(tmp);
            await _context.SaveChangesAsync();
            return new CommodityTypePayload(tmp);

        }

        public async Task Delete(CommodityType data)
        {
            var tmp = await _context.CommodityTypes.FirstOrDefaultAsync(r => r.Id == data.Id);
            if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
            _context.CommodityTypes.Remove(tmp);
            await _context.SaveChangesAsync();
        }
    }
}