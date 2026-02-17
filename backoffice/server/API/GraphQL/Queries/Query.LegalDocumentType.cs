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
        
        public IQueryable<LegalDocumentType> LegalDocumentTypes(
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] LegalDocumentTypeRepository  legalDocumentTypeRepository
        )
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return legalDocumentTypeRepository.LegalDocumentTypes(userId);
        }
    
        public async Task<LegalDocumentType> LegalDocumentTypeById(int id,
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] LegalDocumentTypeRepository  legalDocumentTypeRepository)
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return await legalDocumentTypeRepository.LegalDocumentTypeById(id, userId);
        }
    }
}
