
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
        public async Task<SurveyQuestionPayload> CreateSurveyQuestion(
                 CreateSurveyQuestionInput data
               , [Service] IHttpContextAccessor contextAccessor
               , [Service] SurveyQuestionRepository  surveyQuestionRepository
             // , [Service] ISurveyQuestionEventSender eventSender 
       )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<CreateSurveyQuestionInput>(data, userId);
            var val = await surveyQuestionRepository.Create(data2);
            // await eventSender.SendAsync(data.TypeId, val);
            return val;
        }

        public async Task<SurveyQuestionPayload> UpdateSurveyQuestion(
            UpdateSurveyQuestionInput data
            , [Service] IHttpContextAccessor contextAccessor
            , [Service] SurveyQuestionRepository  surveyQuestionRepository
         //, [Service] ISurveyQuestionEventSender eventSender
        )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<UpdateSurveyQuestionInput>(data, userId);
            var val = await surveyQuestionRepository.Update(data2);
            // await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            return val;

        }

        public async Task DeleteSurveyQuestion(SurveyQuestion data
        , [Service] SurveyQuestionRepository  surveyQuestionRepository
            // , [Service] ISurveyQuestionEventSender eventSender
            )
        {
           await surveyQuestionRepository.Delete(data );
            //await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            //return val;
        }
    }
    
}
