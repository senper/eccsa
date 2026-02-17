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
        
        public IQueryable<ArbitrationService> ArbitrationServices(
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] ArbitrationServiceRepository  arbitrationServiceRepository
        )
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return arbitrationServiceRepository.ArbitrationServices(userId);
        }
    
        public async Task<ArbitrationService> ArbitrationServiceById(Guid id,
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] ArbitrationServiceRepository  arbitrationServiceRepository)
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return await arbitrationServiceRepository.ArbitrationServiceById(id, userId);
        }
    }
}
