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
        
        public IQueryable<LegalDocument> LegalDocuments(
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] LegalDocumentRepository  legalDocumentRepository
        )
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return legalDocumentRepository.LegalDocuments(userId);
        }
    
        public async Task<LegalDocument> LegalDocumentById(Guid id,
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] LegalDocumentRepository  legalDocumentRepository)
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return await legalDocumentRepository.LegalDocumentById(id, userId);
        }
    }
}
