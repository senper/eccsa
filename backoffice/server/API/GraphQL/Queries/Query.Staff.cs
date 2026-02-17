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
        
        public IQueryable<Staff> Employees(
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] StaffRepository  staffRepository
        )
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return staffRepository.Employees(Guid.Empty, userId);
        }
    
        public async Task<Staff> StaffById(Guid id,
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] StaffRepository  staffRepository)
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return await staffRepository.StaffById(id, userId);
        }
    }
}
