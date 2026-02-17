
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
        public async Task<RepresentativePayload> CreateRepresentative(
                 CreateRepresentativeInput data
               , [Service] IHttpContextAccessor contextAccessor
               , [Service] RepresentativeRepository  representativeRepository
             // , [Service] IRepresentativeEventSender eventSender 
       )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<CreateRepresentativeInput>(data, userId);
            var val = await representativeRepository.Create(data2);
            // await eventSender.SendAsync(data.TypeId, val);
            return val;
        }

        public async Task<RepresentativePayload> UpdateRepresentative(
            UpdateRepresentativeInput data
            , [Service] IHttpContextAccessor contextAccessor
            , [Service] RepresentativeRepository  representativeRepository
         //, [Service] IRepresentativeEventSender eventSender
        )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<UpdateRepresentativeInput>(data, userId);
            var val = await representativeRepository.Update(data2);
            // await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            return val;

        }

        public async Task DeleteRepresentative(Representative data
        , [Service] RepresentativeRepository  representativeRepository
            // , [Service] IRepresentativeEventSender eventSender
            )
        {
           await representativeRepository.Delete(data );
            //await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            //return val;
        }
    }
    
}
