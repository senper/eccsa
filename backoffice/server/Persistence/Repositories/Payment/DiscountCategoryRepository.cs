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
    public class DiscountCategoryRepository 
    {
        private readonly EccsaContext _context;
        private readonly EccsaUserContext _userContext;
        public DiscountCategoryRepository(EccsaContext context, EccsaUserContext userContext)
        {
            _context = context;
            _userContext = userContext;
        }

        public IQueryable<DiscountCategory> DiscountCategories(Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return _context.DiscountCategories;
        }

        public async Task<DiscountCategory> DiscountCategoryById(int id, Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return await _context.DiscountCategories.FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task<DiscountCategoryPayload> Create(GenericInput<CreateDiscountCategoryInput> input)
        {
            DiscountCategory data = new()
            {
             Rate = input.PayLoad.Rate

            };
            await _context.DiscountCategories.AddAsync(data);
           var val = await _context.SaveChangesAsync();
           return new DiscountCategoryPayload(data);
        }

        public async Task<DiscountCategoryPayload> Update(GenericInput<UpdateDiscountCategoryInput> data)
        {
            var tmp = await  _context.DiscountCategories.FirstOrDefaultAsync(r => r.Id == data.PayLoad.Id);
            if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
            						tmp.Rate = data.PayLoad.Rate;

            _context.DiscountCategories.Update(tmp);
            await _context.SaveChangesAsync();
            return new DiscountCategoryPayload(tmp);

        }

        public async Task Delete(DiscountCategory data)
        {
            var tmp = await _context.DiscountCategories.FirstOrDefaultAsync(r => r.Id == data.Id);
            if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
            _context.DiscountCategories.Remove(tmp);
            await _context.SaveChangesAsync();
        }
    }
}