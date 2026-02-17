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
        
        public IQueryable<Individual> Individuals(
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] IndividualRepository  individualRepository
        )
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return individualRepository.Individuals(userId);
        }
    
        public async Task<Individual> IndividualById(Guid id,
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] IndividualRepository  individualRepository)
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return await individualRepository.IndividualById(id, userId);
        }
    }
}
