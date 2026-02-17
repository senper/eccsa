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
        
        public IQueryable<ArbitrationFollowup> ArbitrationFollowups(
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] ArbitrationFollowupRepository  arbitrationFollowupRepository
        )
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return arbitrationFollowupRepository.ArbitrationFollowups(userId);
        }
    
        public async Task<ArbitrationFollowup> ArbitrationFollowupById(Guid id,
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] ArbitrationFollowupRepository  arbitrationFollowupRepository)
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return await arbitrationFollowupRepository.ArbitrationFollowupById(id, userId);
        }
    }
}
