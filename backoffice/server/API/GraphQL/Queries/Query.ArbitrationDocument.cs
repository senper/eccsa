using ECCSA.BackOffice.Server.Model.Arbitration;
using ECCSA.BackOffice.Server.Persistence.Repositories.Arbitration;
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
        
        public IQueryable<ArbitrationDocument> ArbitrationDocuments(
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] ArbitrationDocumentRepository  arbitrationDocumentRepository
        )
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return arbitrationDocumentRepository.ArbitrationDocuments(userId);
        }
    
        public async Task<ArbitrationDocument> ArbitrationDocumentById(Guid id,
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] ArbitrationDocumentRepository  arbitrationDocumentRepository)
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return await arbitrationDocumentRepository.ArbitrationDocumentById(id, userId);
        }
    }
}
