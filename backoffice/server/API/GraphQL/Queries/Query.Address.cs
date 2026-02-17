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
        
        public IQueryable<Address> Addresses(
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] AddressRepository  addressRepository
        )
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return addressRepository.Addresses(userId);
        }
    
        public async Task<Address> AddressById(Guid id,
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] AddressRepository  addressRepository)
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return await addressRepository.AddressById(id, userId);
        }
    }
}
