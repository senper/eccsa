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
        
        public IQueryable<Campaign> Campaigns(
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] CampaignRepository  campaignRepository
        )
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return campaignRepository.Campaigns(userId);
        }
    
        public async Task<Campaign> CampaignById(Guid id,
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] CampaignRepository  campaignRepository)
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return await campaignRepository.CampaignById(id, userId);
        }
    }
}
