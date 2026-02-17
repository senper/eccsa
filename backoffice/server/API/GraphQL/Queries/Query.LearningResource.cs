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
        
        public IQueryable<LearningResource> LearningResources(
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] LearningResourceRepository  learningResourceRepository
        )
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return learningResourceRepository.LearningResources(userId, Guid.Empty);
        }
    
        public async Task<LearningResource> LearningResourceById(Guid id,
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] LearningResourceRepository  learningResourceRepository)
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return await learningResourceRepository.LearningResourceById(id, userId);
        }
    }
}
