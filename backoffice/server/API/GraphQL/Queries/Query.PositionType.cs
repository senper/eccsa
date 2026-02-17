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
        
        public IQueryable<PositionType> PositionTypes(
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] PositionTypeRepository  positionTypeRepository
        )
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return positionTypeRepository.PositionTypes(userId);
        }
    
        public async Task<PositionType> PositionTypeById(Guid id,
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] PositionTypeRepository  positionTypeRepository)
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return await positionTypeRepository.PositionTypeById(id, userId);
        }
    }
}
