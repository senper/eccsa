
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
        public async Task<RenewalHistoryPayload> CreateRenewalHistory(
                 CreateRenewalHistoryInput data
               , [Service] IHttpContextAccessor contextAccessor
               , [Service] RenewalHistoryRepository  renewalHistoryRepository
             // , [Service] IRenewalHistoryEventSender eventSender 
       )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<CreateRenewalHistoryInput>(data, userId);
            var val = await renewalHistoryRepository.Create(data2);
            // await eventSender.SendAsync(data.TypeId, val);
            return val;
        }

        public async Task<RenewalHistoryPayload> UpdateRenewalHistory(
            UpdateRenewalHistoryInput data
            , [Service] IHttpContextAccessor contextAccessor
            , [Service] RenewalHistoryRepository  renewalHistoryRepository
         //, [Service] IRenewalHistoryEventSender eventSender
        )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<UpdateRenewalHistoryInput>(data, userId);
            var val = await renewalHistoryRepository.Update(data2);
            // await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            return val;

        }

        public async Task DeleteRenewalHistory(RenewalHistory data
        , [Service] RenewalHistoryRepository  renewalHistoryRepository
            // , [Service] IRenewalHistoryEventSender eventSender
            )
        {
           await renewalHistoryRepository.Delete(data );
            //await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            //return val;
        }
    }
    
}
