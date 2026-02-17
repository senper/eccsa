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
        
        public IQueryable<Category> Categories(
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] CategoryRepository  categoryRepository
        )
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return categoryRepository.Categories(userId);
        }
    
        public async Task<Category> CategoryById(int id,
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] CategoryRepository  categoryRepository)
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return await categoryRepository.CategoryById(id, userId);
        }
    }
}
