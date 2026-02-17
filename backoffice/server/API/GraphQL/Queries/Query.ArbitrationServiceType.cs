using ECCSA.BackOffice.Server.Model.Arbitration;
using ECCSA.BackOffice.Server.Persistence.Repositories.Arbitration;
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
        
        public IQueryable<ArbitrationServiceType> ArbitrationServiceTypes(
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] ArbitrationServiceTypeRepository  arbitrationServiceTypeRepository
        )
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return arbitrationServiceTypeRepository.ArbitrationServiceTypes(userId);
        }
    
        public async Task<ArbitrationServiceType> ArbitrationServiceTypeById(Guid id,
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] ArbitrationServiceTypeRepository  arbitrationServiceTypeRepository)
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return await arbitrationServiceTypeRepository.ArbitrationServiceTypeById(id, userId);
        }
    }
}
