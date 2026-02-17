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
        
        public IQueryable<CampaignPartnerAgency> CampaignPartnerAgencies(
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] CampaignPartnerAgencyRepository  campaignPartnerAgencyRepository
        )
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return campaignPartnerAgencyRepository.CampaignPartnerAgencies(userId);
        }
    
        public async Task<CampaignPartnerAgency> CampaignPartnerAgencyById(Guid id,
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] CampaignPartnerAgencyRepository  campaignPartnerAgencyRepository)
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return await campaignPartnerAgencyRepository.CampaignPartnerAgencyById(id, userId);
        }
    }
}
