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
        
        public IQueryable<ArbitrationRelatedParty> ArbitrationRelatedParties(
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] ArbitrationRelatedPartyRepository  arbitrationRelatedPartyRepository
        )
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return arbitrationRelatedPartyRepository.ArbitrationRelatedParties(userId);
        }
    
        public async Task<ArbitrationRelatedParty> ArbitrationRelatedPartyById(Guid id,
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] ArbitrationRelatedPartyRepository  arbitrationRelatedPartyRepository)
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return await arbitrationRelatedPartyRepository.ArbitrationRelatedPartyById(id, userId);
        }
    }
}
