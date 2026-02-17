
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
        public async Task<StakeholderPayload> CreateStakeholder(
                 CreateStakeholderInput data
               , [Service] IHttpContextAccessor contextAccessor
               , [Service] StakeholderRepository  stakeholderRepository
             // , [Service] IStakeholderEventSender eventSender 
       )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<CreateStakeholderInput>(data, userId);
            var val = await stakeholderRepository.Create(data2);
            // await eventSender.SendAsync(data.TypeId, val);
            return val;
        }

        public async Task<StakeholderPayload> UpdateStakeholder(
            UpdateStakeholderInput data
            , [Service] IHttpContextAccessor contextAccessor
            , [Service] StakeholderRepository  stakeholderRepository
         //, [Service] IStakeholderEventSender eventSender
        )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<UpdateStakeholderInput>(data, userId);
            var val = await stakeholderRepository.Update(data2);
            // await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            return val;

        }

        public async Task DeleteStakeholder(Stakeholder data
        , [Service] StakeholderRepository  stakeholderRepository
            // , [Service] IStakeholderEventSender eventSender
            )
        {
           await stakeholderRepository.Delete(data );
            //await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            //return val;
        }
    }
    
}
