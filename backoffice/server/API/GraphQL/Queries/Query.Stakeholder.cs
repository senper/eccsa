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
        
        public IQueryable<Stakeholder> Stakeholders(
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] StakeholderRepository  stakeholderRepository
        )
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return stakeholderRepository.Stakeholders(userId);
        }
    
        public async Task<Stakeholder> StakeholderById(Guid id,
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] StakeholderRepository  stakeholderRepository)
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return await stakeholderRepository.StakeholderById(id, userId);
        }
    }
}
