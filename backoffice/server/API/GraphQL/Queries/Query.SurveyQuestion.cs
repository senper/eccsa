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
        
        public IQueryable<SurveyQuestion> SurveyQuestions(
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] SurveyQuestionRepository  surveyQuestionRepository
        )
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return surveyQuestionRepository.SurveyQuestions(userId);
        }
    
        public async Task<SurveyQuestion> SurveyQuestionById(Guid id,
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] SurveyQuestionRepository  surveyQuestionRepository)
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return await surveyQuestionRepository.SurveyQuestionById(id, userId);
        }
    }
}
