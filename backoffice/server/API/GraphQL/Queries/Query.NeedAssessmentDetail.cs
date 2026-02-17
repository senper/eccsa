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
        
        public IQueryable<NeedAssessmentDetail> NeedAssessmentDetails(
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] NeedAssessmentDetailRepository  needAssessmentDetailRepository
        )
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return needAssessmentDetailRepository.NeedAssessmentDetails(userId);
        }
    
        public async Task<NeedAssessmentDetail> NeedAssessmentDetailById(Guid id,
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] NeedAssessmentDetailRepository  needAssessmentDetailRepository)
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return await needAssessmentDetailRepository.NeedAssessmentDetailById(id, userId);
        }
    }
}
