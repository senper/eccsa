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
        
        public IQueryable<Assessment> Assessments(
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] AssessmentRepository  assessmentRepository
        )
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return assessmentRepository.Assessments(userId, Guid.Empty);
        }
    
        public async Task<Assessment> AssessmentById(Guid id,
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] AssessmentRepository  assessmentRepository)
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return await assessmentRepository.AssessmentById(id, userId);
        }
    }
}
