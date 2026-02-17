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
        
        public IQueryable<MediaCoverage> MediaCoverages(
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] MediaCoverageRepository  mediaCoverageRepository
        )
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            throw new NotImplementedException();
        }
    
        public async Task<MediaCoverage> MediaCoverageById(Guid id,
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] MediaCoverageRepository  mediaCoverageRepository)
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            throw new NotImplementedException();
        }
    }
}
