
using ECCSA.BackOffice.Server.Model.Membership;
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
        public async Task<AddressPayload> CreateAddress(
                 CreateAddressInput data
               , [Service] IHttpContextAccessor contextAccessor
               , [Service] AddressRepository  addressRepository
             // , [Service] IAddressEventSender eventSender 
       )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<CreateAddressInput>(data, userId);
            var val = await addressRepository.Create(data2);
            // await eventSender.SendAsync(data.TypeId, val);
            return val;
        }

        public async Task<AddressPayload> UpdateAddress(
            UpdateAddressInput data
            , [Service] IHttpContextAccessor contextAccessor
            , [Service] AddressRepository  addressRepository
         //, [Service] IAddressEventSender eventSender
        )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<UpdateAddressInput>(data, userId);
            var val = await addressRepository.Update(data2);
            // await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            return val;

        }

        public async Task DeleteAddress(Address data
        , [Service] AddressRepository  addressRepository
            // , [Service] IAddressEventSender eventSender
            )
        {
           await addressRepository.Delete(data );
            //await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            //return val;
        }
    }
    
}
