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
        
        public IQueryable<ApplicantDocument> ApplicantDocuments(
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] ApplicantDocumentRepository  applicantDocumentRepository
        )
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return applicantDocumentRepository.ApplicantDocuments(userId);
        }
    
        public async Task<ApplicantDocument> ApplicantDocumentById(Guid id,
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] ApplicantDocumentRepository  applicantDocumentRepository)
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return await applicantDocumentRepository.ApplicantDocumentById(id);
        }
    }
}
