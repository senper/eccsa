using ECCSA.BackOffice.Server.Model.Common.Servicing;
using ECCSA.BackOffice.Server.Persistence.DTO;
using ECCSA.BackOffice.Server.Persistence.DTO.Common;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common;
using HotChocolate;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ECCSA.BackOffice.Server.API.GraphQL.Mutations
{
    public partial class Mutation
    {
       // public async Task<ServiceTypePayload> CreateServiceType(
       //          CreateServiceTypeInput data
       //        , [Service] IHttpContextAccessor contextAccessor
       //        , [Service] ServiceTypeRepository  serviceTypeRepository
       //      // , [Service] IServiceTypeEventSender eventSender 
       //)
       // {
       //     var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
       //     var data2 = new GenericInput<CreateServiceTypeInput>(data, userId);
       //     var val = await serviceTypeRepository.Create(data2);
       //     // await eventSender.SendAsync(data.TypeId, val);
       //     return val;
       // }

        public async Task<ServiceTypePayload> UpdateServiceType(
            UpdateServiceTypeInput data
            , [Service] IHttpContextAccessor contextAccessor
         //, [Service] IServiceTypeEventSender eventSender
        )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<UpdateServiceTypeInput>(data, userId);
            // await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            return null;

        }

        public async Task DeleteServiceType(ServiceType data
            // , [Service] IServiceTypeEventSender eventSender
            )
        {
         //  await serviceTypeRepository.Delete(data );
            //await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            //return val;
        }
    }
    
}
