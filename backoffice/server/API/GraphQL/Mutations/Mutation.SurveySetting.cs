
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
        public async Task<SurveySettingPayload> CreateSurveySetting(
                 CreateSurveySettingInput data
               , [Service] IHttpContextAccessor contextAccessor
               , [Service] SurveySettingRepository  surveySettingRepository
             // , [Service] ISurveySettingEventSender eventSender 
       )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<CreateSurveySettingInput>(data, userId);
            var val = await surveySettingRepository.Create(data2);
            // await eventSender.SendAsync(data.TypeId, val);
            return val;
        }

        public async Task<SurveySettingPayload> UpdateSurveySetting(
            UpdateSurveySettingInput data
            , [Service] IHttpContextAccessor contextAccessor
            , [Service] SurveySettingRepository  surveySettingRepository
         //, [Service] ISurveySettingEventSender eventSender
        )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<UpdateSurveySettingInput>(data, userId);
            var val = await surveySettingRepository.Update(data2);
            // await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            return val;

        }

        public async Task DeleteSurveySetting(SurveySetting data
        , [Service] SurveySettingRepository  surveySettingRepository
            // , [Service] ISurveySettingEventSender eventSender
            )
        {
           await surveySettingRepository.Delete(data );
            //await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            //return val;
        }
    }
    
}
