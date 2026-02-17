using ECCSA.BackOffice.Server.Model.Academy;
using ECCSA.BackOffice.Server.Persistence.Repositories.Academy;
using System.Threading.Tasks;
using System.Linq;
using System;
using HotChocolate;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ECCSA.BackOffice.Server.API.GraphQL.Queries
{
    public partial class Query
    {
        
        public IQueryable<ResourceDelivery> ResourceDeliveries(
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] ResourceDeliveryRepository  resourceDeliveryRepository
        )
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return resourceDeliveryRepository.ResourceDeliveries(userId);
        }
    
        public async Task<ResourceDelivery> ResourceDeliveryById(Guid id,
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] ResourceDeliveryRepository  resourceDeliveryRepository)
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return await resourceDeliveryRepository.ResourceDeliveryById(id, userId);
        }
    }
}
