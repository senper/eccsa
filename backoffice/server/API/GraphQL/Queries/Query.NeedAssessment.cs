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
        
        public IQueryable<NeedAssessment> NeedAssessments(
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] NeedAssessmentRepository  needAssessmentRepository
        )
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return needAssessmentRepository.NeedAssessments(userId);
        }
    
        public async Task<NeedAssessment> NeedAssessmentById(Guid id,
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] NeedAssessmentRepository  needAssessmentRepository)
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return await needAssessmentRepository.NeedAssessmentById(id, userId);
        }
    }
}
