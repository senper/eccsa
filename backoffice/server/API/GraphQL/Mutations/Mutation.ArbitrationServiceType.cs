
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
        public async Task<ArbitrationServiceTypePayload> CreateArbitrationServiceType(
                 CreateArbitrationServiceTypeInput data
               , [Service] IHttpContextAccessor contextAccessor
               , [Service] ArbitrationServiceTypeRepository  arbitrationServiceTypeRepository
             // , [Service] IArbitrationServiceTypeEventSender eventSender 
       )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<CreateArbitrationServiceTypeInput>(data, userId);
            var val = await arbitrationServiceTypeRepository.Create(data2);
            // await eventSender.SendAsync(data.TypeId, val);
            return val;
        }

        public async Task<ArbitrationServiceTypePayload> UpdateArbitrationServiceType(
            UpdateArbitrationServiceTypeInput data
            , [Service] IHttpContextAccessor contextAccessor
            , [Service] ArbitrationServiceTypeRepository  arbitrationServiceTypeRepository
         //, [Service] IArbitrationServiceTypeEventSender eventSender
        )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<UpdateArbitrationServiceTypeInput>(data, userId);
            var val = await arbitrationServiceTypeRepository.Update(data2);
            // await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            return val;

        }

        public async Task DeleteArbitrationServiceType(ArbitrationServiceType data
        , [Service] ArbitrationServiceTypeRepository  arbitrationServiceTypeRepository
            // , [Service] IArbitrationServiceTypeEventSender eventSender
            )
        {
           await arbitrationServiceTypeRepository.Delete(data );
            //await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            //return val;
        }
    }
    
}
