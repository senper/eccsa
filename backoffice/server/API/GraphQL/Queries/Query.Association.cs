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
        
        public IQueryable<Association> Associations(
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] AssociationRepository  associationRepository
        )
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return associationRepository.Associations(userId);
        }
    
        public async Task<Association> AssociationById(Guid id,
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] AssociationRepository  associationRepository)
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return await associationRepository.AssociationById(id, userId);
        }
    }
}
