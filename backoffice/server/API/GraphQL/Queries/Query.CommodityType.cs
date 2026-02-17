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
        
        public IQueryable<CommodityType> CommodityTypes(
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] CommodityTypeRepository  commodityTypeRepository
        )
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return commodityTypeRepository.CommodityTypes(userId);
        }
    
        public async Task<CommodityType> CommodityTypeById(Guid id,
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] CommodityTypeRepository  commodityTypeRepository)
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return await commodityTypeRepository.CommodityTypeById(id, userId);
        }
    }
}
