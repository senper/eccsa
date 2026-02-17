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
        
        public IQueryable<ArbitrationFollowupResult> ArbitrationFollowupResults(
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] ArbitrationFollowupResultRepository  arbitrationFollowupResultRepository
        )
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return arbitrationFollowupResultRepository.ArbitrationFollowupResults(userId);
        }
    
        public async Task<ArbitrationFollowupResult> ArbitrationFollowupResultById(Guid id,
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] ArbitrationFollowupResultRepository  arbitrationFollowupResultRepository)
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return await arbitrationFollowupResultRepository.ArbitrationFollowupResultById(id, userId);
        }
    }
}
