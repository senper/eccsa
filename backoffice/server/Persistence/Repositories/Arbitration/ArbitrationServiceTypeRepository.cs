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
    public class ArbitrationServiceTypeRepository 
    {
        private readonly EccsaContext _context;
        private readonly EccsaUserContext _userContext;
        public ArbitrationServiceTypeRepository(EccsaContext context, EccsaUserContext userContext)
        {
            _context = context;
            _userContext = userContext;
        }

        public IQueryable<ArbitrationServiceType> ArbitrationServiceTypes(Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return _context.ArbitrationServiceTypes.Include(x => x.Services)
;
        }

        public async Task<ArbitrationServiceType> ArbitrationServiceTypeById(Guid id, Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return await _context.ArbitrationServiceTypes.Include(x => x.Services)
.FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task<ArbitrationServiceTypePayload> Create(GenericInput<CreateArbitrationServiceTypeInput> input)
        {
            ArbitrationServiceType data = new()
            {
                			Id = Guid.NewGuid()
			, NameEng = input.PayLoad.NameEng
			, NameLocal = input.PayLoad.NameLocal
			, RequiresThirdParty = input.PayLoad.RequiresThirdParty
			, DescriptionEng = input.PayLoad.DescriptionEng
			, DescriptionLocal = input.PayLoad.DescriptionLocal
			, ServiceCharge = input.PayLoad.ServiceCharge
			, CreatedBy = input.UserId
			, CreateStamp = DateTime.UtcNow
			, UpdatedBy = input.UserId
			, UpdateStamp = DateTime.UtcNow

            };
            await _context.ArbitrationServiceTypes.AddAsync(data);
           var val = await _context.SaveChangesAsync();
           return new ArbitrationServiceTypePayload(data);
        }

        public async Task<ArbitrationServiceTypePayload> Update(GenericInput<UpdateArbitrationServiceTypeInput> data)
        {
            var tmp = await  _context.ArbitrationServiceTypes.FirstOrDefaultAsync(r => r.Id == data.PayLoad.Id);
            if (tmp == null) throw new KeyNotFoundException("Service Type not found");
            						tmp.NameEng = data.PayLoad.NameEng;
			tmp.NameLocal = data.PayLoad.NameLocal;
			tmp.RequiresThirdParty = data.PayLoad.RequiresThirdParty;
			tmp.DescriptionEng = data.PayLoad.DescriptionEng;
			tmp.DescriptionLocal = data.PayLoad.DescriptionLocal;
			tmp.ServiceCharge = data.PayLoad.ServiceCharge;
									tmp.UpdatedBy = data.UserId;
			tmp.UpdateStamp = DateTime.UtcNow;

            _context.ArbitrationServiceTypes.Update(tmp);
            await _context.SaveChangesAsync();
            return new ArbitrationServiceTypePayload(tmp);

        }

        public async Task Delete(ArbitrationServiceType data)
        {
            var tmp = await _context.ArbitrationServiceTypes.FirstOrDefaultAsync(r => r.Id == data.Id);
            if (tmp == null) throw new KeyNotFoundException("Service Type not found");
            _context.ArbitrationServiceTypes.Remove(tmp);
            await _context.SaveChangesAsync();
        }
    }
}