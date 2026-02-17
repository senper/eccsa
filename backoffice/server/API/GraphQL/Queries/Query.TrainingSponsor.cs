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
        
        public IQueryable<TrainingSponsor> TrainingSponsors(
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] TrainingSponsorRepository  trainingSponsorRepository
        )
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return trainingSponsorRepository.TrainingSponsors(userId, Guid.Empty);
        }
    
        public async Task<TrainingSponsor> TrainingSponsorById(Guid id,
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] TrainingSponsorRepository  trainingSponsorRepository)
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return await trainingSponsorRepository.TrainingSponsorById(id, userId);
        }
    }
}
