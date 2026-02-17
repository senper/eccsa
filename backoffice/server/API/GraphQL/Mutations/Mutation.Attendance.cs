
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
        public async Task<AttendancePayload> CreateAttendance(
                 CreateAttendanceInput data
               , [Service] IHttpContextAccessor contextAccessor
               , [Service] AttendanceRepository  attendanceRepository
             // , [Service] IAttendanceEventSender eventSender 
       )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<CreateAttendanceInput>(data, userId);
            var val = await attendanceRepository.Create(data2);
            // await eventSender.SendAsync(data.TypeId, val);
            return val;
        }

        public async Task<AttendancePayload> UpdateAttendance(
            UpdateAttendanceInput data
            , [Service] IHttpContextAccessor contextAccessor
            , [Service] AttendanceRepository  attendanceRepository
         //, [Service] IAttendanceEventSender eventSender
        )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<UpdateAttendanceInput>(data, userId);
            var val = await attendanceRepository.Update(data2);
            // await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            return val;

        }

        public async Task DeleteAttendance(Attendance data
        , [Service] AttendanceRepository  attendanceRepository
            // , [Service] IAttendanceEventSender eventSender
            )
        {
           await attendanceRepository.Delete(data );
            //await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            //return val;
        }
    }
    
}
