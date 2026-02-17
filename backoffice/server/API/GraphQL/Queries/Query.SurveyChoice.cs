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
        
        public IQueryable<SurveyChoice> SurveyChoices(
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] SurveyChoiceRepository  surveyChoiceRepository
        )
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return surveyChoiceRepository.SurveyChoices(userId);
        }
    
        public async Task<SurveyChoice> SurveyChoiceById(Guid id,
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] SurveyChoiceRepository  surveyChoiceRepository)
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return await surveyChoiceRepository.SurveyChoiceById(id, userId);
        }
    }
}
