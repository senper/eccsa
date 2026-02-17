
using ECCSA.BackOffice.Server.Model.Communication;
using ECCSA.BackOffice.Server.Persistence.DTO;
using ECCSA.BackOffice.Server.Persistence.DTO.Communication;
using ECCSA.BackOffice.Server.Persistence.Repositories.Communication;
using HotChocolate;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ECCSA.BackOffice.Server.API.GraphQL.Mutations
{
    public partial class Mutation
    {
        public async Task<ResourcePayload> CreateResource(
                 CreateResourceInput data
               , [Service] IHttpContextAccessor contextAccessor
               , [Service] ResourceRepository  resourceRepository
             // , [Service] IResourceEventSender eventSender 
       )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<CreateResourceInput>(data, userId);
            var val = await resourceRepository.Create(data2);
            // await eventSender.SendAsync(data.TypeId, val);
            return val;
        }

        public async Task<ResourcePayload> UpdateResource(
            UpdateResourceInput data
            , [Service] IHttpContextAccessor contextAccessor
            , [Service] ResourceRepository  resourceRepository
         //, [Service] IResourceEventSender eventSender
        )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<UpdateResourceInput>(data, userId);
            var val = await resourceRepository.Update(data2);
            // await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            return val;

        }

        public async Task DeleteResource(Resource data
        , [Service] ResourceRepository  resourceRepository
            // , [Service] IResourceEventSender eventSender
            )
        {
           await resourceRepository.Delete(data );
            //await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            //return val;
        }
    }
    
}
