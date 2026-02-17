
using ECCSA.BackOffice.Server.Model.Arbitration;
using ECCSA.BackOffice.Server.Persistence.DTO;
using ECCSA.BackOffice.Server.Persistence.DTO.Arbitration;
using ECCSA.BackOffice.Server.Persistence.Repositories.Arbitration;
using HotChocolate;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ECCSA.BackOffice.Server.API.GraphQL.Mutations
{
    public partial class Mutation
    {
        public async Task<ArbitrationFollowupPayload> CreateArbitrationFollowup(
                 CreateArbitrationFollowupInput data
               , [Service] IHttpContextAccessor contextAccessor
               , [Service] ArbitrationFollowupRepository  arbitrationFollowupRepository
             // , [Service] IArbitrationFollowupEventSender eventSender 
       )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<CreateArbitrationFollowupInput>(data, userId);
            var val = await arbitrationFollowupRepository.Create(data2);
            // await eventSender.SendAsync(data.TypeId, val);
            return val;
        }

        public async Task<ArbitrationFollowupPayload> UpdateArbitrationFollowup(
            UpdateArbitrationFollowupInput data
            , [Service] IHttpContextAccessor contextAccessor
            , [Service] ArbitrationFollowupRepository  arbitrationFollowupRepository
         //, [Service] IArbitrationFollowupEventSender eventSender
        )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<UpdateArbitrationFollowupInput>(data, userId);
            var val = await arbitrationFollowupRepository.Update(data2);
            // await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            return val;

        }

        public async Task DeleteArbitrationFollowup(ArbitrationFollowup data
        , [Service] ArbitrationFollowupRepository  arbitrationFollowupRepository
            // , [Service] IArbitrationFollowupEventSender eventSender
            )
        {
           await arbitrationFollowupRepository.Delete(data );
            //await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            //return val;
        }
    }
    
}
