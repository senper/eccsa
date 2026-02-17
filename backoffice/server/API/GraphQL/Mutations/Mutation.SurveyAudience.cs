
using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Persistence.DTO;
using ECCSA.BackOffice.Server.Persistence.DTO.Common;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common;
using HotChocolate;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ECCSA.BackOffice.Server.API.GraphQL.Mutations
{
    public partial class Mutation
    {
        public async Task<SurveyAudiencePayload> CreateSurveyAudience(
                 CreateSurveyAudienceInput data
               , [Service] IHttpContextAccessor contextAccessor
               , [Service] SurveyAudienceRepository  surveyAudienceRepository
             // , [Service] ISurveyAudienceEventSender eventSender 
       )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<CreateSurveyAudienceInput>(data, userId);
            var val = await surveyAudienceRepository.Create(data2);
            // await eventSender.SendAsync(data.TypeId, val);
            return val;
        }

        public async Task<SurveyAudiencePayload> UpdateSurveyAudience(
            UpdateSurveyAudienceInput data
            , [Service] IHttpContextAccessor contextAccessor
            , [Service] SurveyAudienceRepository  surveyAudienceRepository
         //, [Service] ISurveyAudienceEventSender eventSender
        )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<UpdateSurveyAudienceInput>(data, userId);
            var val = await surveyAudienceRepository.Update(data2);
            // await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            return val;

        }

        public async Task DeleteSurveyAudience(SurveyAudience data
        , [Service] SurveyAudienceRepository  surveyAudienceRepository
            // , [Service] ISurveyAudienceEventSender eventSender
            )
        {
           await surveyAudienceRepository.Delete(data );
            //await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            //return val;
        }
    }
    
}
