using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Persistence.DTO;
using ECCSA.BackOffice.Server.Model.Academy;
using ECCSA.BackOffice.Server.Persistence.Context;
using ECCSA.BackOffice.Server.Persistence.DTO.Academy;
using Microsoft.EntityFrameworkCore;

namespace ECCSA.BackOffice.Server.Persistence.Repositories.Academy
{
    public class ResourceDeliveryRepository 
    {
        private readonly EccsaContext _context;
        private readonly EccsaUserContext _userContext;
        public ResourceDeliveryRepository(EccsaContext context, EccsaUserContext userContext)
        {
            _context = context;
            _userContext = userContext;
        }

        public IQueryable<ResourceDelivery> ResourceDeliveries(Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return _context.ResourceDeliveries.Include(x => x.Resource)
.Include(x => x.Registration)
;
        }

        public async Task<ResourceDelivery> ResourceDeliveryById(Guid id, Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return await _context.ResourceDeliveries.Include(x => x.Resource)
.Include(x => x.Registration)
.FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task<ResourceDeliveryPayload> Create(GenericInput<CreateResourceDeliveryInput> input)
        {
            ResourceDelivery data = new()
            {
                			Id = Guid.NewGuid()
			, DeliveryMethod = input.PayLoad.DeliveryMethod
			, ResourceId = input.PayLoad.ResourceId
			, RegistrationId = input.PayLoad.RegistrationId
			, DeliveryDate = input.PayLoad.DeliveryDate

            };
            await _context.ResourceDeliveries.AddAsync(data);
           var val = await _context.SaveChangesAsync();
           return new ResourceDeliveryPayload(data);
        }

        public async Task<ResourceDeliveryPayload> Update(GenericInput<UpdateResourceDeliveryInput> data)
        {
            var tmp = await  _context.ResourceDeliveries.FirstOrDefaultAsync(r => r.Id == data.PayLoad.Id);
            if (tmp == null) throw new KeyNotFoundException("Resource Delivery not found");
            						tmp.DeliveryMethod = data.PayLoad.DeliveryMethod;
			tmp.ResourceId = data.PayLoad.ResourceId;
			tmp.RegistrationId = data.PayLoad.RegistrationId;
			tmp.DeliveryDate = data.PayLoad.DeliveryDate;

            _context.ResourceDeliveries.Update(tmp);
            await _context.SaveChangesAsync();
            return new ResourceDeliveryPayload(tmp);

        }

        public async Task Delete(ResourceDelivery data)
        {
            var tmp = await _context.ResourceDeliveries.FirstOrDefaultAsync(r => r.Id == data.Id);
            if (tmp == null) throw new KeyNotFoundException("Resource Delivery not found");
            _context.ResourceDeliveries.Remove(tmp);
            await _context.SaveChangesAsync();
        }
    }
}