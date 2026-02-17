
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
        public async Task<SurveyRespondentPayload> CreateSurveyRespondent(
                 CreateSurveyRespondentInput data
               , [Service] IHttpContextAccessor contextAccessor
               , [Service] SurveyRespondentRepository  surveyRespondentRepository
             // , [Service] ISurveyRespondentEventSender eventSender 
       )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<CreateSurveyRespondentInput>(data, userId);
            var val = await surveyRespondentRepository.Create(data2);
            // await eventSender.SendAsync(data.TypeId, val);
            return val;
        }

        public async Task<SurveyRespondentPayload> UpdateSurveyRespondent(
            UpdateSurveyRespondentInput data
            , [Service] IHttpContextAccessor contextAccessor
            , [Service] SurveyRespondentRepository  surveyRespondentRepository
         //, [Service] ISurveyRespondentEventSender eventSender
        )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<UpdateSurveyRespondentInput>(data, userId);
            var val = await surveyRespondentRepository.Update(data2);
            // await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            return val;

        }

        public async Task DeleteSurveyRespondent(SurveyRespondent data
        , [Service] SurveyRespondentRepository  surveyRespondentRepository
            // , [Service] ISurveyRespondentEventSender eventSender
            )
        {
           await surveyRespondentRepository.Delete(data );
            //await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            //return val;
        }
    }
    
}
