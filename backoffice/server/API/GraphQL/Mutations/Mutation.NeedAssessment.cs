
using ECCSA.BackOffice.Server.Model.Academy;
using ECCSA.BackOffice.Server.Persistence.DTO;
using ECCSA.BackOffice.Server.Persistence.DTO.Academy;
using ECCSA.BackOffice.Server.Persistence.Repositories.Academy;
using HotChocolate;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ECCSA.BackOffice.Server.API.GraphQL.Mutations
{
    public partial class Mutation
    {
        public async Task<NeedAssessmentPayload> CreateNeedAssessment(
                 CreateNeedAssessmentInput data
               , [Service] IHttpContextAccessor contextAccessor
               , [Service] NeedAssessmentRepository  needAssessmentRepository
             // , [Service] INeedAssessmentEventSender eventSender 
       )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<CreateNeedAssessmentInput>(data, userId);
            var val = await needAssessmentRepository.Create(data2);
            // await eventSender.SendAsync(data.TypeId, val);
            return val;
        }

        public async Task<NeedAssessmentPayload> UpdateNeedAssessment(
            UpdateNeedAssessmentInput data
            , [Service] IHttpContextAccessor contextAccessor
            , [Service] NeedAssessmentRepository  needAssessmentRepository
         //, [Service] INeedAssessmentEventSender eventSender
        )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<UpdateNeedAssessmentInput>(data, userId);
            var val = await needAssessmentRepository.Update(data2);
            // await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            return val;

        }

        public async Task DeleteNeedAssessment(NeedAssessment data
        , [Service] NeedAssessmentRepository  needAssessmentRepository
            // , [Service] INeedAssessmentEventSender eventSender
            )
        {
           await needAssessmentRepository.Delete(data );
            //await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            //return val;
        }
    }
    
}
