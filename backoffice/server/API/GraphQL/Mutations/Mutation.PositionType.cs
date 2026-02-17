
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
        public async Task<PositionTypePayload> CreatePositionType(
                 CreatePositionTypeInput data
               , [Service] IHttpContextAccessor contextAccessor
               , [Service] PositionTypeRepository  positionTypeRepository
             // , [Service] IPositionTypeEventSender eventSender 
       )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<CreatePositionTypeInput>(data, userId);
            var val = await positionTypeRepository.Create(data2);
            // await eventSender.SendAsync(data.TypeId, val);
            return val;
        }

        public async Task<PositionTypePayload> UpdatePositionType(
            UpdatePositionTypeInput data
            , [Service] IHttpContextAccessor contextAccessor
            , [Service] PositionTypeRepository  positionTypeRepository
         //, [Service] IPositionTypeEventSender eventSender
        )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<UpdatePositionTypeInput>(data, userId);
            var val = await positionTypeRepository.Update(data2);
            // await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            return val;

        }

        public async Task DeletePositionType(PositionType data
        , [Service] PositionTypeRepository  positionTypeRepository
            // , [Service] IPositionTypeEventSender eventSender
            )
        {
           await positionTypeRepository.Delete(data );
            //await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            //return val;
        }
    }
    
}
