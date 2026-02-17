using ECCSA.BackOffice.Server.Persistence.Repositories.Common;
using System.Threading.Tasks;
using System.Linq;
using System;
using HotChocolate;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using ECCSA.BackOffice.Server.Model.Common.Servicing;

namespace ECCSA.BackOffice.Server.API.GraphQL.Queries
{
    public partial class Query
    {
        
        public IQueryable<ServiceType> ServiceTypes(
                [Service] IHttpContextAccessor contextAccessor
        )
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            return null;// serviceTypeRepository.ServiceTypes(userId);
        }
    
        public async Task<ServiceType> ServiceTypeById(Guid id,
                [Service] IHttpContextAccessor contextAccessor
               )
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            return null; // await serviceTypeRepository.ServiceTypeById(id, userId);
        }
    }
}
