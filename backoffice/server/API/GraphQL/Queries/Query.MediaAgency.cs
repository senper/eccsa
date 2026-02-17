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
        
        public IQueryable<MediaAgency> MediaAgencies(
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] MediaAgencyRepository  mediaAgencyRepository
        )
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            throw new NotImplementedException();
               // return mediaAgencyRepository.MediaAgencies(userId);
        }
    
        public async Task<MediaAgency> MediaAgencyById(Guid id,
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] MediaAgencyRepository  mediaAgencyRepository)
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return await mediaAgencyRepository.MediaAgencyById(id, userId);
        }
    }
}
