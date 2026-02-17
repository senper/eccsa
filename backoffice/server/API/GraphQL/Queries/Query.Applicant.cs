using ECCSA.BackOffice.Server.Model.Membership;
using ECCSA.BackOffice.Server.Persistence.Repositories.Membership;
using System.Threading.Tasks;
using System.Linq;
using System;
using HotChocolate;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Collections.Generic;

namespace ECCSA.BackOffice.Server.API.GraphQL.Queries
{
    public partial class Query
    {
        
        public IQueryable<Applicant> Applicants(
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] ApplicantRepository  applicantRepository
        )
        {
            var exceptions = string.Empty;
            try
            {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return applicantRepository.Applicants(userId);
            }
            catch(Exception ex)
            {
                exceptions = ex.Message;
                return new List<Applicant>() as IQueryable<Applicant>;
            }
        }
    
        public async Task<Applicant> ApplicantById(Guid id,
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] ApplicantRepository  applicantRepository)
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return await applicantRepository.ApplicantById(id, userId);
        }
    }
}
