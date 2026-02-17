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
        
        public IQueryable<Representative> Representatives(
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] RepresentativeRepository  representativeRepository
        )
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return representativeRepository.Representatives(userId);
        }
    
        public async Task<Representative> RepresentativeById(Guid id,
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] RepresentativeRepository  representativeRepository)
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return await representativeRepository.RepresentativeById(id, userId);
        }
    }
}
