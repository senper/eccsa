
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
        public async Task<ArbitrationTaskTypePayload> CreateArbitrationTaskType(
                 CreateArbitrationTaskTypeInput data
               , [Service] IHttpContextAccessor contextAccessor
               , [Service] ArbitrationTaskTypeRepository  arbitrationTaskTypeRepository
             // , [Service] IArbitrationTaskTypeEventSender eventSender 
       )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<CreateArbitrationTaskTypeInput>(data, userId);
            var val = await arbitrationTaskTypeRepository.Create(data2);
            // await eventSender.SendAsync(data.TypeId, val);
            return val;
        }

        public async Task<ArbitrationTaskTypePayload> UpdateArbitrationTaskType(
            UpdateArbitrationTaskTypeInput data
            , [Service] IHttpContextAccessor contextAccessor
            , [Service] ArbitrationTaskTypeRepository  arbitrationTaskTypeRepository
         //, [Service] IArbitrationTaskTypeEventSender eventSender
        )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<UpdateArbitrationTaskTypeInput>(data, userId);
            var val = await arbitrationTaskTypeRepository.Update(data2);
            // await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            return val;

        }

        public async Task DeleteArbitrationTaskType(ArbitrationTaskType data
        , [Service] ArbitrationTaskTypeRepository  arbitrationTaskTypeRepository
            // , [Service] IArbitrationTaskTypeEventSender eventSender
            )
        {
           await arbitrationTaskTypeRepository.Delete(data );
            //await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            //return val;
        }
    }
    
}
