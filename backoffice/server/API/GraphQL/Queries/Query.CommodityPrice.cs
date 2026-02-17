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
        
        public IQueryable<CommodityPrice> CommodityPrices(
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] CommodityPriceRepository  commodityPriceRepository
        )
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return commodityPriceRepository.CommodityPrices(userId);
        }
    
        public async Task<CommodityPrice> CommodityPriceById(Guid id,
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] CommodityPriceRepository  commodityPriceRepository)
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return await commodityPriceRepository.CommodityPriceById(id, userId);
        }
    }
}
