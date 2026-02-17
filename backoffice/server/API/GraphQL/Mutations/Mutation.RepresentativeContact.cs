
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
        public async Task<RepresentativeContactPayload> CreateRepresentativeContact(
                 CreateRepresentativeContactInput data
               , [Service] IHttpContextAccessor contextAccessor
               , [Service] RepresentativeContactRepository  representativeContactRepository
             // , [Service] IRepresentativeContactEventSender eventSender 
       )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<CreateRepresentativeContactInput>(data, userId);
            var val = await representativeContactRepository.Create(data2);
            // await eventSender.SendAsync(data.TypeId, val);
            return val;
        }

        public async Task<RepresentativeContactPayload> UpdateRepresentativeContact(
            UpdateRepresentativeContactInput data
            , [Service] IHttpContextAccessor contextAccessor
            , [Service] RepresentativeContactRepository  representativeContactRepository
         //, [Service] IRepresentativeContactEventSender eventSender
        )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<UpdateRepresentativeContactInput>(data, userId);
            var val = await representativeContactRepository.Update(data2);
            // await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            return val;

        }

        public async Task DeleteRepresentativeContact(RepresentativeContact data
        , [Service] RepresentativeContactRepository  representativeContactRepository
            // , [Service] IRepresentativeContactEventSender eventSender
            )
        {
           await representativeContactRepository.Delete(data );
            //await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            //return val;
        }
    }
    
}
