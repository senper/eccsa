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
        
        public IQueryable<Survey> Surveys(
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] SurveyRepository  surveyRepository
        )
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return surveyRepository.Surveys(userId);
        }
    
        public async Task<Survey> SurveyById(Guid id,
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] SurveyRepository  surveyRepository)
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return await surveyRepository.SurveyById(id, userId);
        }
    }
}
