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
        
        public IQueryable<SurveyRespondent> SurveyRespondents(
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] SurveyRespondentRepository  surveyRespondentRepository
        )
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return surveyRespondentRepository.SurveyRespondents(userId);
        }
    
        public async Task<SurveyRespondent> SurveyRespondentById(Guid id,
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] SurveyRespondentRepository  surveyRespondentRepository)
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return await surveyRespondentRepository.SurveyRespondentById(id, userId);
        }
    }
}
