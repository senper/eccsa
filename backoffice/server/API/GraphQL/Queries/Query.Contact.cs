using ECCSA.BackOffice.Server.Model.Membership;
using ECCSA.BackOffice.Server.Persistence.Repositories.Membership;
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
        
        public IQueryable<Contact> Contacts(
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] ContactRepository  contactRepository
        )
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return contactRepository.Contacts(userId);
        }
    
        public async Task<Contact> ContactById(Guid id,
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] ContactRepository  contactRepository)
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return await contactRepository.ContactById(id, userId);
        }
    }
}
