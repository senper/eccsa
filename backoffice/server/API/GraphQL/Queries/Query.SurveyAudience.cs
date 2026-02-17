using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common;
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
        
        public IQueryable<SurveyAudience> SurveyAudiences(
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] SurveyAudienceRepository  surveyAudienceRepository
        )
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return surveyAudienceRepository.SurveyAudiences(userId);
        }
    
        public async Task<SurveyAudience> SurveyAudienceById(Guid id,
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] SurveyAudienceRepository  surveyAudienceRepository)
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return await surveyAudienceRepository.SurveyAudienceById(id, userId);
        }
    }
}
