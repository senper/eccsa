
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
        public async Task<MembershipRequirementPayload> CreateMembershipRequirement(
                 CreateMembershipRequirementInput data
               , [Service] IHttpContextAccessor contextAccessor
               , [Service] MembershipRequirementRepository  membershipRequirementRepository
             // , [Service] IMembershipRequirementEventSender eventSender 
       )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<CreateMembershipRequirementInput>(data, userId);
            var val = await membershipRequirementRepository.Create(data2);
            // await eventSender.SendAsync(data.TypeId, val);
            return val;
        }

        public async Task<MembershipRequirementPayload> UpdateMembershipRequirement(
            UpdateMembershipRequirementInput data
            , [Service] IHttpContextAccessor contextAccessor
            , [Service] MembershipRequirementRepository  membershipRequirementRepository
         //, [Service] IMembershipRequirementEventSender eventSender
        )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<UpdateMembershipRequirementInput>(data, userId);
            var val = await membershipRequirementRepository.Update(data2);
            // await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            return val;

        }

        public async Task DeleteMembershipRequirement(MembershipRequirement data
        , [Service] MembershipRequirementRepository  membershipRequirementRepository
            // , [Service] IMembershipRequirementEventSender eventSender
            )
        {
           await membershipRequirementRepository.Delete(data );
            //await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            //return val;
        }
    }
    
}
