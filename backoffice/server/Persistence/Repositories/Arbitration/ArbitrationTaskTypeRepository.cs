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
    public class ArbitrationTaskTypeRepository 
    {
        private readonly EccsaContext _context;
        private readonly EccsaUserContext _userContext;
        public ArbitrationTaskTypeRepository(EccsaContext context, EccsaUserContext userContext)
        {
            _context = context;
            _userContext = userContext;
        }

        public IQueryable<ArbitrationTaskType> ArbitrationTasks(Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return _context.ArbitrationTasks.Include(x => x.Services)
;
        }

        public async Task<ArbitrationTaskType> ArbitrationTaskTypeById(Guid id, Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return await _context.ArbitrationTasks.Include(x => x.Services)
.FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task<ArbitrationTaskTypePayload> Create(GenericInput<CreateArbitrationTaskTypeInput> input)
        {
            ArbitrationTaskType data = new()
            {
                			Id = Guid.NewGuid()
			, NameEng = input.PayLoad.NameEng
			, NameLocal = input.PayLoad.NameLocal
			, DescriptionEng = input.PayLoad.DescriptionEng
			, DescriptionLocal = input.PayLoad.DescriptionLocal
			, ServiceCharge = input.PayLoad.ServiceCharge

            };
            await _context.ArbitrationTasks.AddAsync(data);
           var val = await _context.SaveChangesAsync();
           return new ArbitrationTaskTypePayload(data);
        }

        public async Task<ArbitrationTaskTypePayload> Update(GenericInput<UpdateArbitrationTaskTypeInput> data)
        {
            var tmp = await  _context.ArbitrationTasks.FirstOrDefaultAsync(r => r.Id == data.PayLoad.Id);
            if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
            						tmp.NameEng = data.PayLoad.NameEng;
			tmp.NameLocal = data.PayLoad.NameLocal;
			tmp.DescriptionEng = data.PayLoad.DescriptionEng;
			tmp.DescriptionLocal = data.PayLoad.DescriptionLocal;
			tmp.ServiceCharge = data.PayLoad.ServiceCharge;

            _context.ArbitrationTasks.Update(tmp);
            await _context.SaveChangesAsync();
            return new ArbitrationTaskTypePayload(tmp);

        }

        public async Task Delete(ArbitrationTaskType data)
        {
            var tmp = await _context.ArbitrationTasks.FirstOrDefaultAsync(r => r.Id == data.Id);
            if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
            _context.ArbitrationTasks.Remove(tmp);
            await _context.SaveChangesAsync();
        }
    }
}