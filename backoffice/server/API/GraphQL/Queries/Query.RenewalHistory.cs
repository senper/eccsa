using ECCSA.BackOffice.Server.Model.Membership;
using ECCSA.BackOffice.Server.Persistence.Repositories.Membership;
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
        
        public IQueryable<RenewalHistory> RenewalHistory(
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] RenewalHistoryRepository  renewalHistoryRepository
        )
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return renewalHistoryRepository.RenewalHistory(userId);
        }
    
        public async Task<RenewalHistory> RenewalHistoryById(Guid id,
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] RenewalHistoryRepository  renewalHistoryRepository)
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return await renewalHistoryRepository.RenewalHistoryById(id, userId);
        }
    }
}
