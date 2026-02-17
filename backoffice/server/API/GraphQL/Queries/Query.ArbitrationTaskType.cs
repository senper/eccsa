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
        
        public IQueryable<ArbitrationTaskType> ArbitrationTasks(
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] ArbitrationTaskTypeRepository  arbitrationTaskTypeRepository
        )
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return arbitrationTaskTypeRepository.ArbitrationTasks(userId);
        }
    
        public async Task<ArbitrationTaskType> ArbitrationTaskTypeById(Guid id,
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] ArbitrationTaskTypeRepository  arbitrationTaskTypeRepository)
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return await arbitrationTaskTypeRepository.ArbitrationTaskTypeById(id, userId);
        }
    }
}
