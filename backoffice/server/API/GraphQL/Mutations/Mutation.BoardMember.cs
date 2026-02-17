
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
        public async Task<BoardMemberPayload> CreateBoardMember(
                 CreateBoardMemberInput data
               , [Service] IHttpContextAccessor contextAccessor
               , [Service] BoardMemberRepository  boardMemberRepository
             // , [Service] IBoardMemberEventSender eventSender 
       )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<CreateBoardMemberInput>(data, userId);
            var val = await boardMemberRepository.Create(data2);
            // await eventSender.SendAsync(data.TypeId, val);
            return val;
        }

        public async Task<BoardMemberPayload> UpdateBoardMember(
            UpdateBoardMemberInput data
            , [Service] IHttpContextAccessor contextAccessor
            , [Service] BoardMemberRepository  boardMemberRepository
         //, [Service] IBoardMemberEventSender eventSender
        )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<UpdateBoardMemberInput>(data, userId);
            var val = await boardMemberRepository.Update(data2);
            // await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            return val;

        }

        public async Task DeleteBoardMember(BoardMember data
        , [Service] BoardMemberRepository  boardMemberRepository
            // , [Service] IBoardMemberEventSender eventSender
            )
        {
           await boardMemberRepository.Delete(data );
            //await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            //return val;
        }
    }
    
}
