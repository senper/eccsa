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
        
        public IQueryable<SurveySetting> SurveySettings(
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] SurveySettingRepository  surveySettingRepository
        )
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return surveySettingRepository.SurveySettings(userId);
        }
    
        public async Task<SurveySetting> SurveySettingById(Guid id,
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] SurveySettingRepository  surveySettingRepository)
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return await surveySettingRepository.SettingById(id);
        }
    }
}
