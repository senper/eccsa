
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
        public async Task<BusinessProfilePayload> CreateBusinessProfile(
                 CreateBusinessProfileInput data
               , [Service] IHttpContextAccessor contextAccessor
               , [Service] BusinessProfileRepository  businessProfileRepository
             // , [Service] IBusinessProfileEventSender eventSender 
       )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<CreateBusinessProfileInput>(data, userId);
            var val = await businessProfileRepository.Create(data2);
            // await eventSender.SendAsync(data.TypeId, val);
            return val;
        }

        public async Task<BusinessProfilePayload> UpdateBusinessProfile(
            UpdateBusinessProfileInput data
            , [Service] IHttpContextAccessor contextAccessor
            , [Service] BusinessProfileRepository  businessProfileRepository
         //, [Service] IBusinessProfileEventSender eventSender
        )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<UpdateBusinessProfileInput>(data, userId);
            var val = await businessProfileRepository.Update(data2);
            // await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            return val;

        }

        public async Task DeleteBusinessProfile(BusinessProfile data
        , [Service] BusinessProfileRepository  businessProfileRepository
            // , [Service] IBusinessProfileEventSender eventSender
            )
        {
           await businessProfileRepository.Delete(data );
            //await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            //return val;
        }
    }
    
}
