using ECCSA.BackOffice.Server.Model.Academy;
using ECCSA.BackOffice.Server.Persistence.Repositories.Academy;
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
        
        public IQueryable<Sponsor> Sponsors(
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] SponsorRepository  sponsorRepository
        )
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return sponsorRepository.Sponsors(userId);
        }
    
        public async Task<Sponsor> SponsorById(Guid id,
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] SponsorRepository  sponsorRepository)
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return await sponsorRepository.SponsorById(id, userId);
        }
    }
}
