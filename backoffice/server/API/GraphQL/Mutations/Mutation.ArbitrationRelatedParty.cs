
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
        public async Task<ArbitrationRelatedPartyPayload> CreateArbitrationRelatedParty(
                 CreateArbitrationRelatedPartyInput data
               , [Service] IHttpContextAccessor contextAccessor
               , [Service] ArbitrationRelatedPartyRepository  arbitrationRelatedPartyRepository
             // , [Service] IArbitrationRelatedPartyEventSender eventSender 
       )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<CreateArbitrationRelatedPartyInput>(data, userId);
            var val = await arbitrationRelatedPartyRepository.Create(data2);
            // await eventSender.SendAsync(data.TypeId, val);
            return val;
        }

        public async Task<ArbitrationRelatedPartyPayload> UpdateArbitrationRelatedParty(
            UpdateArbitrationRelatedPartyInput data
            , [Service] IHttpContextAccessor contextAccessor
            , [Service] ArbitrationRelatedPartyRepository  arbitrationRelatedPartyRepository
         //, [Service] IArbitrationRelatedPartyEventSender eventSender
        )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<UpdateArbitrationRelatedPartyInput>(data, userId);
            var val = await arbitrationRelatedPartyRepository.Update(data2);
            // await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            return val;

        }

        public async Task DeleteArbitrationRelatedParty(ArbitrationRelatedParty data
        , [Service] ArbitrationRelatedPartyRepository  arbitrationRelatedPartyRepository
            // , [Service] IArbitrationRelatedPartyEventSender eventSender
            )
        {
           await arbitrationRelatedPartyRepository.Delete(data );
            //await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            //return val;
        }
    }
    
}
