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
        
        public IQueryable<Model.Membership.Location> Locations(
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] LocationRepository  locationRepository
        )
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return locationRepository.Locations(userId);
        }
    
        public async Task<Model.Membership.Location> LocationById(Guid id,
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] LocationRepository  locationRepository)
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return await locationRepository.LocationById(id, userId);
        }
    }
}
