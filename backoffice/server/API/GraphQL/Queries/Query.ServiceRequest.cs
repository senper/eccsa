using System.Threading.Tasks;
using System.Linq;
using System;
using HotChocolate;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using ECCSA.BackOffice.Server.Model.Common.Servicing;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Servicing;

namespace ECCSA.BackOffice.Server.API.GraphQL.Queries
{
    public partial class Query
    {
        
        public IQueryable<ServiceRequest> ServiceRequests(
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] ServiceRequestRepository  serviceRequestRepository
        )
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return serviceRequestRepository.ServiceRequests(userId);
        }
    
        public async Task<ServiceRequest> ServiceRequestById(Guid id,
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] ServiceRequestRepository  serviceRequestRepository)
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return await serviceRequestRepository.ServiceRequestById(id, userId);
        }
    }
}
