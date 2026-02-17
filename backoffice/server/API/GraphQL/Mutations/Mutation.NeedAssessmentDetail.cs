
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
        public async Task<NeedAssessmentDetailPayload> CreateNeedAssessmentDetail(
                 CreateNeedAssessmentDetailInput data
               , [Service] IHttpContextAccessor contextAccessor
               , [Service] NeedAssessmentDetailRepository  needAssessmentDetailRepository
             // , [Service] INeedAssessmentDetailEventSender eventSender 
       )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<CreateNeedAssessmentDetailInput>(data, userId);
            var val = await needAssessmentDetailRepository.Create(data2);
            // await eventSender.SendAsync(data.TypeId, val);
            return val;
        }

        public async Task<NeedAssessmentDetailPayload> UpdateNeedAssessmentDetail(
            UpdateNeedAssessmentDetailInput data
            , [Service] IHttpContextAccessor contextAccessor
            , [Service] NeedAssessmentDetailRepository  needAssessmentDetailRepository
         //, [Service] INeedAssessmentDetailEventSender eventSender
        )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<UpdateNeedAssessmentDetailInput>(data, userId);
            var val = await needAssessmentDetailRepository.Update(data2);
            // await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            return val;

        }

        public async Task DeleteNeedAssessmentDetail(NeedAssessmentDetail data
        , [Service] NeedAssessmentDetailRepository  needAssessmentDetailRepository
            // , [Service] INeedAssessmentDetailEventSender eventSender
            )
        {
           await needAssessmentDetailRepository.Delete(data );
            //await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            //return val;
        }
    }
    
}
