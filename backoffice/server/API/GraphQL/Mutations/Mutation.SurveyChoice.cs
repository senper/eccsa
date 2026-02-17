
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
        public async Task<SurveyChoicePayload> CreateSurveyChoice(
                 CreateSurveyChoiceInput data
               , [Service] IHttpContextAccessor contextAccessor
               , [Service] SurveyChoiceRepository  surveyChoiceRepository
             // , [Service] ISurveyChoiceEventSender eventSender 
       )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<CreateSurveyChoiceInput>(data, userId);
            var val = await surveyChoiceRepository.Create(data2);
            // await eventSender.SendAsync(data.TypeId, val);
            return val;
        }

        public async Task<SurveyChoicePayload> UpdateSurveyChoice(
            UpdateSurveyChoiceInput data
            , [Service] IHttpContextAccessor contextAccessor
            , [Service] SurveyChoiceRepository  surveyChoiceRepository
         //, [Service] ISurveyChoiceEventSender eventSender
        )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<UpdateSurveyChoiceInput>(data, userId);
            var val = await surveyChoiceRepository.Update(data2);
            // await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            return val;

        }

        public async Task DeleteSurveyChoice(SurveyChoice data
        , [Service] SurveyChoiceRepository  surveyChoiceRepository
            // , [Service] ISurveyChoiceEventSender eventSender
            )
        {
           await surveyChoiceRepository.Delete(data );
            //await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            //return val;
        }
    }
    
}
