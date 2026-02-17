using ECCSA.BackOffice.Server.Model.Academy;
using ECCSA.BackOffice.Server.Persistence.Repositories.Academy;
using System.Threading.Tasks;
using System.Linq;
using System;
using HotChocolate;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ECCSA.BackOffice.Server.API.GraphQL.Queries
{
    public partial class Query
    {
        
        public IQueryable<Attendance> Attendances(
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] AttendanceRepository  attendanceRepository
        )
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return attendanceRepository.Attendances(userId);
        }
    
        public async Task<Attendance> AttendanceById(Guid id,
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] AttendanceRepository  attendanceRepository)
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return await attendanceRepository.AttendanceById(id, userId);
        }
    }
}
