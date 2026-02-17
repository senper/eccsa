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
        
        public IQueryable<Topic> Topics(
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] TopicRepository  topicRepository
        )
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return topicRepository.Topics(userId);
        }
    
        public async Task<Topic> TopicById(Guid id,
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] TopicRepository  topicRepository)
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return await topicRepository.TopicById(id, userId);
        }
    }
}
