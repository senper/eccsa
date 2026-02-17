
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
        public async Task<ArbitrationServicePayload> CreateArbitrationService(
                 CreateArbitrationServiceInput data
               , [Service] IHttpContextAccessor contextAccessor
               , [Service] ArbitrationServiceRepository  arbitrationServiceRepository
             // , [Service] IArbitrationServiceEventSender eventSender 
       )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<CreateArbitrationServiceInput>(data, userId);
            var val = await arbitrationServiceRepository.Create(data2);
            // await eventSender.SendAsync(data.TypeId, val);
            return val;
        }

        public async Task<ArbitrationServicePayload> UpdateArbitrationService(
            UpdateArbitrationServiceInput data
            , [Service] IHttpContextAccessor contextAccessor
            , [Service] ArbitrationServiceRepository  arbitrationServiceRepository
         //, [Service] IArbitrationServiceEventSender eventSender
        )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<UpdateArbitrationServiceInput>(data, userId);
            var val = await arbitrationServiceRepository.Update(data2);
            // await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            return val;

        }

        public async Task DeleteArbitrationService(ArbitrationService data
        , [Service] ArbitrationServiceRepository  arbitrationServiceRepository
            // , [Service] IArbitrationServiceEventSender eventSender
            )
        {
           await arbitrationServiceRepository.Delete(data );
            //await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            //return val;
        }
    }
    
}
