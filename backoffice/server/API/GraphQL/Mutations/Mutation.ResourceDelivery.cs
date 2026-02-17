
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
        public async Task<ResourceDeliveryPayload> CreateResourceDelivery(
                 CreateResourceDeliveryInput data
               , [Service] IHttpContextAccessor contextAccessor
               , [Service] ResourceDeliveryRepository  resourceDeliveryRepository
             // , [Service] IResourceDeliveryEventSender eventSender 
       )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<CreateResourceDeliveryInput>(data, userId);
            var val = await resourceDeliveryRepository.Create(data2);
            // await eventSender.SendAsync(data.TypeId, val);
            return val;
        }

        public async Task<ResourceDeliveryPayload> UpdateResourceDelivery(
            UpdateResourceDeliveryInput data
            , [Service] IHttpContextAccessor contextAccessor
            , [Service] ResourceDeliveryRepository  resourceDeliveryRepository
         //, [Service] IResourceDeliveryEventSender eventSender
        )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<UpdateResourceDeliveryInput>(data, userId);
            var val = await resourceDeliveryRepository.Update(data2);
            // await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            return val;

        }

        public async Task DeleteResourceDelivery(ResourceDelivery data
        , [Service] ResourceDeliveryRepository  resourceDeliveryRepository
            // , [Service] IResourceDeliveryEventSender eventSender
            )
        {
           await resourceDeliveryRepository.Delete(data );
            //await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            //return val;
        }
    }
    
}
