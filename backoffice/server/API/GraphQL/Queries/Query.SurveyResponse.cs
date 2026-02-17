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
        
        public IQueryable<SurveyResponse> SurveyResponses(
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] SurveyResponseRepository  surveyResponseRepository
        )
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return surveyResponseRepository.SurveyResponses(userId);
        }
    
        public async Task<SurveyResponse> SurveyResponseById(Guid id,
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] SurveyResponseRepository  surveyResponseRepository)
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return await surveyResponseRepository.SurveyResponseById(id, userId);
        }
    }
}
