
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
        public async Task<ArbitrationFollowupResultPayload> CreateArbitrationFollowupResult(
                 CreateArbitrationFollowupResultInput data
               , [Service] IHttpContextAccessor contextAccessor
               , [Service] ArbitrationFollowupResultRepository  arbitrationFollowupResultRepository
             // , [Service] IArbitrationFollowupResultEventSender eventSender 
       )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<CreateArbitrationFollowupResultInput>(data, userId);
            var val = await arbitrationFollowupResultRepository.Create(data2);
            // await eventSender.SendAsync(data.TypeId, val);
            return val;
        }

        public async Task<ArbitrationFollowupResultPayload> UpdateArbitrationFollowupResult(
            UpdateArbitrationFollowupResultInput data
            , [Service] IHttpContextAccessor contextAccessor
            , [Service] ArbitrationFollowupResultRepository  arbitrationFollowupResultRepository
         //, [Service] IArbitrationFollowupResultEventSender eventSender
        )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<UpdateArbitrationFollowupResultInput>(data, userId);
            var val = await arbitrationFollowupResultRepository.Update(data2);
            // await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            return val;

        }

        public async Task DeleteArbitrationFollowupResult(ArbitrationFollowupResult data
        , [Service] ArbitrationFollowupResultRepository  arbitrationFollowupResultRepository
            // , [Service] IArbitrationFollowupResultEventSender eventSender
            )
        {
           await arbitrationFollowupResultRepository.Delete(data );
            //await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            //return val;
        }
    }
    
}
