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
        
        public IQueryable<Registration> Registrations(
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] RegistrationRepository  registrationRepository
        )
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return registrationRepository.Registrations(userId);
        }
    
        public async Task<Registration> RegistrationById(Guid id,
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] RegistrationRepository  registrationRepository)
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return await registrationRepository.RegistrationById(id, userId);
        }
    }
}
