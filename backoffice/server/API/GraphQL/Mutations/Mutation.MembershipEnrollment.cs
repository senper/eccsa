
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
        public async Task<MembershipEnrollmentPayload> CreateMembershipEnrollment(
                 CreateMembershipEnrollmentInput data
               , [Service] IHttpContextAccessor contextAccessor
               , [Service] MembershipEnrollmentRepository  membershipEnrollmentRepository
             // , [Service] IMembershipEnrollmentEventSender eventSender 
       )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<CreateMembershipEnrollmentInput>(data, userId);
            var val = await membershipEnrollmentRepository.Create(data2);
            // await eventSender.SendAsync(data.TypeId, val);
            return val;
        }

        public async Task<MembershipEnrollmentPayload> UpdateMembershipEnrollment(
            UpdateMembershipEnrollmentInput data
            , [Service] IHttpContextAccessor contextAccessor
            , [Service] MembershipEnrollmentRepository  membershipEnrollmentRepository
         //, [Service] IMembershipEnrollmentEventSender eventSender
        )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<UpdateMembershipEnrollmentInput>(data, userId);
            var val = await membershipEnrollmentRepository.Update(data2);
            // await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            return val;

        }

        public async Task DeleteMembershipEnrollment(MembershipEnrollment data
        , [Service] MembershipEnrollmentRepository  membershipEnrollmentRepository
            // , [Service] IMembershipEnrollmentEventSender eventSender
            )
        {
           await membershipEnrollmentRepository.Delete(data );
            //await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            //return val;
        }
    }
    
}
