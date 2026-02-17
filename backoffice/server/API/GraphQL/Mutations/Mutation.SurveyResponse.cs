
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
        public async Task<SurveyResponsePayload> CreateSurveyResponse(
                 CreateSurveyResponseInput data
               , [Service] IHttpContextAccessor contextAccessor
               , [Service] SurveyResponseRepository  surveyResponseRepository
             // , [Service] ISurveyResponseEventSender eventSender 
       )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<CreateSurveyResponseInput>(data, userId);
            var val = await surveyResponseRepository.Create(data2);
            // await eventSender.SendAsync(data.TypeId, val);
            return val;
        }

        public async Task<SurveyResponsePayload> UpdateSurveyResponse(
            UpdateSurveyResponseInput data
            , [Service] IHttpContextAccessor contextAccessor
            , [Service] SurveyResponseRepository  surveyResponseRepository
         //, [Service] ISurveyResponseEventSender eventSender
        )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<UpdateSurveyResponseInput>(data, userId);
            var val = await surveyResponseRepository.Update(data2);
            // await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            return val;

        }

        public async Task DeleteSurveyResponse(SurveyResponse data
        , [Service] SurveyResponseRepository  surveyResponseRepository
            // , [Service] ISurveyResponseEventSender eventSender
            )
        {
           await surveyResponseRepository.Delete(data );
            //await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            //return val;
        }
    }
    
}
