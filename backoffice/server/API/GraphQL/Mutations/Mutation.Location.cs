using ECCSA.BackOffice.Server.Persistence.DTO;
using ECCSA.BackOffice.Server.Persistence.DTO.Membership;
using ECCSA.BackOffice.Server.Persistence.Repositories.Membership;
using HotChocolate;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ECCSA.BackOffice.Server.API.GraphQL.Mutations
{
    public partial class Mutation
    {
        public async Task<LocationPayload> CreateLocation(
                 CreateLocationInput data
               , [Service] IHttpContextAccessor contextAccessor
               , [Service] LocationRepository  locationRepository
             // , [Service] ILocationEventSender eventSender 
       )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<CreateLocationInput>(data, userId);
            var val = await locationRepository.Create(data2);
            // await eventSender.SendAsync(data.TypeId, val);
            return val;
        }

        public async Task<LocationPayload> UpdateLocation(
            UpdateLocationInput data
            , [Service] IHttpContextAccessor contextAccessor
            , [Service] LocationRepository  locationRepository
         //, [Service] ILocationEventSender eventSender
        )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<UpdateLocationInput>(data, userId);
            var val = await locationRepository.Update(data2);
            // await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            return val;

        }

        public async Task DeleteLocation(Model.Membership.Location data
        , [Service] LocationRepository  locationRepository
            // , [Service] ILocationEventSender eventSender
            )
        {
           await locationRepository.Delete(data );
            //await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            //return val;
        }
    }
    
}
