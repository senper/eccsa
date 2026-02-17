
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
        public async Task<StaffPayload> CreateStaff(
                 CreateStaffInput data
               , [Service] IHttpContextAccessor contextAccessor
               , [Service] StaffRepository  staffRepository
             // , [Service] IStaffEventSender eventSender 
       )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<CreateStaffInput>(data, userId);
            var val = await staffRepository.Create(data2);
            // await eventSender.SendAsync(data.TypeId, val);
            return val;
        }

        public async Task<StaffPayload> UpdateStaff(
            UpdateStaffInput data
            , [Service] IHttpContextAccessor contextAccessor
            , [Service] StaffRepository  staffRepository
         //, [Service] IStaffEventSender eventSender
        )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<UpdateStaffInput>(data, userId);
            var val = await staffRepository.Update(data2);
            // await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            return val;

        }

        public async Task DeleteStaff(Staff data
        , [Service] StaffRepository  staffRepository
            // , [Service] IStaffEventSender eventSender
            )
        {
           await staffRepository.Delete(data.Id );
            //await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            //return val;
        }
    }
    
}
