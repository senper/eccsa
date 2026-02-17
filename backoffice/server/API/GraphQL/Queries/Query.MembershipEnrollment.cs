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
        
        public IQueryable<MembershipEnrollment> MembershipEnrollments(
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] MembershipEnrollmentRepository  membershipEnrollmentRepository
        )
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return membershipEnrollmentRepository.MembershipEnrollments(Guid.Empty, userId);
        }
    
        public async Task<MembershipEnrollment> MembershipEnrollmentById(Guid id,
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] MembershipEnrollmentRepository  membershipEnrollmentRepository)
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return await membershipEnrollmentRepository.MembershipEnrollmentById(id, userId);
        }
    }
}
