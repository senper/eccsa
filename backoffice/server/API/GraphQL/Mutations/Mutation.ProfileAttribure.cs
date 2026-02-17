
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
        public async Task<ProfileAttriburePayload> CreateProfileAttribure(
                 CreateProfileAttribureInput data
               , [Service] IHttpContextAccessor contextAccessor
               , [Service] ProfileAttribureRepository  profileAttribureRepository
             // , [Service] IProfileAttribureEventSender eventSender 
       )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<CreateProfileAttribureInput>(data, userId);
            var val = await profileAttribureRepository.Create(data2);
            // await eventSender.SendAsync(data.TypeId, val);
            return val;
        }

        public async Task<ProfileAttriburePayload> UpdateProfileAttribure(
            UpdateProfileAttribureInput data
            , [Service] IHttpContextAccessor contextAccessor
            , [Service] ProfileAttribureRepository  profileAttribureRepository
         //, [Service] IProfileAttribureEventSender eventSender
        )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<UpdateProfileAttribureInput>(data, userId);
            var val = await profileAttribureRepository.Update(data2);
            // await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            return val;

        }

        public async Task DeleteProfileAttribure(ProfileAttribure data
        , [Service] ProfileAttribureRepository  profileAttribureRepository
            // , [Service] IProfileAttribureEventSender eventSender
            )
        {
           await profileAttribureRepository.Delete(data );
            //await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            //return val;
        }
    }
    
}
