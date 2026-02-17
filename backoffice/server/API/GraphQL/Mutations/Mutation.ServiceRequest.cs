using ECCSA.BackOffice.Server.Model.Common.Servicing;
using ECCSA.BackOffice.Server.Persistence.DTO;
using ECCSA.BackOffice.Server.Persistence.DTO.Common;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Servicing;
using HotChocolate;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ECCSA.BackOffice.Server.API.GraphQL.Mutations
{
    public partial class Mutation
    {
        public async Task<ServiceRequestPayload> CreateServiceRequest(
                 CreateServiceRequestInput data
               , [Service] IHttpContextAccessor contextAccessor
               , [Service] ServiceRequestRepository  serviceRequestRepository
             // , [Service] IServiceRequestEventSender eventSender 
       )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<CreateServiceRequestInput>(data, userId);
            var val = await serviceRequestRepository.Create(data2);
            // await eventSender.SendAsync(data.TypeId, val);
            return val;
        }

        public async Task<ServiceRequestPayload> UpdateServiceRequest(
            UpdateServiceRequestInput data
            , [Service] IHttpContextAccessor contextAccessor
            , [Service] ServiceRequestRepository  serviceRequestRepository
         //, [Service] IServiceRequestEventSender eventSender
        )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<UpdateServiceRequestInput>(data, userId);
            var val = await serviceRequestRepository.Update(data2);
            // await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            return val;

        }

        public async Task DeleteServiceRequest(ServiceRequest data
        , [Service] ServiceRequestRepository  serviceRequestRepository
            // , [Service] IServiceRequestEventSender eventSender
            )
        {
           await serviceRequestRepository.Delete(data );
            //await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            //return val;
        }
    }
    
}
