using ECCSA.BackOffice.Server.Model.Communication;
using ECCSA.BackOffice.Server.Persistence.Repositories.Communication;
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
        
        public IQueryable<Resource> Resources(
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] ResourceRepository  resourceRepository
        )
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return resourceRepository.Resources(userId);
        }
    
        public async Task<Resource> ResourceById(Guid id,
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] ResourceRepository  resourceRepository)
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return await resourceRepository.ResourceById(id, userId);
        }
    }
}
