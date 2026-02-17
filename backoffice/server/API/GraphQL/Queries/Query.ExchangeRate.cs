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
        
        public IQueryable<ExchangeRate> ExchangeRates(
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] ExchangeRateRepository  exchangeRateRepository
        )
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return exchangeRateRepository.ExchangeRates(userId);
        }
    
        public async Task<ExchangeRate> ExchangeRateById(Guid id,
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] ExchangeRateRepository  exchangeRateRepository)
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return await exchangeRateRepository.ExchangeRateById(id, userId);
        }
    }
}
