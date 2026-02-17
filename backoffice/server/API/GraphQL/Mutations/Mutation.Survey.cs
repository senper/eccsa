
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
        public async Task<SurveyPayload> CreateSurvey(
                 CreateSurveyInput data
               , [Service] IHttpContextAccessor contextAccessor
               , [Service] SurveyRepository  surveyRepository
             // , [Service] ISurveyEventSender eventSender 
       )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<CreateSurveyInput>(data, userId);
            var val = await surveyRepository.Create(data2);
            // await eventSender.SendAsync(data.TypeId, val);
            return val;
        }

        public async Task<SurveyPayload> UpdateSurvey(
            UpdateSurveyInput data
            , [Service] IHttpContextAccessor contextAccessor
            , [Service] SurveyRepository  surveyRepository
         //, [Service] ISurveyEventSender eventSender
        )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<UpdateSurveyInput>(data, userId);
            var val = await surveyRepository.Update(data2);
            // await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            return val;

        }

        public async Task DeleteSurvey(Survey data
        , [Service] SurveyRepository  surveyRepository
            // , [Service] ISurveyEventSender eventSender
            )
        {
           await surveyRepository.Delete(data );
            //await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            //return val;
        }
    }
    
}
