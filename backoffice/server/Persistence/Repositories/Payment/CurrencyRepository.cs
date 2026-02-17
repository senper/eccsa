using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Persistence.DTO;
using ECCSA.BackOffice.Server.Model.Payment;
using ECCSA.BackOffice.Server.Persistence.Context;
using ECCSA.BackOffice.Server.Persistence.DTO.Payment;
using Microsoft.EntityFrameworkCore;

namespace ECCSA.BackOffice.Server.Persistence.Repositories.Payment
{
    public class CurrencyRepository 
    {
        private readonly EccsaContext _context;
        private readonly EccsaUserContext _userContext;
        public CurrencyRepository(EccsaContext context, EccsaUserContext userContext)
        {
            _context = context;
            _userContext = userContext;
        }

        public IQueryable<Currency> Currencies(Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return _context.Currencies;
        }

        public async Task<Currency> CurrencyById(string id, Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return await _context.Currencies.FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task<CurrencyPayload> Create(GenericInput<CreateCurrencyInput> input)
        {
            Currency data = new()
            {
                Name = input.PayLoad.Name

            };
            await _context.Currencies.AddAsync(data);
           var val = await _context.SaveChangesAsync();
           return new CurrencyPayload(data);
        }

        public async Task<CurrencyPayload> Update(GenericInput<UpdateCurrencyInput> data)
        {
            var tmp = await  _context.Currencies.FirstOrDefaultAsync(r => r.Id == data.PayLoad.Id);
            if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
            tmp.Name = data.PayLoad.Name;

            _context.Currencies.Update(tmp);
            await _context.SaveChangesAsync();
            return new CurrencyPayload(tmp);

        }

        public async Task Delete(Currency data)
        {
            var tmp = await _context.Currencies.FirstOrDefaultAsync(r => r.Id == data.Id);
            if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
            _context.Currencies.Remove(tmp);
            await _context.SaveChangesAsync();
        }
    }
}