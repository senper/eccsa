using ECCSA.BackOffice.Server.Model.Trade;
using ECCSA.BackOffice.Server.Persistence.Repositories.Trade;
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
        
        public IQueryable<PriceUnitRate> PriceUnitRates(
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] PriceUnitRateRepository  priceUnitRateRepository
        )
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return priceUnitRateRepository.PriceUnitRates(userId);
        }
    
        public async Task<PriceUnitRate> PriceUnitRateById(Guid id,
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] PriceUnitRateRepository  priceUnitRateRepository)
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return await priceUnitRateRepository.PriceUnitRateById(id, userId);
        }
    }
}
