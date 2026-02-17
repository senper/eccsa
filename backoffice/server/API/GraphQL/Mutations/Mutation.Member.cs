
using ECCSA.BackOffice.Server.Model.Academy;
using ECCSA.BackOffice.Server.Persistence.DTO;
using ECCSA.BackOffice.Server.Persistence.DTO.Academy;
using ECCSA.BackOffice.Server.Persistence.Repositories.Academy;
using HotChocolate;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ECCSA.BackOffice.Server.API.GraphQL.Mutations
{
    public partial class Mutation
    {
        public async Task<MemberPayload> CreateMember(
                 CreateMemberInput data
               , [Service] IHttpContextAccessor contextAccessor
               , [Service] MemberRepository  memberRepository
             // , [Service] IMemberEventSender eventSender 
       )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<CreateMemberInput>(data, userId);
            var val = await memberRepository.Create(data2);
            // await eventSender.SendAsync(data.TypeId, val);
            return val;
        }

        public async Task<MemberPayload> UpdateMember(
            UpdateMemberInput data
            , [Service] IHttpContextAccessor contextAccessor
            , [Service] MemberRepository  memberRepository
         //, [Service] IMemberEventSender eventSender
        )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<UpdateMemberInput>(data, userId);
            var val = await memberRepository.Update(data2);
            // await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            return val;

        }

        public async Task DeleteMember(Member data
        , [Service] MemberRepository  memberRepository
            // , [Service] IMemberEventSender eventSender
            )
        {
           await memberRepository.Delete(data );
            //await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            //return val;
        }
    }
    
}
